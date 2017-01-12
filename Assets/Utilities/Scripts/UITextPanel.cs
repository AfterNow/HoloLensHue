using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextPanel : MonoBehaviour {

    private Image image;
    private BoxCollider col;

    void Start()
    {
        image = GetComponent<Image>();
        col = GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        NotificationManager.newNotification += ActivateImage;
        NotificationManager.notificationCanceled += DeactivateImage;
    }

    void OnDisable()
    {
        NotificationManager.newNotification -= ActivateImage;
        NotificationManager.notificationCanceled -= DeactivateImage;
    }

    private void ActivateImage(Notification notification, Color color)
    {
        image.enabled = true;
        col.enabled = true;
    }

    private void DeactivateImage()
    {
        image.enabled = false;
        col.enabled = false;
    }
}
