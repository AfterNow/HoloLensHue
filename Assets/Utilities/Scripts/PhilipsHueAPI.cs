using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using HoloToolkit.Unity;

using MiniJSON;
using System;

public class PhilipsHueAPI : MonoBehaviour {

    private HueBridgeManager bridgeValues;
    private GameObject hologramCollection;

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
            hologramCollection.BroadcastMessage("UpdateSmartLightUI", sl);
        }
    }

    //private void PostLights(int newApplesCount)
    //{
    //    Debug.Log("lights were updated via events with params: " + newApplesCount);
    //}

    private void SendLightBrightness(int id, int bri)
    {
        Debug.Log(string.Format("light {0} has been updated with a brightness of {1}.", id, bri));
    }

    private void SendLightHue(int id, int hue)
    {
        Debug.Log(string.Format("light {0} has been updated with a hue of {1}.", id, hue));
    }

    private void SendLightSaturation(int id, int sat)
    {
        Debug.Log(string.Format("light {0} has been updated with a saturation of {1}.", id, sat));
    }

    private void SendLightState(int id, State state)
    {
        Debug.Log(string.Format("light {0} has been updated with a State of {1}.", id, state));
    }

    //private IEnumerator updateLight(State slState)
    //{
    //    string json = JsonUtility.ToJson(slState);

    //    UnityWebRequest www = UnityWebRequest.Put(request, json);
    //    yield return www.Send();

    //    if (www.isError)
    //    {
    //        Debug.LogError("There was an error with your request: " + www.error);
    //    }
    //    else
    //    {
    //        Debug.Log("response code: " + www.responseCode);
    //        Debug.Log("updating light isDone: " + www.isDone);
    //        //hologramCollection.BroadcastMessage("UpdateSmartLightUI", sl);
    //    }
    //}

    // for testing
    //tempTime += Time.deltaTime;
    //if (tempTime > requestFrequency)
    //{
    //    int brightness = (int)((percentOfMaxHeight * brightnessRange) + minBrightness);

    //    sl.State.Bri = brightness;
    //    sl.State.On = true;

    //    StartCoroutine(updateLight(sl.State));
    //    tempTime = 0;

    //}
}
