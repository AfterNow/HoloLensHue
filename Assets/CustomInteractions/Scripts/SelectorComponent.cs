using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class SelectorComponent : MonoBehaviour {

    [Tooltip("Frequency of light update requests to the API. Higher the number the more frequently requests are made")]
    public float requestFrequency = 20f;
    private float tempTime = 0.0f;

    private GameObject colorWheel;
    private int layerMask = 1 << 8;

    // for testing
    SmartLight sl;
    State testState;
    string request = "http://" + "192.168.0.16" + "/api/" + "i2Voaj8bPhY53PNDxstogI2so6WL-K9OEWaE7N6s" + "/lights/2/state";
    string previousHitTag;

    // Use this for initialization
    void Start () {
        colorWheel = GameObject.Find("ColorWheel");

        // for testing
        sl = new SmartLight();
        testState = new State();
        sl.State = testState;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(colorWheel.transform);
	}

    void LateUpdate()
    {
        performRayCast();
    }

    void performRayCast()
    {
        var rayDirection = gameObject;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo,
            0.1f, layerMask))
        {
            if (previousHitTag != hitInfo.collider.tag)
            {
                previousHitTag = hitInfo.collider.tag;

                Debug.Log("hit info: " + hitInfo.collider.tag);

                int hue = ColorService.GetHueByColor(hitInfo.collider.tag);
                sl.State.Hue = hue;
                sl.State.On = true;

                //StartCoroutine(updateLight(sl.State));
                // for testing
                //tempTime += Time.deltaTime;
                //if (tempTime > requestFrequency)
                //{

                //    int hue = 0;

                //    sl.State.Hue = hue;
                //    sl.State.On = true;

                //    //StartCoroutine(updateLight(sl.State));
                //    tempTime = 0;
                //}
            }
            

        }
    }



    private IEnumerator updateLight(State slState)
    {
        //Debug.Log("Send triggered to " + request);
        string otherJson = JsonUtility.ToJson("{\"devicetype\":\"hololenshue#hololens\"}");
        Debug.Log(slState.Hue);
        string json = JsonUtility.ToJson(slState);
        
        UnityWebRequest www = UnityWebRequest.Put(request, json);
        Debug.Log("json data: " + json);
        yield return www.Send();
        Debug.Log("was i sent");
        if (www.isError)
        {
            Debug.LogError("There was an error with your request: " + www.error);
        }
        else
        {
            Debug.Log("response code: " + www.responseCode);
            Debug.Log("isDone: " + www.isDone);
            //hologramCollection.BroadcastMessage("UpdateSmartLightUI", sl);
        }
    }
}
