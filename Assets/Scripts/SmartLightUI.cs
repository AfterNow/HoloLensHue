using UnityEngine;
using System.Collections;

public class SmartLightUI : MonoBehaviour {

	void UpdateSmartLightUI(SmartLight sl)
    {
        if (sl.Name == gameObject.name)
        {
            Renderer rend = gameObject.GetComponent<Renderer>();
            Debug.Log(sl.State.Hue);
            Vector4 ledColor = ColorService.GetColorByHue(sl.State.Hue);
            Debug.Log(ledColor);
            rend.material.color = ledColor;
        }
    }
}
