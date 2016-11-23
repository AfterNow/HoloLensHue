using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using HoloToolkit.Unity;

using MiniJSON;
using System;

public class PhilipsHueAPI : MonoBehaviour {

    private HueBridgeManager bridgeValues;
    private GameObject hologramCollection;

    [Tooltip("Rate limit of network requests. Value indicates time between requests in seconds")]
    public float timeBetweenRequests = 0.3f;
    private float requestCounter = 0f;

    void Start()
    {
        bridgeValues = GetComponent<HueBridgeManager>();
        if (GameObject.Find("HologramCollection") != null)
        {
            hologramCollection = GameObject.Find("HologramCollection");      
        }
        else
        {
            Debug.LogError("No GameObject name HologramCollection can be found. This object should contain all holograms and the SmartLightManager");
        }
    }

    void OnEnable()
    {
        SmartLightManager.brightnessChanged += SendLightBrightness;
        SmartLightManager.hueChanged += SendLightHue;
        SmartLightManager.saturationChanged += SendLightSaturation;
        SmartLightManager.stateChanged += SendLightState;
    }

    void OnDisable()
    {
        SmartLightManager.brightnessChanged -= SendLightBrightness;
        SmartLightManager.hueChanged -= SendLightHue;
        SmartLightManager.saturationChanged -= SendLightSaturation;
        SmartLightManager.stateChanged -= SendLightState;
    }

    public void UpdateLight(SmartLight sl)
    {
        StartCoroutine(updateLight(sl));
    }
    private IEnumerator updateLight(SmartLight sl)
    {
        string request = "http://" + bridgeValues.bridgeip + "/api/" + bridgeValues.username + "/lights/" + sl.ID.ToString() + "/state";
        //Debug.Log("Send triggered to " + request);

        string json = JsonUtility.ToJson(sl.State);

        UnityWebRequest www = UnityWebRequest.Put(request, json);
        yield return www.Send();
        if (www.isError)
        {
            Debug.LogError("There was an error with your request: " + www.error);
        }
        else
        {
            // TODO - Perhaps this should be changed to be an event in the event mananger's listing
            hologramCollection.BroadcastMessage("UpdateSmartLightUI", sl);
        }
    }

    private void SendLightBrightness(int id, int bri)
    {
        Debug.Log(string.Format("light {0} has been updated with a bri of {1}.", id, bri));
    }

    private void SendLightHue(int id, int hue)
    {
        Debug.Log(string.Format("light {0} has been updated with a hue of {1}.", id, hue));
    }

    private void SendLightSaturation(int id, int sat)
    {
        Debug.Log(string.Format("light {0} has been updated with a saturation of {1}.", id, sat));
    }

    // subscribed to any change in a light's state. The id refers to the Hue Light's id value, not array id value.
    private void SendLightState(int id, State state)
    {
        // variable limiter for network requests to the Hue bridge
        requestCounter += Time.deltaTime;
        if (requestCounter > timeBetweenRequests)
        {
            StartCoroutine(sendLightPutRequest(id, state));
            requestCounter = 0;
        }
    }

    private IEnumerator sendLightPutRequest(int id, State state)
    {
        string request = "http://" + bridgeValues.bridgeip + "/api/" + bridgeValues.username + "/lights/" + id.ToString() + "/state";
        string json = JsonUtility.ToJson(state);

        UnityWebRequest www = UnityWebRequest.Put(request, json);
        yield return www.Send();

        if (www.isError)
        {
            Debug.LogError("There was an error with your request: " + www.error);
        }
        else
        {
            Debug.Log("response code: " + www.responseCode);
            Debug.Log("updating light isDone: " + www.isDone);
            //hologramCollection.BroadcastMessage("UpdateSmartLightUI", sl);
        }
    }
}
