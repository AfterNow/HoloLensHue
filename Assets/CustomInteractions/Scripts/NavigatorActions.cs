using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorActions : MonoBehaviour
{
    // Rotation actions
    [Header("Enable Rotation")]
    public bool RotateX;
    public bool RotateY, RotateZ;

    // Slider Actions
    [Header("Enable Slider")]
    public bool Slider;
    public GameObject SliderAction;

    // Rotation actions
    [Tooltip("If action should be performed on object different from 'this', add here.")]
    [Header("Apply To")]
    public GameObject ObjectToRotate;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActionController(Vector3 scaledLocalPositionDelta)
    {
        //transform.position = worldObjectPosition;
        if (RotateX)
        {
            transform.RotateAround(gameObject.transform.position, Vector3.right, scaledLocalPositionDelta.y);
        }

        if (RotateY)
        {
            if (ObjectToRotate)
            {
                // inverted rotation to mimic user's hand movement direction
                ObjectToRotate.transform.RotateAround(ObjectToRotate.transform.position, Vector3.up, scaledLocalPositionDelta.x * -1);
            }
            else
            {
                // inverted rotation to mimic user's hand movement direction
                transform.RotateAround(gameObject.transform.position, Vector3.up, scaledLocalPositionDelta.x * -1);
            }
        }

        if (RotateZ)
        {
            transform.RotateAround(gameObject.transform.position, Vector3.forward, scaledLocalPositionDelta.z);
        }

        if (SliderAction != null)
        {
            if (Slider)
            {
                SliderAction.SendMessage("OnSlider", scaledLocalPositionDelta, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (Slider)
        {
            Debug.Log("No Slider Action script is attached");
        }
    }
}
