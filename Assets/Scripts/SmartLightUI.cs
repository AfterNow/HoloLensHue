using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class SmartLightUI : MonoBehaviour {

    private GameObject holoLightContainer;
    private GameObject lightUI;

    private bool showLightUI = false;

    private GestureManager gestureManager;

    public void Start()
    {
        gestureManager = GestureManager.Instance;

        foreach (Transform child in transform)
        {
            if (child.name == "HoloLightContainer(Clone)")
            {
                holoLightContainer = child.gameObject;
            }
        }
        //if (gameObject.transform.GetChild(0))
        //{
        //    holoLightContainer = gameObject.transform.GetChild(0).gameObject;
        //}
        // hides HoloLightContainer visual UI immediately after instantiation
        
        if (holoLightContainer)
        {
            holoLightContainer.SetActive(false);
        }
    }

    void UpdateSmartLightUI(SmartLight sl)
    {
        if (sl.Name == gameObject.name)
        {
            Renderer rend = gameObject.GetComponent<Renderer>();
            //Debug.Log(sl.State.Hue);
            Vector4 ledColor = ColorService.GetColorByHue(sl.State.Hue);
            //Debug.Log(ledColor);
            rend.material.color = ledColor;
        }
    }

    public void OnSelect()
    {
        Debug.Log("i was selected");
        //foreach (Transform child in transform)
        //{
        //    if (gameObject.name != gestureManager.FocusedObject.name)
        //    {
        //        Debug.Log("WE are not the selected one: " + child.name);
        //        holoLightContainer.SetActive(false);
        //    }
        //}
        Debug.Log("here is the focused object onSelect: " + gestureManager.FocusedObject);
        showLightUI = !showLightUI;
        holoLightContainer.SetActive(showLightUI);

        
        // TODO Needs to check is exists first
        //lightUIHologram.SetActive(showLightUI);
    }
}
