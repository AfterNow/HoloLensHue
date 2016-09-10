using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using MiniJSON;


public class HueBridgeManager : MonoBehaviour {

    [Tooltip("IP address of the hue bridge: https://www.meethue.com/api/nupnp")]
    public string bridgeip = "127.0.0.1";
    public int portNumber = 8000;
    [Tooltip("Developer username")]
    public string username = "newdeveloper";

    public UnityWebRequest lights_json;
    public List<SmartLight> smartLights = null;

    // TODO remove mock data
    MockSmartLights mockLights;

    void Awake()
    {
        smartLights = new List<SmartLight>();
    }
    void Start()
    {
       // MOCK smart lights for testing
       mockLights = new MockSmartLights();
       smartLights = mockLights.getLights();
       convertLightData();
       // MOCK end mock setup

        if ((!bridgeip.Equals("127.0.0.1")) && (!username.Equals("newdeveloper")))
        {
            Debug.Log("co called");
            StartCoroutine(DiscoverLights(convertLightData));
        }
    }

    public IEnumerator DiscoverLights(Action nextAction)
    {
        lights_json = UnityWebRequest.Get("http://" + bridgeip + "/api/" + username + "/lights");
        Debug.Log("Hue Response Errors: " + lights_json.error);
        yield return lights_json.Send();

        nextAction();

        Debug.Log("http" + bridgeip + portNumber + "/api/" + username + "/lights");
    }

    private List<SmartLight> getAllLights()
    {
        Debug.Log("getAll called");
        return smartLights;
    }

    public List<SmartLight> GetLightCollection()
    {
        return smartLights;
    }

    void convertLightData()
    {
        var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);
        foreach (string key in lights.Keys)
        {
            // init state types
            bool on;
            int id, bri, hue, sat;
            string effect, alert;

            //Debug.Log("made it to the foreach loop "+ key);

            var light = (Dictionary<string, object>)lights[key];
            var state = (Dictionary<string, dynamic>)light["state"];

            // converting needs to be done prior to instantiating new SmartLightState
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
        SendMessage("createLights", smartLights);
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
