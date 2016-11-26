using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour {

    private Text message;

    void Start()
    {
        message = GetComponent<Text>();
    }

    void OnEnable()
    {
        NotificationManager.newNotification += UpdateMessage;
    }

    void OnDisable()
    {
        NotificationManager.newNotification -= UpdateMessage;
    }

    private void UpdateMessage(Notification notification, Color color)
    {
        message.text = notification.Message;
    }
}
