using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour {

    private UIPanel panel;
    private Image panelImage;

	void Start () {
        panel = GetComponent<UIPanel>();
        panelImage = panel.GetComponent<Image>();
	}
	
    void OnEnable()
    {
        NotificationManager.newNotification += UpdateColor;
    }

    void OnDisable()
    {
        NotificationManager.newNotification -= UpdateColor;
    }

    private void UpdateColor(Notification notification, Color color)
    {
        panelImage.color = color;
    }
}
