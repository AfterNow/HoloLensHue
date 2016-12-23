using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class LightUIManager : Singleton<LightUIManager> {

    private static LightUIManager lightUIManager;

    public static List<LightUI> lightUIs = new List<LightUI>();

    public delegate void ColorChanged(int id, Color color);
    public static event ColorChanged colorChanged;

    public delegate void BrightnessChanged(int id, int brightness);
    public static event BrightnessChanged brightnessChanged;

    public delegate void ToggleUIChanged();
    public static event ToggleUIChanged toggleUIChanged;

    void Awake()
    {
        lightUIManager = this;
    }

    void OnStart()
    {

    }

    void OnEnable()
    {
        SmartLightManager.stateChanged += updateLightUI;
    }

    void OnDisable()
    {
        SmartLightManager.stateChanged -= updateLightUI;
    }

    // Called when the HueBridgeManager has gathered all light info and created a list of SmartLights
    public static void InitLightUI()
    {
        foreach (SmartLight sl in SmartLightManager.lights)
        {
            lightUIs.Add(new LightUI(sl.ID, false, ColorService.GetColorByHue(sl.State.Hue), sl.State.Bri, sl.Name));
        }
    }

    // updates LightUI.Show bool, adjusts all others accordingly, and 
    private void ToggleUI(int id)
    {
        int adjustedId = id + 1;
        foreach (LightUI ui in lightUIs)
        {
            // toggles selected light on/off
            if (ui.LightID == adjustedId)
            {
                ui.Show = !ui.Show;
            }
            else // all other lights will always be off
            {
                ui.Show = false;
            }      
        }

        if (toggleUIChanged != null)
        {
            toggleUIChanged();
        }

        //foreach (LightUI ui in lightUIs)
        //{
        //   Debug.Log("light: " + ui.LightID + "with a show status of: " + ui.Show);

        //}
    }

    private void updateLightUI(int id, State state)
    {
        // ajustment needed to compensate for difference in light.ID and array index
        int adjustedId = id - 1;
        LightUI currentUI = lightUIs[adjustedId];

        if (currentUI.OrbColor.Equals(ColorService.GetColorByHue(state.Hue)))
        {
            
        }
        else
        {
            currentUI.OrbColor = ColorService.GetColorByHue(state.Hue);
            if (colorChanged != null)
            {
                colorChanged(currentUI.LightID, currentUI.OrbColor);
            }
        }

        if (currentUI.Brightness == state.Bri)
        {

        }
        else
        {
            currentUI.Brightness = state.Bri;
            if (colorChanged != null)
            {
                brightnessChanged(currentUI.LightID, currentUI.Brightness);
            }
        }
    }
}
