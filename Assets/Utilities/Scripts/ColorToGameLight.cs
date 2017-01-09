using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorToGameLight : MonoBehaviour {

    private Light lightColor;
    private GameObject gameLight;
    private string grandparentTag;

    void OnEnable()
    {
        grandparentTag = gameObject.transform.parent.transform.parent.tag;
        Debug.Log("grandytag: " + grandparentTag);
        if (grandparentTag != "Untagged")
        {
            int tagId = int.Parse(grandparentTag);
            int currentHue = SmartLightManager.lights[tagId].State.Hue;
            Debug.Log("curr: " + currentHue);
            updateGameLight(tagId, SmartLightManager.lights[tagId].State);
        }

        SmartLightManager.stateChanged += updateGameLight;
    }

    void OnDisable()
    {
        SmartLightManager.stateChanged -= updateGameLight;
    }

    void updateGameLight(int id, State state)
    {
        lightColor = GetComponent<Light>();
        Debug.Log("incoming light id?: " + id);

        // adjusted id to match array position
        int currentHue = SmartLightManager.lights[id - 1].State.Hue;

        if (currentHue >= 0 && currentHue <= 4500)
        {
            lightColor.color = Color.red;
        }
        else if (currentHue > 4500 && currentHue <= 14000)
        {
            lightColor.color = new Color(1, 0.65f, 0);
        }
        else if (currentHue > 14000 && currentHue <= 21000)
        {
            lightColor.color = Color.yellow;
        }
        else if (currentHue > 21000 && currentHue <= 35000)
        {
            lightColor.color = Color.green;
        }
        else if (currentHue > 35000 && currentHue <= 48000)
        {
            lightColor.color = Color.blue;
        }
        else if (currentHue > 48000 && currentHue <= 53500)
        {
            lightColor.color = new Color(0.3f, 0, 0.5f);
        }
        else if (currentHue > 53500)
        {
            lightColor.color = new Color(0.5f, 0, 0.5f);
        }
    }
}
