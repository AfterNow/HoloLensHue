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

    public float blinkFrequency = 0.5f;
    private bool blinking;

    private int arrayId;

    private Coroutine coroutine;
    private bool blinkInProgress;

    void Awake()
    {
        lightUIManager = this;
    }

    void OnStart()
    {

    }

    void OnEnable()
    {
        HueBridgeManager.smartLightsReady += initColors;
        SmartLightManager.stateChanged += updateLightUI;
    }

    void OnDisable()
    {
        HueBridgeManager.smartLightsReady -= initColors;
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

    public void initColors(List<SmartLight> sls)
    {
        Debug.Log("was initColors called???");
        foreach (SmartLight sl in sls)
        {
            lightUIs.Add(new LightUI(sl.ID, false, ColorService.GetColorByHue(sl.State.Hue), sl.State.Bri, sl.Name));
            updateLightUI(sl.ID, sl.State);
        }
    }

    // updates LightUI.Show bool, adjusts all others accordingly, and 
    private void ToggleUI(int id)
    {
        // ajustment needed to compensate for difference in light.ID and array index
        int adjustedId = id + 1;
        
        if (!StateManager.Instance.SetupMode)
        {
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
        Debug.Log("UALUI claled");
        // ajustment needed to compensate for difference in light.ID and array index
        int adjustedId = id - 1;
        LightUI currentUI = lightUIs[adjustedId];

        if (currentUI.OrbColor.Equals(ColorService.GetColorByHue(state.Hue)))
        {
            Debug.Log("do i equalsss???");
            Debug.Log("cur orb color is: " + currentUI.OrbColor);
        }
        else
        {
            Debug.Log("make it to this else???");
            currentUI.OrbColor = ColorService.GetColorByHue(state.Hue);
            Debug.Log("cur orb color is: " + currentUI.OrbColor);
            if (colorChanged != null)
            {
                Debug.Log("what is the color here???: " + currentUI.OrbColor);
                colorChanged(currentUI.LightID, currentUI.OrbColor);
            }
            UpdateOrbColor(id, state);

        }

        if (currentUI.Brightness == state.Bri)
        {

        }
        else
        {
            currentUI.Brightness = state.Bri;
            if (brightnessChanged != null)
            {
                brightnessChanged(currentUI.LightID, currentUI.Brightness);
            }
        }

        if (state.Alert == "lselect")
        {
            MakeOrbBlink(id);
        }
    }

    // sets color of light prefab based on current light hue state
    public void UpdateOrbColor(int id, State state)
    {
        foreach (Transform child in transform)
        {
            if (child.tag != "Untagged")
            {
                var idTag = child.tag;

                // Ignores objects that do not have a valid id assigned to tag
                if (int.TryParse(idTag, out arrayId))
                {
                    
                    // adjusted arrayId to compensate for Hue starting index at 1
                    if ((arrayId + 1) == id)
                    {
                        Renderer rend = child.GetComponent<Renderer>();
                        Vector4 ledColor = ColorService.GetColorByHue(state.Hue);
                        rend.material.color = ledColor;
                    }
                }
            }
        }
    }

    public void MakeOrbBlink(int id)
    {
        foreach (Transform child in transform)
        {
            if (child.tag != "Untagged")
            {
                var idTag = child.tag;

                // Ignores objects that do not have a valid id assigned to tag
                if (int.TryParse(idTag, out arrayId))
                {

                    // adjusted arrayId to compensate for Hue starting index at 1
                    if ((arrayId + 1) == id)
                    {
                        Renderer rend = child.GetComponent<Renderer>();
                        
                        // if transition is already active, we discard the previous transition before we start a new one
                        if (blinkInProgress)
                        {
                            StopCoroutine(coroutine);
                        }
                        blinkInProgress = true;

                        coroutine = StartCoroutine(Blink(rend));
                    }
                }
            }
        }
    }

    private IEnumerator Blink(Renderer rend)
    {
        // value of 20 is equal to 10 seconds - matches the time the Hue hardware blinks
        int blinkTime = 20;

        while (blinkTime > 0)
        {
            rend.enabled = blinking;
            yield return new WaitForSeconds(blinkFrequency);
            blinking = !blinking;
            blinkTime--;
        }
    }
}
