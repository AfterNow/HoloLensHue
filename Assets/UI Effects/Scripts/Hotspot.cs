using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotspot : MonoBehaviour {

    public string moodTitle = "Default";

    private Renderer rend;

    void OnEnable()
    {
        HotspotManager.hotspotsEnabled += activateHotspot;
        HotspotManager.hotspotsDisabled += deactivateHotspot;

        HotspotManager.showHotspots += showHotspot;
        HotspotManager.hideHotspots += hideHotspot;
    }

    void OnDisable()
    {
        HotspotManager.hotspotsEnabled -= activateHotspot;
        HotspotManager.hotspotsDisabled -= deactivateHotspot;

        HotspotManager.showHotspots -= showHotspot;
        HotspotManager.hideHotspots -= hideHotspot;
    }

    // Use this for initialization
    void Start() {
        rend = GetComponent<Renderer>();
    }

    private void activateHotspot()
    {
        gameObject.SetActive(true);
    }

    private void deactivateHotspot()
    {
        gameObject.SetActive(false);
    }

    private void showHotspot()
    {
        if (rend != null)
        {
            rend.enabled = true;
        }
    }

    private void hideHotspot()
    {
        if (rend != null)
        {
            rend.enabled = false;
        }
    }
}
