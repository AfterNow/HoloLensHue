using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;

public class NotificationManager : Singleton<NotificationManager> {

    public delegate void NewNotification(Notification notification, Color color);
    public static event NewNotification newNotification;

    private static Color color;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void DisplayNotification(Notification notification)
    {
        
        if (newNotification != null)
        {
            if (notification.Type == "error")
            {
                color = Color.red;
            } else if (notification.Type == "warning")
            {
                color = Color.blue;
            }
            if (notification.SendToConsole)
            {
                Debug.Log(notification.Message);
            }
            newNotification(notification, color);
        }
    }
}
