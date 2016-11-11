using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using HoloToolkit.Unity;

using MiniJSON;

public class PhilipsHueAPI : Singleton<PhilipsHueAPI> {

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
}
 