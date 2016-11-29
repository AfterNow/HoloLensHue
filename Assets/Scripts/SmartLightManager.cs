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
        Debug.Log("StateMgr Start");
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
            var lightContObject = Instantiate(holoLightContPrefab, lightContainerPos, Quaternion.identity);
            
            // assigns light ID to tag for easier interating downstream.
            var lightIDOffset = light.ID - 1;
            currentLight.tag = lightIDOffset.ToString();
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

    public static void UpdateLightState(int arrayId)
    {
        SmartLight light = lights[arrayId];
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
