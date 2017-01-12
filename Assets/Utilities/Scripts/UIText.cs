using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour {

    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void OnEnable()
    {
        NotificationManager.newNotification += UpdateMessage;
        NotificationManager.notificationCanceled += CancelMessage;
    }

    void OnDisable()
    {
        NotificationManager.newNotification -= UpdateMessage;
        NotificationManager.notificationCanceled -= CancelMessage;
    }

    private void UpdateMessage(Notification notification, Color color)
    {
        text.enabled = true;
        text.text = notification.Message;
    }

    private void CancelMessage()
    {
        text.enabled = false;
        text.text = "";
    }
}
