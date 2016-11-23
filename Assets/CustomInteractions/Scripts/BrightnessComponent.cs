using System;
using System.Collections;
using UnityEngine;


public class BrightnessComponent : MonoBehaviour
{
    /// <summary>
    /// Allows access to the parent's details and values needed for this component
    /// </summary>
    [Tooltip("Attach the parent HoloLightContainer.")]
    public GameObject holoLightContainer;

    // Slider actions
    [Header("Enable Sliders")]
    public bool SliderX;
    public bool SliderY, SliderZ;

    [Tooltip("A higher value increases the value per hand traveled distance.")]
    public float sliderSensitivity = 1;

    [Tooltip("Frequency of light update requests to the API. Higher the number the more frequently requests are made")]
    public float requestFrequency = 0.5f;

    [Tooltip("The minimum size the brightness gameObject BrightnessIndicator will scale.")]
    public float minSize = 0.04f;
    [Tooltip("The maximum size the brightness gameObject BrightnessIndicator will scale.")]
    public float maxSize = 0.15f;
    private float sizeRange;

    [Tooltip("The minimum height the brightness gameObject BrightnessIndicator will be relative to the center of y-axis.")]
    public float minHeight = -0.03f;
    [Tooltip("The maximum height the brightness gameObject BrightnessIndicator will be relative to the center of y-axis.")]
    public float maxHeight = 0.2f;
    private float heightRange;

    private float minBrightness = 1.0f;
    private float maxBrightness = 254f;
    private float brightnessRange;

    // assigned upon initialization of initBrightness call from SmartLightManager
    private SmartLight currentLight;
    // position of light in lights array. Needed as Hue API starts light array at '1'.
    private int arrayId;

    void OnEnable()
    {
        EventManager.StartListening("SmartLightManagerReady", initBrightness);
    }

    void OnDisable()
    {
        EventManager.StopListening("SmartLightManagerReady", initBrightness);
    }

    private void initBrightness()
    {
        var idTag = holoLightContainer.tag;
        // Ignores HoloLightContainers that do not have a valid id assigned to tag
        if (int.TryParse(idTag, out arrayId))
        {
            currentLight = SmartLightManager.lights[arrayId];
        }

        // Calculates ranges for BrightnessIndicator public vars 
        sizeRange = maxSize - minSize;
        heightRange = maxHeight - minHeight;
        brightnessRange = maxBrightness - minBrightness;
    }

    // Assign actions to slider value changes
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

            int brightness = (int)((percentOfMaxHeight * brightnessRange) + minBrightness);

            if (currentLight.State.Bri != brightness)
            {
                currentLight.State.Bri = brightness;
                SmartLightManager.UpdateLightState(arrayId);
            }
        }

        if (SliderZ)
        {

        }
    }
}

