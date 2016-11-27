using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;

public class NotificationManager : Singleton<NotificationManager> {

    public delegate void NewNotification(Notification notification, Color color);
    public static event NewNotification newNotification;

    // Time notification should be displayed. Void if notification requires user action
    private static float TimeTillExpiration = 5f;

    private static Canvas canvas;
    private static Color color;

    private static NotificationManager notificationManager;

    private static Coroutine coroutine;
    private static bool notificationActive;

    void Awake()
    {
        notificationManager = this;
    }

    void Start () {
        if (transform.GetChild(0).name == "Canvas")
        {
            canvas = transform.GetChild(0).gameObject.GetComponent<Canvas>();
            canvas.enabled = false;
        }
        else
        {
            Debug.Log("No child Canvas was found. Please add one to use notification system.");
        }
	}
	
    public static void DisplayNotification(Notification notification)
    {
        if (newNotification != null)
        {
            canvas.enabled = true;

            if (notification.Type == "error")
            {
                color = Color.red;
            } else if (notification.Type == "alert")
            {
                color = new Color(0.27f, 0.5f, 0.7f);
            }

            if (notification.SendToConsole)
            {
                Debug.Log(notification.Message);
            }

            newNotification(notification, color);

            if (!notification.RequiresAction)
            {
                // if notification is active, we discard the previous expiration timer before we start a new one
                if (notificationActive)
                {
                    notificationManager.StopCoroutine(coroutine);
                }
                notificationActive = true;
                coroutine = notificationManager.StartCoroutine(NotificationExpiration(TimeTillExpiration));
            }
        }
    }

    public static void CancelNotification()
    {
        canvas.enabled = false;
        notificationActive = false;
    }

    private static IEnumerator NotificationExpiration(float seconds)
    {    
        yield return new WaitForSeconds(seconds);
        CancelNotification();
    }
}
