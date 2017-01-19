using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToRotate : MonoBehaviour {

    [Tooltip("How much should the GameObject rotate when air-tapped.")]
    public float RotationDegrees = 180;

    [Tooltip("How fast, in seconds, the GameObject should complete specified rotation.")]
    public float TransitionTime = 0.5f; // This will be your time in seconds.

    [Tooltip("GameObject to Rotate. By default, this is set to self.")]
    public GameObject ObjectToRotate; // This will be your time in seconds.

    private Coroutine coroutine;
    private bool rotateInProgress;

    private bool rotated;
    private float currentDegrees;
    private float nextRotation;

    // Use this for initialization
    void Start () {
        if (ObjectToRotate == null)
        {
            ObjectToRotate = this.gameObject;
        }
        currentDegrees = ObjectToRotate.transform.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {

    }
    // Called by GazeGestureManager when the user performs a tap gesture.
    public void OnSelect()
    {
        currentDegrees = ObjectToRotate.transform.eulerAngles.y;
        nextRotation = currentDegrees + RotationDegrees;

        // if transition is already active, we discard the previous transition before we start a new one
        if (rotateInProgress)
        {
            StopCoroutine(coroutine);
        }

        rotateInProgress = true;
        rotated = !rotated; 

        coroutine = StartCoroutine(LerpRotation(new Vector3(0, nextRotation, 0), TransitionTime));
    }

    IEnumerator LerpRotation(Vector3 target, float tTime)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = 0.01f / tTime; //The amount of change to apply.

        float current = transform.eulerAngles.y;

        while (progress < 1)
        {
            float angle = Mathf.LerpAngle(current, target.y, progress);
            ObjectToRotate.transform.eulerAngles = new Vector3(0, angle, 0);

            progress += increment;

            yield return null;
        }

        yield return null;
        rotateInProgress = false;
    }
}
