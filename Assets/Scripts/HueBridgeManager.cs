using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using MiniJSON;
using System.Text;

public class HueBridgeManager : MonoBehaviour {

    [Tooltip("IP address of the hue bridge: https://www.meethue.com/api/nupnp")]
    public string bridgeip = "127.0.0.1";
    public int portNumber = 8000;
    [Tooltip("Developer username")]
    public string username = "newdeveloper";

    public UnityWebRequest lights_json;
    public UnityWebRequest bridgeJson;
    public List<SmartLight> smartLights = null;

    private string parsedBridgeip;
    private bool bridgeFound;

    private GameObject hologramCollection;
    private SmartLightManager slm;

    // TODO remove mock data
    MockSmartLights mockLights;

    void Awake()
    {
        smartLights = new List<SmartLight>();
        if (GameObject.Find("HologramCollection") != null)
        {
            hologramCollection = GameObject.Find("HologramCollection");
            slm = hologramCollection.GetComponent<SmartLightManager>();
        }
        else
        {
            Debug.LogError("No GameObject name HologramCollection can be found. This object should contain all holograms and the SmartLightManager");
        }   
    }

    void Start()
    {
       // MOCK smart lights for testing
        mockLights = new MockSmartLights();
        smartLights = mockLights.getLights();
        slm.InitSmartLightManager(smartLights);
        //convertLightData();
        // MOCK end mock setup

        if ((bridgeip != "127.0.0.1" || bridgeip != "") && (username != "newdeveloper"))
        {
            if (StateManager.Instance.Starting)
            {
                StateManager.Instance.CurrentState = StateManager.HueAppState.ConnectedDevices_Initializing;
                StartCoroutine(DiscoverLights(lightDataToClass));
            }
            else
            {
                Debug.Log("There was an error with the app startup state");
            }
        }
        else
        {
            State sState = new State(true, 254, 0, 254, "none", "none");
            sState.Bri = 100;
            StartCoroutine(CheckOrGetBridgeIP());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            SmartLightManager.lights[0].State.Bri = 200;
            SmartLightManager.UpdateLightState(0);
        }
        if (Input.GetKeyDown("w"))
        {
            SmartLightManager.lights[0].State.Bri = 155;
            SmartLightManager.UpdateLightState(0);
        }
        if (Input.GetKeyDown("p"))
        {
            SmartLightManager.UpdateLightHue(2, 212);
        }
        if (Input.GetKeyDown("o"))
        {
            SmartLightManager.UpdateLightSaturation(2, 202);
        }
    }

    public IEnumerator CheckOrGetBridgeIP()
    {
        string url = "https://www.meethue.com/api/nupnp";
        bridgeJson = UnityWebRequest.Get(url);
        if (bridgeJson.error != null)
        {
            // TODO - message to be displayed on headset
            Debug.Log("there was an error attempting to reach bridge");
        }
        yield return bridgeJson.Send();

        if (bridgeJson.isError)
        {
            // TODO - message to be displayed on headset
            
            Debug.Log("There was an error attempting to discover bridge ip");
            yield break;
        }
        Debug.Log(bridgeJson.downloadHandler.text);

        if (bridgeJson.downloadHandler.text != "[]")
        {
            bridgeFound = true;
        }

        if (bridgeFound)
        {
            string bridgeJsonValue = bridgeJson.downloadHandler.text;

            string[] bridgeResponse = bridgeJsonValue.Split(',');

            foreach (string s in bridgeResponse)
            {
                if (s.Contains("internalipaddress"))
                {
                    parsedBridgeip = s;
                }

            }

            // TODO will need a more robust system for parsing ip
            var charsToRemove = new string[] { "internalipaddress", "\"", ":", "}", "]" };
            foreach (var c in charsToRemove)
            {
                parsedBridgeip = parsedBridgeip.Replace(c, string.Empty);
            }

            Debug.Log("bridge found: " + parsedBridgeip);
            bridgeip = parsedBridgeip;
            StartCoroutine(CreateBridgeUser(parsedBridgeip));
        }
        else
        {
            Debug.Log("No bridge was discovered on the current network. Refer to https://www.meethue.com/api/nupnp");
        }
        //Debug.Log("Please enter your Bridge IP and username");
    }

    public IEnumerator CreateBridgeUser(string ip)
    {
        string url = "http://" + ip +"/api";

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"devicetype\": \"hololenshue#android\"}");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        if (request.isError)
        {
            // TODO - message to be displayed on headset
            Debug.LogError("The request timed out. Please check your Bridge IP and internet connection");
            yield break;
        }

        if (request.downloadHandler.text.Contains("\"error\":{\"type\":101"))
        {
            // TODO - message to be displayed on headset
            Debug.Log("Please press the link button on your Bridge and try again");
        }
        Debug.Log("user json: " + request.downloadHandler.text);
        Debug.Log(request);
    }

    public IEnumerator DiscoverLights(Action nextAction)
    {
        string url = "http://" + bridgeip + "/api/" + username + "/lights";
        Debug.Log("Request url: " + url);

        lights_json = UnityWebRequest.Get(url);
        if (lights_json.error != null)
        {
            Debug.Log("There was an error. Your request was not sent");
        }
        yield return lights_json.Send();

        if (lights_json.isError)
        {
            // TODO - message to be displayed on headset
            Debug.LogError("The request timed out. Please check your Bridge IP and internet connection");
            yield break;
        }

        string jsonValue = lights_json.downloadHandler.text;
        if (jsonValue.Contains("error"))
        {
            if (jsonValue.Contains("unauthorized"))
            {
                Debug.LogError("Unauthorized user. Please add valid Username to Hue Bridge Manager.");
            }
            else
            {
                // TODO - message to be displayed on headset
                Debug.LogError("A bridge could not be found or could not be accessed at this time.");
                StateManager.Instance.CurrentState = StateManager.HueAppState.ConnectedDevices_Failed;
            }
        }
        else
        {
            Debug.Log(lights_json.downloadHandler.text);
            StateManager.Instance.CurrentState = StateManager.HueAppState.ConnectedDevices_Initialized;
            GetComponent<VoiceManager>().RegisterPhrases();
            nextAction();
        }
    }

    void lightDataToClass()
    {
        if (StateManager.Instance.ConnectedDevices_Initialized && lights_json != null)
        {
            var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);

            foreach (string key in lights.Keys)
            {
                // init state types
                bool on;
                int id, bri, hue, sat;
                string effect, alert;

                var light = (Dictionary<string, object>)lights[key];
                //var state = (Dictionary<string, dynamic>)light["state"];

                //// converting needs to be done prior to instantiating new SmartLight
                //on = Convert.ToBoolean(state["on"]);
                //bri = Convert.ToInt32(state["bri"]);
                //hue = Convert.ToInt32(state["hue"]);
                //sat = Convert.ToInt32(state["sat"]);
                //effect = Convert.ToString(state["effect"]);
                //alert = Convert.ToString(state["alert"]);

                id = Convert.ToInt32(key);
                State smartLightState = new State(true, 254, 0, 254, "none", "none");
                //State smartLightState = new State(on, bri, hue, sat, effect, alert);
                smartLights.Add(new SmartLight(id, light["name"].ToString(), light["modelid"].ToString(), smartLightState));
            }
            // sends collection of lights to the SmartLightManager to handle all future changes
            if (slm != null)
            {
                slm.InitSmartLightManager(smartLights);
            }
            else
            {
                Debug.LogError("No SmartLightManager was found. Please check inspector and be sure the correct GameObject is set on the HueBridgeManager");
            }
        }
        else
        {
            // TODO - message to be displayed on headset
            Debug.LogError("No lights can be found. Please ensure the Bridge IP is set and your lighting system is functioning properly");
        }
    }

    public void TestPut()
    {
        //State testState = smartLights[0].getState();
        //testState.isOn(false);
        //string request = "http://" + bridgeip + "/api/" + username + "/lights/1/state";
        //string json = JsonUtility.ToJson(testState);
        //JsonUtility.FromJson<State>(json);

        //UnityWebRequest setLight = UnityWebRequest.Put(request, json);
        //Debug.Log("Send triggered to " + request);
        //setLight.Send();
    }

}
