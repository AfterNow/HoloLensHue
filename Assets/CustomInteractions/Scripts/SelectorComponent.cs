using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class SelectorComponent : MonoBehaviour {

    /// <summary>
    /// Allows access to the parent's details and values needed for this component
    /// </summary>
    [Tooltip("Attach the parent HoloLightContainer.")]
    public GameObject holoLightContainer;

    /// <summary>
    /// The selector uses a raycast to determine what color the panel selected currently is. The selector should
    /// should always face toward the center of the object it is observing.
    /// </summary>
    [Tooltip("Attach the object that the selector should always face.")]
    public GameObject colorWheel;

    private int layerMask = 1 << 8;

    // assigned upon initialization of initBrightness call from SmartLightManager
    private SmartLight currentLight;
    // position of light in lights array. Needed as Hue API starts light array at '1'.
    private int arrayId;

    string previousHitTag;

    void OnEnable()
    {
        EventManager.StartListening("SmartLightManagerReady", initSelector);
    }

    void OnDisable()
    {
        EventManager.StopListening("SmartLightManagerReady", initSelector);
    }

    private void initSelector()
    {
        if (holoLightContainer.tag != "Untagged")
        {
            var idTag = holoLightContainer.tag;
            // Ignores HoloLightContainers that do not have a valid id assigned to tag
            if (int.TryParse(idTag, out arrayId))
            {
                currentLight = SmartLightManager.lights[arrayId];
            }
        }
        else
        {
            Debug.Log("No tag containing arrayId was found on this HoloLightContainer.");
        }       
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
            // Only call update if the color has changed
            if (previousHitTag != hitInfo.collider.tag)
            {
                previousHitTag = hitInfo.collider.tag;

                Debug.Log("hit info: " + hitInfo.collider.tag);

                int hue = ColorService.GetHueByColor(hitInfo.collider.tag);

                if (currentLight != null)
                {
                    currentLight.State.Hue = hue;
                    SmartLightManager.UpdateLightState(arrayId);
                }
            }
        }
    }
}
