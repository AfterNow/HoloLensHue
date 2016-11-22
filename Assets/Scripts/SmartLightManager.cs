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
    private List<SmartLight> lights = new List<SmartLight>();
    private SmartLight currentLight;

    public delegate void BrightnessChanged(int id, int bri);
    public static event BrightnessChanged brightnessChanged;

    public delegate void HueChanged(int id, int hue);
    public static event HueChanged hueChanged;

    public delegate void SaturationChanged(int id, int sat);
    public static event SaturationChanged saturationChanged;

    public delegate void StateChanged(int id, State state);
    public static event StateChanged stateChanged;

    public int apples = 0;

    [Tooltip("The GameObject that contains the app specific managers. By default - AppManager Prefab")]
    public GameObject appManager;
    private PhilipsHueAPI hueAPI;

    void Start()
    {
        Debug.Log("was start called");
        hueMgr = GetComponent<HueBridgeManager>();
        hueAPI = appManager.GetComponent<PhilipsHueAPI>();
        //lightPrefab = (GameObject)Resources.Load("Prefabs/SmartBulb");
        Debug.Log(lightPrefab);
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

            // sets color of light prefab based on current light hue state
            Renderer rend = currentLight.GetComponent<Renderer>();
            Vector4 ledColor = ColorService.GetColorByHue(light.State.Hue);
            rend.material.color = ledColor;
            // TODO commented out while testing. This hides spawned prefabs.
            //if (!StateManager.Instance.Editing)
            //{
            //    rend.enabled = false;
            //}

            // increments x value to space out spawned prefabs
            pos += new Vector3(1, 0, 0);

            // TODO see if this call is needed. Real lights should already be these values
            //hueAPI.UpdateLight(light);
        }
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
    public void UpdateLightBrightness(int id, int bri)
    {
        SmartLight light = lights[id];
        // checks if there has been any change. If not, don't do anything
        if (light.State.bri != bri)
        {
            light.State.Bri = bri;

            if (brightnessChanged != null)
            {
                brightnessChanged(light.ID, light.State.Bri);
            }
        }
        
    }

    public void UpdateLightHue(int id, int hue)
    {
        SmartLight light = lights[id];
        light.State.Hue = hue;

        if (hueChanged != null)
        {
            hueChanged(light.ID, light.State.Hue);
        }
    }

    public void UpdateLightSaturation(int id, int sat)
    {
        SmartLight light = lights[id];
        light.State.Sat = sat;

        if (saturationChanged != null)
        {
            saturationChanged(light.ID, light.State.Sat);
        }
    }

    public void UpdateLightState(int id, State state)
    {
        SmartLight light = lights[id];
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
