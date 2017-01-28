using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class HotspotManager : Singleton<HotspotManager> {

    public delegate void HotspotsEnabled();
    public static event HotspotsEnabled hotspotsEnabled;

    public delegate void HotspotsDisabled();
    public static event HotspotsDisabled hotspotsDisabled;

    public delegate void ShowHotspots();
    public static event ShowHotspots showHotspots;

    public delegate void HideHotspots();
    public static event HideHotspots hideHotspots;

    [Tooltip("Add a Hotspot name and it will be spawed")]
    public string[] hotspots;

    private List<string> colorList;

    private Mood currentMood;

    private Material mat;

    // distance to spawn hotspot in front of user
    private float distanceFromUser;

    private float hotspotSpacing;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void changeAllColorsRequest(Mood mood)
    {
        string currentColor = mood.MainHue;
        List<SmartLight> lightList = SmartLightManager.lights;

        if (lightList.Count > 0)
        {
            // TODO hardcored for demo
            for (int i = 0; i < lightList.Count; i++)
            {
                lightList[i].State.Hue = ColorService.GetHueByColor(currentColor);

                // Todo hardcoded for demo. Will enum for V2
                if (currentColor == mood.MainHue)
                {
                    currentColor = mood.SecondaryHue;
                }
                else if (currentColor == mood.SecondaryHue)
                {
                    currentColor = mood.AccentHue;
                }
                else if (currentColor == mood.AccentHue)
                {
                    currentColor = mood.AccentHueAlt;
                }
                else {
                    currentColor = mood.MainHue;
                }
            }
            SmartLightManager.Instance.ChangeAllLights(lightList);
        }  
    }

    // TODO moods currently hardcoded
    public void HotspotTriggered(string moodName)
    {
        if (moodName == "Relax" || moodName == "Default")
        {
            currentMood = new Mood("Blue", "Purple");
        }
        else if (moodName == "Funky")
        {
            currentMood = new Mood("Pink", "Blue", "Orange", "Red");
        }
        else if (moodName == "Hot")
        {
            currentMood = new Mood("Red", "Orange", "White", "Red");
        }
        else if (moodName == "Boring")
        {
            currentMood = new Mood("Yellow");
        }
        changeAllColorsRequest(currentMood);
    }

    public void EnableHotspots()
    {
        // destroy any hotspots that may exist before creating new ones
        DisableHotspots();

        // temp generation of hotspts
        generateHotspots();

        if (hotspotsEnabled != null)
        {
            hotspotsEnabled();
        }
    }

    public void DisableHotspots()
    {
        // remove all hotspot gameObjects from the instance
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (hotspotsDisabled != null)
        {
            hotspotsDisabled();
        }
    }

    public void ShowAllHotspots()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<Renderer>().enabled = true;
        }

        if (showHotspots != null)
        {
            showHotspots();
        }
    }

    public void HideAllHotspots()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (hideHotspots != null)
        {
            hideHotspots();
        }
    }

    private void generateHotspots()
    {
        Vector3 userPos = Camera.main.transform.position;
        Vector3 lookDir = Camera.main.transform.forward;
        Quaternion camRotation = Camera.main.transform.rotation;

        distanceFromUser = 2.2f;
        hotspotSpacing = distanceFromUser;

        foreach (string name in hotspots)
        {
            Vector3 spawnPos = userPos + lookDir * distanceFromUser;

            var hotspotPrefab = (GameObject)Resources.Load("Prefabs/HotspotV1");
            var hotspotGO = Instantiate(hotspotPrefab, spawnPos, camRotation);
            hotspotGO.name = name;

            Vector4 matColor = SetHotspotColor(name);
            hotspotGO.GetComponent<Renderer>().material.color = matColor;

            GameObject currentHotspot = GameObject.Find(hotspotGO.name);
            currentHotspot.transform.parent = gameObject.transform;

            distanceFromUser += hotspotSpacing;
        }
    }

    // TODO make less redundant V2 and not hardcoded - for demo
    public Vector4 SetHotspotColor(string moodName)
    {
        Vector4 currentMat;

        if (moodName == "Relax" || moodName == "Default")
        {
            currentMat = ColorService.GetRGBAbyColor("Blue");
        }
        else if (moodName == "Funky")
        {
            currentMat = ColorService.GetRGBAbyColor("Pink");
        }
        else if (moodName == "Hot")
        {
            currentMat = ColorService.GetRGBAbyColor("Red");
        }
        else if (moodName == "Boring")
        {
            currentMat = ColorService.GetRGBAbyColor("Yellow");
        }
        else
        {
            currentMat = ColorService.GetRGBAbyColor("Red");
        }

        return currentMat;
    }
}
