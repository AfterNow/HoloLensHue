using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class HologramController : MonoBehaviour {

    // Note that ActiveGestureAction Navigation refers to the movement of the hand. This will not refer
    // to rotation of the hologram. For instance, moving the hand from left to right on the NavigatingX axis will
    // rotate (spin) the hologram along its y axis
    public enum ActiveGestureAction
    {
        // Legacy States
        Manipulating,
        NavigatingY,
        NavigatingX,
        Fixed,
        // Hue Specific States
        NavigatingXY
    }

    ActiveGestureAction currentState = ActiveGestureAction.NavigatingXY;

    [Tooltip("Hologram object to be wrapped in control functionality")]
    public GameObject hologram;

    [Tooltip("Time in seconds to complete consecutive air taps - Triggers double-tap action")]
    public float doubleTapTolerance = 0.5f;

    [Tooltip("Determines the size buffer added to the hologram collider. This prevents the hologram's collider from breaking through thus blocking gestures.")]
    public Vector3 colliderBuffer = new Vector3(0.05f, 0.05f, 0.05f);

    private float lastTap = 0;

    private CustomManipulation cm;
    private CustomNavigation cn;

    // Use this for initialization
    void Start ()
    {
        if (hologram != null)
        {
            Debug.Log("here is hologram: " + hologram);
            // wraps a collider that will trigger gestures around the encased hologram
            Vector3 childColSize = hologram.GetComponent<Collider>().bounds.size;
            Debug.Log("child col size: " + childColSize);
            BoxCollider boxCol = gameObject.GetComponent<BoxCollider>();
            // colliderBuffer buffers the containers collider to prevent gaze from being intercepted
            boxCol.size = childColSize + colliderBuffer;

            // reference scripts for enabling/disabling
            //cm = GetComponent<CustomManipulation>();
            cn = GetComponent<CustomNavigation>();

            //cm.enabled = true;
            cn.enabled = true;
        }
        else
        {
            Debug.Log("Please attach a hologram in the inspector");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public void OnSelect()
    //{
    //    if ((Time.time - lastTap) < doubleTapTolerance)
    //    {
    //        if (currentState == ActiveGestureAction.NavigatingX)
    //        {
    //            currentState = 0;
    //        }
    //        else
    //        {
    //            currentState++;
    //        }
    //        EnableCurrentGestureAction();
    //        TriggerSoundEffect();
    //    }
    //    lastTap = Time.time;
    //}

    private void EnableCurrentGestureAction()
    {
        if (currentState == ActiveGestureAction.Manipulating)
        {
            cm.enabled = true;
            cn.enabled = false;
        }
        else if (currentState == ActiveGestureAction.NavigatingY)
        {
            //cm.enabled = false;
            cn.enabled = true;
            cn.NavigationAxis(CustomNavigation.ActiveAxis.y);
        }
        else if (currentState == ActiveGestureAction.NavigatingX)
        {
            cn.NavigationAxis(CustomNavigation.ActiveAxis.x);
        }
        else if (currentState == ActiveGestureAction.NavigatingXY)
        {
            cn.enabled = true;
            cn.NavigationAxis(CustomNavigation.ActiveAxis.xy);
        }
    }

    private void TriggerSoundEffect()
    {
        if (gameObject.GetComponent<AudioSource>())
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
