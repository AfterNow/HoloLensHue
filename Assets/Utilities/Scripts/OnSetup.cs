using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class OnSetup : MonoBehaviour {

    private int arrayId;

    private float middleLight;
    private bool arrowEnabled;
    private bool currentArrowEnabled;

    // Use this for initialization
    void Start () {
        if (StateManager.Instance.SetupMode)
        {
            enableDIScript();
        }
    }

    void OnEnable()
    {
        StateManager.onSetup += enableDIScript;
        StateManager.onConfiguration += enableDIScript;
        StateManager.onReady += disableDIScript;

        if (StateManager.Instance.SetupMode)
        {
            enableDIScript();
        }
    }

    void OnDisable()
    {
        StateManager.onSetup -= enableDIScript;
        StateManager.onConfiguration -= enableDIScript;
        StateManager.onReady -= disableDIScript;
    }
    // Update is called once per frame
    void Update () {
		
	}

    void enableDIScript()
    {
        middleLight = SmartLightManager.lights.Count / 2;

        if (IsDIEnabled())
        {
            GetComponent<HoloToolkit.CustomDirectionIndicator>().enabled = true;
        }
        
    }

    void disableDIScript()
    {
        GetComponent<HoloToolkit.CustomDirectionIndicator>().enabled = false;
    }

    private bool IsDIEnabled()
    {
        if (transform.parent.tag != "Untagged")
        {
            var idTag = transform.parent.tag;
            // Ignores objects that do not have a valid id assigned to tag
            if (int.TryParse(idTag, out arrayId))
            {
                // adjusted arrayId to compensate for Hue starting index at 1
                currentArrowEnabled = ((arrayId + 1) == middleLight);
            }
        }
        return currentArrowEnabled;
    }
}
