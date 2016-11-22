using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class SmartLightUI : MonoBehaviour {

    private GameObject lightUI;
    private GameObject lightUIHologram;

    private bool showLightUI = false;

    public void Start()
    {

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
        // TODO Needs to check is exists first
        lightUIHologram.SetActive(showLightUI);
    }
}
