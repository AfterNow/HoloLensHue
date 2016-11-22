using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;

public class SmartLightManager : Singleton<SmartLightManager> {

    private HueBridgeManager hueMgr;
    private GameObject lightPrefab;
    private GameObject holoLightContPrefab;
    public static List<SmartLight> lights = new List<SmartLight>();
    public int lightID;
    private SmartLight currentLight;

    public delegate void BrightnessChanged(int id, int bri);
    public static event BrightnessChanged brightnessChanged;

    public delegate void HueChanged(int id, int hue);
    public static event HueChanged hueChanged;

    public delegate void SaturationChanged(int id, int sat);
    public static event SaturationChanged saturationChanged;

    public delegate void StateChanged(int id, State state);
    public static event StateChanged stateChanged;

    [Tooltip("The GameObject that contains the app specific managers. By default - AppManager Prefab")]
    public GameObject appManager;
    private PhilipsHueAPI hueAPI;

    [Tooltip("The height offset above or below a SmartBulb a HoloLightContainer will be spawned")]
    public float lightContainerOffset = 1.25f;

    void Start()
    {
        hueMgr = GetComponent<HueBridgeManager>();
        hueAPI = appManager.GetComponent<PhilipsHueAPI>();
    }

    // called when bridge has been found and lights are available
    public void InitSmartLightManager(List<SmartLight> smartLights)
    {
        lights = smartLights;
        InstantiateLights();
    }

    // creates smart light game objects and sets color of prefab
    void InstantiateLights()
    {
        lightPrefab = (GameObject)Resources.Load("Prefabs/SmartBulb");
        holoLightContPrefab = (GameObject)Resources.Load("Prefabs/HoloLightContainer");

        Vector3 camPos = Camera.main.transform.position;
        // where to spawn unassigned SmartBulb GameObjects in relation to the user's current position
        Vector3 pos = new Vector3(-1, 0, 2);
        
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, camPos);

        //
        foreach (SmartLight light in lights)
        {
            var lightObject = Instantiate(lightPrefab, pos, rotation);
            lightObject.name = light.Name;
            // gets newly instantiated GameObject and sets to child of Parent GameObject 
            GameObject currentLight = GameObject.Find(light.Name);
            currentLight.transform.parent = gameObject.transform;

            Vector3 lightContainerPos = new Vector3(pos.x, lightContainerOffset * currentLight.transform.localScale.y, pos.z);
            var lightContObject = Instantiate(holoLightContPrefab, lightContainerPos, rotation);
            // assigns light ID to tag for easier interating downstream.
            var lightIDOffset = light.ID - 1;
            lightContObject.tag = lightIDOffset.ToString();
            lightContObject.transform.parent = currentLight.transform;

            // sets color of light prefab based on current light hue state
            Renderer rend = currentLight.GetComponent<Renderer>();
            Vector4 ledColor = ColorService.GetColorByHue(light.State.Hue);
            rend.material.color = ledColor;
            // TODO commented out while testing. This hides spawned prefabs.
            //if (!StateManager.Instance.Editing)
            //{
            //    rend.enabled = false;
            //}

            // increments x value to space out spawned prefabs that have no Anchor Store entry.
            pos += new Vector3(1, 0, 0);

            // TODO see if this call is needed. Real lights should already be these values
            //hueAPI.UpdateLight(light);
        }
        EventManager.TriggerEvent("SmartLightManagerReady");
        StateManager.Instance.CurrentState = StateManager.HueAppState.Ready;
    }

    public void UpdateLightState(string name, string param, int value)
    {
        Debug.Log("here is name in ULS: " + name);
        foreach (SmartLight l in lights)
        {
            Debug.Log("here is name in lights list: " + l.Name);         
            if (l.Name == name)
            {
                Debug.Log("name was a match in if statement");
                currentLight = l;
            }
        }
        if (currentLight != null)
        {
            if (param == "On")
            {
                currentLight.State.On = true;
            }
            else if (param == "Off")
            {
                currentLight.State.On = false;
            }
            else if (param == "hue")
            {
                Debug.Log("what is current light: " + currentLight);
                currentLight.State.Hue = value;
                currentLight.State.Sat = 254;
            }
            else if (param == "bri")
            {
                currentLight.State.Bri = value;
            }
            else if (param == "alert")
            {
                if (value == 0)
                {
                    currentLight.State.Alert = "none";
                }
                else
                {
                    currentLight.State.Alert = "lselect";
                }
            }
            hueAPI.UpdateLight(currentLight);
            currentLight.State.Alert = "none";
        }
        //int adjustedID = (lightID - 1);
        
    }

    // TODO if no change has been made perhaps no change should be called. Done for this function, double check and dupe for others
    public static void UpdateLightBrightness(int arrayId, int bri)
    {
        SmartLight light = lights[arrayId];
        // checks if there has been any change. If not, don't do anything
        if (light.State.Bri != bri)
        {
            light.State.Bri = bri;

            if (brightnessChanged != null)
            {
                brightnessChanged(light.ID, light.State.Bri);
            }
        }
    }

    public static void UpdateLightBrightness(int arrayId)
    {
        SmartLight light = lights[arrayId];
        if (brightnessChanged != null)
        {
            brightnessChanged(light.ID, light.State.Bri);
        }
    }

    // TODO if no change has been made perhaps no change should be called. Done for this function, double check and dupe for others
    public static void UpdateLightHue(int arrayId, int hue)
    {
        SmartLight light = lights[arrayId];
        // checks if there has been any change. If not, don't do anything
        if (light.State.Hue != hue)
        {
            light.State.Hue = hue;

            if (hueChanged != null)
            {
                hueChanged(light.ID, light.State.Hue);
            }
        }
    }

    public static void UpdateLightHue(int arrayId)
    {
        SmartLight light = lights[arrayId];
        if (hueChanged != null)
        {
            hueChanged(light.ID, light.State.Hue);
        }
    }

    // TODO if no change has been made perhaps no change should be called. Done for this function, double check and dupe for others
    public static void UpdateLightSaturation(int arrayId, int sat)
    {
        SmartLight light = lights[arrayId];
        // checks if there has been any change. If not, don't do anything
        if (light.State.Sat != sat)
        {
            light.State.Sat = sat;

            if (saturationChanged != null)
            {
                saturationChanged(light.ID, light.State.Sat);
            }
        }      
    }

    public static void UpdateLightSaturation(int arrayId)
    {
        SmartLight light = lights[arrayId];
        if (hueChanged != null)
        {
            saturationChanged(light.ID, light.State.Sat);
        }
    }

    public static void UpdateLightState(int arrayId, State state)
    {
        SmartLight light = lights[arrayId];
        light.State = state;

        if (stateChanged != null)
        {
            stateChanged(light.ID, light.State);
        }
    }

    public void SetLightsToDefault()
    {
        //for (int i = 0; i < lights.Count; i++)
        //{
        //    State currentState;
        //    currentState = lights[i].State;
        //    currentState.setHue(16000);
        //    currentState.setBri(254);
        //    currentState.setSat(150);
        //    currentState.setAlert("none");
        //    // compensate for index vs request id
        //    hueAPI.UpdateLight(lights[i + 1]);
        //}
    }
}
