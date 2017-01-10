using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToColor : MonoBehaviour {

    private string grandparentTag;

    private float wheelOffset = -12;
    private float toNextPanel = 51.428f;

	// Update is called once per frame
	void Update () {
	}

    void OnEnable()
    {
        grandparentTag = gameObject.transform.parent.transform.parent.tag;
        float cameraAngle = Camera.main.transform.eulerAngles.y;

        if (grandparentTag != "Untagged")
        {
            int tagId = int.Parse(grandparentTag);
            int currentHue = SmartLightManager.lights[tagId].State.Hue;

            if (currentHue >= 0 && currentHue <= 4500)
            {
                //transform.Rotate(Vector3.up, -12);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, -12 + cameraAngle, transform.eulerAngles.z);
            }
            else if (currentHue > 4500 && currentHue <= 14000)
            {
                //transform.Rotate(Vector3.up, 39.428f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 39.428f + cameraAngle, transform.eulerAngles.z);
            }
            else if (currentHue > 14000 && currentHue <= 21000)
            {
                //transform.Rotate(Vector3.up, 90.91f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90.91f + cameraAngle, transform.eulerAngles.z);
            }
            else if (currentHue > 21000 && currentHue <= 35000)
            {
                //transform.Rotate(Vector3.up, 142.37f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 142.37f + cameraAngle, transform.eulerAngles.z);
            }
            else if (currentHue > 35000 && currentHue <= 48000)
            {
                //transform.Rotate(Vector3.up, 193.764f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 193.764f + cameraAngle, transform.eulerAngles.z);
            }
            else if (currentHue > 48000 && currentHue <= 53500)
            {
                //transform.Rotate(Vector3.up, 245.192f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 245.192f + cameraAngle, transform.eulerAngles.z);
            }
            else if (currentHue > 53500)
            {
                //transform.Rotate(Vector3.up, 296.62f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 296.62f + cameraAngle, transform.eulerAngles.z);
            }
        }
    }

    void OnDisable()
    {

    }
}
