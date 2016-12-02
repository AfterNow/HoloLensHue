using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class SmartLightUI : MonoBehaviour {

    private GameObject holoLightContainer;
    private GameObject lightUI;

    private bool showLightUI = false;

    public void Start()
    {
        if (gameObject.transform.GetChild(0))
        {
            holoLightContainer = gameObject.transform.GetChild(0).gameObject;
        }
        // hides HoloLightContainer visual UI immediately after instantiation
        holoLightContainer.SetActive(false);
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
        showLightUI = !showLightUI;
        holoLightContainer.SetActive(showLightUI);
        
        // TODO Needs to check is exists first
        //lightUIHologram.SetActive(showLightUI);
    }
}
