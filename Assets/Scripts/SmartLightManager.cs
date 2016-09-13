using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;

public class SmartLightManager : MonoBehaviour {

    private HueBridgeManager hueMgr;
    private GameObject lightPrefab;
    private List<SmartLight> lights = new List<SmartLight>();

    void Start()
    {
        hueMgr = GetComponent<HueBridgeManager>();
        lightPrefab = (GameObject)Resources.Load("Prefabs/SmartBulb");
    }

    public void InitSmartLightManager(List<SmartLight> sl)
    {
        lights = sl;
        InstantiateLights();
    }

    // creates smart light game objects and sets color of prefab
    void InstantiateLights()
    {
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
        }
        StateManager.Instance.CurrentState = StateManager.HueAppState.Ready;
    }
}
