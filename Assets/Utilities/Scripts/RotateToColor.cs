using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToColor : MonoBehaviour {

    private string grandparentTag;

    private float wheelOffset = -12;
    private float toNextPanel = 51.428f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        grandparentTag = gameObject.transform.parent.transform.parent.tag;

        if (grandparentTag != "Untagged")
        {
            int tagId = int.Parse(grandparentTag);
            int currentHue = SmartLightManager.lights[tagId].State.Hue;
            Debug.Log("here is currentHue: " + currentHue);
            if (currentHue == 0)
            {
                transform.Rotate(Vector3.up, -12);
            }
            else if (currentHue == 9000)
            {
                transform.Rotate(Vector3.up, 39.428f);
            }
            else if (currentHue == 19000)
            {
                transform.Rotate(Vector3.up, 90.91f);
            }
            else if (currentHue == 23500)
            {
                transform.Rotate(Vector3.up, 142.37f);
            }
            else if (currentHue == 46950)
            {
                transform.Rotate(Vector3.up, 193.764f);
            }
            else if (currentHue == 50500)
            {
                transform.Rotate(Vector3.up, 245.192f);
            }
            else if (currentHue == 57100)
            {
                transform.Rotate(Vector3.up, 296.62f);
            }
        }
        
    }

    void OnDisable()
    {
        Debug.Log("on disable rotatetocolor");
    }
}
