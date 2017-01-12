using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelBorder : MonoBehaviour {

    [Tooltip("Time message should be displayed. Void if message requires user action.")]
    public float MessageExpiration = 3f;

    private BoxCollider col;
    private Image panelImage;

    private Material gradientMaterial;

    // Use this for initialization
    void Start () {
        gradientMaterial = (Material)Resources.Load("Materials/BorderGradient");

        panelImage = GetComponent<Image>();
        col = GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        NotificationManager.newMenu += ShowPanel;

        NotificationManager.newNotification += ShowNotification;
        NotificationManager.notificationCanceled += CancelNotification;
    }

    void OnDisable()
    {
        NotificationManager.newNotification -= ShowNotification;
        NotificationManager.notificationCanceled -= CancelNotification;
    }

    private void ShowPanel(Menu menu)
    {
        if (menu.Name != "MainMenu" && menu.Name != "HideMenu")
        {
            panelImage.enabled = true;
            panelImage.material = gradientMaterial;

            col.enabled = true;
        }     
    }

    private void ShowNotification(Notification notification, Color color)
    {
        // activates PanelBorder Gameobject, removes gradient border, and applies notification color
        panelImage.enabled = true;
        panelImage.material = null;
        panelImage.color = color;

        // enables box collider for gaze targeting
        col.enabled = true;
    }

    private void CancelNotification()
    {
        panelImage.enabled = false;
    }
}
