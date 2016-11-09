using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using MiniJSON;
using System.Linq;

public class HueBridgeManager : MonoBehaviour {

    [Tooltip("IP address of the hue bridge: https://www.meethue.com/api/nupnp")]
    public string bridgeip = "127.0.0.1";
    public int portNumber = 8000;
    [Tooltip("Developer username")]
    public string username = "newdeveloper";

    public UnityWebRequest lights_json;
    public List<SmartLight> smartLights = null;

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
       //convertLightData();
       // MOCK end mock setup

        if ((!bridgeip.Equals("127.0.0.1")) && (!username.Equals("newdeveloper")))
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
            Debug.Log("Please enter your Bridge IP and username");
        }
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
