using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using MiniJSON;
using Newtonsoft.Json;
using System.Text;

public class HueBridgeManager : MonoBehaviour {

    [Tooltip("IP address of the hue bridge: https://www.meethue.com/api/nupnp")]
    public string bridgeip = "127.0.0.1";
    public int portNumber = 8000;
    [Tooltip("Developer username")]
    public string username = "newdeveloper";

    public List<HueDevice> devices;

    private List<SmartLight> smartLights;

    private string parsedBridgeip;
    private bool bridgeFound;

    private string bridgeIpUsername;

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
            //mockLights = new MockSmartLights();
            //smartLights = mockLights.getLights();
            //slm.InitSmartLightManager(smartLights);
            //convertLightData();
        // MOCK end mock setup

        if ((bridgeip != "127.0.0.1" || bridgeip != "") && (username != "newdeveloper" && username != ""))
        {
            if (StateManager.Instance.Starting)
            {
                bridgeReady();
            }
            else
            {
                string message = "There was an error with the app startup state";
                Debug.Log(message);
                Notification notification = new Notification("error", message);
                NotificationManager.DisplayNotification(notification);
            }
        }
        else
        {
            StartCoroutine(CheckOrGetBridgeIP());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            SmartLightManager.lights[0].State.Bri = 200;
            SmartLightManager.UpdateLightState(0);

            Notification notification = new Notification("error", "There was an error with the app startup state");
            NotificationManager.DisplayNotification(notification);
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
        UnityWebRequest request = UnityWebRequest.Get(url);
        if (request.error != null)
        {
            // TODO - message to be displayed on headset
            Debug.Log("there was an error attempting to reach bridge");
        }
        yield return request.Send();

        if (request.isError)
        {
            // TODO - message to be displayed on headset           
            Debug.Log("There was an error attempting to discover bridge ip");
            yield break;
        }
        Debug.Log(request.downloadHandler.text);

        if (request.downloadHandler.text != "[]")
        {
            bridgeFound = true;
        }

        if (bridgeFound)
        {
            string bridgeJsonValue = request.downloadHandler.text;

            if (bridgeJsonValue != null)
            {
                devices = JsonConvert.DeserializeObject<List<HueDevice>>(bridgeJsonValue);

                // TODO - only sending the ip of the first bridge found. Will need to handle multiple briges found cases
                string firstBridgeIP = devices[0].internalipaddress;
                bridgeip = firstBridgeIP;
                StartCoroutine(CheckOrCreateBridgeUser(bridgeip));
            }
            else
            {
                Debug.Log("no JSON data was returned from the bridge");
            }
        }
        else
        {
            Debug.Log("No bridge was discovered on the current network. Refer to https://www.meethue.com/api/nupnp");
        }
    }

    public IEnumerator CheckOrCreateBridgeUser(string ip)
    {
        // if a username has been manually added to the inspector, bypass checking from or creating to the device
        if (username != null && username != "newdeveloper" && username != "")
        {
            Debug.Log("existing username found: " + username);
        }
        else
        {
            // check if this Hue Bridge already has a valid username
            StartCoroutine(GetUsername(ip));
            // if a valid username is saved to the device, set as current user
            if (bridgeIpUsername != null && bridgeIpUsername != "newdeveloper" && bridgeIpUsername != "")
            {
                username = bridgeIpUsername;
                Debug.Log("An existing username has been retrieved from the device");
            }
            // if no username was associated with the Hue Bridge, create and store one
            else
            {
                string url = "http://" + ip + "/api";
                UnityWebRequest request = new UnityWebRequest(url, "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"devicetype\": \"hololenshue#hololens\"}");
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
                    yield break;
                }
                List<HueUser> users = JsonConvert.DeserializeObject<List<HueUser>>(request.downloadHandler.text);
                if (users[0].success != null)
                {
                    username = users[0].success.username;
                }
                StartCoroutine(SetUsername(ip, username));
                Debug.Log("A new username has been created: " + bridgeIpUsername);
            }          
        }
        // Bridgeid and username are both found and valid. Ready to make lighting requests
        bridgeReady();
    }

    private void bridgeReady()
    {
        StateManager.Instance.CurrentState = StateManager.HueAppState.ConnectedDevices_Initializing;
        StartCoroutine(DiscoverLights());
    }

    public IEnumerator DiscoverLights()
    {
        string url = "http://" + bridgeip + "/api/" + username + "/lights";
        Debug.Log("Request url: " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);
        if (request.error != null)
        {
            Debug.Log("There was an error. Your request was not sent");
        }
        yield return request.Send();

        if (request.isError)
        {
            // TODO - message to be displayed on headset
            Debug.LogError("The request timed out. Please check your Bridge IP and internet connection");
            yield break;
        }

        string json = request.downloadHandler.text;
        if (json.Contains("error"))
        {
            if (json.Contains("unauthorized"))
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
            StateManager.Instance.CurrentState = StateManager.HueAppState.ConnectedDevices_Initialized;
            GetComponent<VoiceManager>().RegisterPhrases();
            convertLightData(json);
        }
    }

    private void convertLightData(string json)
    {
        if (StateManager.Instance.ConnectedDevices_Initialized && json != null)
        {
            var lights = (Dictionary<string, object>)Json.Deserialize(json);

            foreach (string key in lights.Keys)
            {
                // init state types
                bool on;
                int id, bri, hue, sat;
                string effect, alert;

                var light = (Dictionary<string, object>)lights[key];

                var state = (Dictionary<string, object>)light["state"];

                on = Convert.ToBoolean(state["on"]);
                bri = Convert.ToInt32(state["bri"]);
                hue = Convert.ToInt32(state["hue"]);
                sat = Convert.ToInt32(state["sat"]);
                effect = Convert.ToString(state["effect"]);
                alert = Convert.ToString(state["alert"]);

                id = Convert.ToInt32(key);

                State smartLightState = new State(on, bri, hue, sat, effect, alert);
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

    IEnumerator GetUsername(string ipKey)
    {
        try
        {
            bridgeIpUsername = PlayerPrefs.GetString(ipKey);
        }
        // handle error if exists
        catch (Exception err)
        {
            Debug.Log("The following error occurred when retrieving username from device: " + err);
        }
        yield return bridgeIpUsername;
    }

    IEnumerator SetUsername(string ipKey, string name)
    {
        try
        {
            PlayerPrefs.SetString(ipKey, name);
        }
        // handle error if exists
        catch (Exception err)
        {
            Debug.Log("The following error occurred when saving username to device: " + err);
        }
        StartCoroutine(GetUsername(ipKey));
        yield return null;
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
