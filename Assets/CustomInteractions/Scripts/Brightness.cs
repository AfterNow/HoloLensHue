using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using MiniJSON;

public class Brightness : MonoBehaviour {

    // Slider actions
    [Header("Enable Sliders")]
    public bool SliderX;
    public bool SliderY, SliderZ;

    [Tooltip("A higher value increases the value per hand traveled distance.")]
    public float sliderSensitivity = 1;

    [Tooltip("Frequency of light update requests to the API. Higher the number the more frequently requests are made")]
    public float requestFrequency = 20f;
    private float tempTime = 0.0f;

    private float minSize = 0.04f;
    private float maxSize = 0.15f;
    private float sizeRange;

    private float minHeight = -0.03f;
    private float maxHeight = 0.2f;
    private float heightRange;

    private float minBrightness = 1.0f;
    private float maxBrightness = 254f;
    private float brightnessRange;

    // TODO remove - for testing only
    SmartLight sl;
    State testState;
    string request = "http://" + "10.0.3.19" + "/api/" + "i2Voaj8bPhY53PNDxstogI2so6WL-K9OEWaE7N6s" + "/lights/4/state";

    // Use this for initialization
    void Start () {
        sizeRange = maxSize - minSize;
        heightRange = maxHeight - minHeight;
        brightnessRange = maxBrightness - minBrightness;

        // TODO remove - for testing only
        sl = new SmartLight();
        testState = new State();
        sl.State = testState;
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void OnSlider(Vector3 scaledLocalPositionDelta)
    {
        if (SliderX)
        {

        }

        if (SliderY)
        {
            float adjustedDelta = Mathf.Clamp((scaledLocalPositionDelta.y * sliderSensitivity), minHeight, maxHeight);
            float percentOfMaxHeight = (adjustedDelta - minHeight) / (heightRange);

            float scaleSize = (percentOfMaxHeight * sizeRange) + minSize;

            gameObject.transform.position = new Vector3(gameObject.transform.position.x, adjustedDelta, gameObject.transform.position.z);
            gameObject.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);

            // for testing
            tempTime += Time.deltaTime;
            if (tempTime > requestFrequency)
            {
                int brightness = (int)((percentOfMaxHeight * brightnessRange) + minBrightness);

                sl.State.Bri = brightness;
                sl.State.On = true;

                StartCoroutine(updateLight(sl.State));
                tempTime = 0;

                EventManager.TriggerEventWithParams("OnBrightnessChange", sl);
            }
        }

        if (SliderZ)
        {

        }
    }

    private IEnumerator updateLight(State slState)
    {
        string json = JsonUtility.ToJson(slState);

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
