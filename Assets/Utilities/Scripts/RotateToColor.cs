using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToColor : MonoBehaviour {

    public bool IsRotating;

    private float transitionTime = 0.5f; // This will be your time in seconds.

    public float smoothness = 0.02f;

    private string grandparentTag;

    private float wheelOffset = -12;
    private float toNextPanel = 51.428f;

    private int currentHue;

    private int arrayId;

    private Vector3 currentAngle;
    private Vector3 targetAngle;

    private bool bypassTransition;

    private Coroutine coroutine;
    private bool rotateInProgress;

    void OnEnable()
    {
        VoiceManager.voiceChangedColor += PerformRotation;

        // perform rotation any time this orb is selected and the UI is opened
        grandparentTag = gameObject.transform.parent.transform.parent.tag;
        if (grandparentTag != "Untagged")
        {
            int tagId = int.Parse(grandparentTag);
            Debug.Log("what is my ROttoCO id??: " + tagId);
            int currentHue = SmartLightManager.lights[tagId].State.Hue;

            Color currentColor = ColorService.GetColorByHue(currentHue);
            bypassTransition = true;
            PerformRotation(tagId + 1, currentColor);
        }     
    }

    void OnDisable()
    {
        VoiceManager.voiceChangedColor -= PerformRotation;
    }

    void Update()
    {

    }

    public void PerformRotation(int id, Color color)
    {
        grandparentTag = gameObject.transform.parent.transform.parent.tag;
        float cameraAngle = Camera.main.transform.eulerAngles.y;

        if (grandparentTag != "Untagged")
        {
            var idTag = grandparentTag;

            // Ignores objects that do not have a valid id assigned to tag
            if (int.TryParse(idTag, out arrayId))
            {
                int adjustedId = id - 1;
                if ((arrayId) == adjustedId)
                {
                    // while programmatically rotating to color let others know so we don't make drroneous calls to the API

                    currentHue = ColorService.GetHueByRGBA(color);

                    if (currentHue >= 0 && currentHue <= 4500)
                    {        
                        targetAngle = new Vector3(transform.eulerAngles.x, -12 + cameraAngle, transform.eulerAngles.z);
                    }
                    else if (currentHue > 4500 && currentHue <= 14000)
                    {
                        targetAngle = new Vector3(transform.eulerAngles.x, 39.428f + cameraAngle, transform.eulerAngles.z);
                    }
                    else if (currentHue > 14000 && currentHue <= 21000)
                    {
                        targetAngle = new Vector3(transform.eulerAngles.x, 90.91f + cameraAngle, transform.eulerAngles.z);
                    }
                    else if (currentHue > 21000 && currentHue <= 35000)
                    {
                        targetAngle = new Vector3(transform.eulerAngles.x, 142.37f + cameraAngle, transform.eulerAngles.z);
                    }
                    else if (currentHue > 35000 && currentHue <= 48000)
                    {
                        targetAngle = new Vector3(transform.eulerAngles.x, 193.764f + cameraAngle, transform.eulerAngles.z);
                    }
                    else if (currentHue > 48000 && currentHue <= 53500)
                    {
                        targetAngle = new Vector3(transform.eulerAngles.x, 245.192f + cameraAngle, transform.eulerAngles.z);
                    }
                    else if (currentHue > 53500)
                    {
                        targetAngle = new Vector3(transform.eulerAngles.x, 296.62f + cameraAngle, transform.eulerAngles.z);
                    }

                    if (bypassTransition)
                    {
                        transform.eulerAngles = new Vector3(0, targetAngle.y, 0);
                        bypassTransition = false;
                    }
                    else
                    {
                        // if transition is already active, we discard the previous transition before we start a new one
                        if (rotateInProgress)
                        {
                            StopCoroutine(coroutine);
                        }
                        rotateInProgress = true;

                        coroutine = StartCoroutine(LerpRotation(targetAngle, transitionTime));
                    }
                    
                    // rotation is complete. We can now resume sending API calls
                    IsRotating = false;
                }
            }    
        }
        Debug.Log("late state of isRot is: " + IsRotating);
    }

    IEnumerator LerpRotation(Vector3 target, float tTime)
    {
        float fromAngle = transform.eulerAngles.y;
        float toAngle = target.y;

        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = 0.02f / tTime; //The amount of change to apply.

        while (progress < 1)
        {
            float angle = Mathf.LerpAngle(fromAngle, toAngle, progress);
            transform.eulerAngles = new Vector3(0, angle, 0);

            progress += increment;

            yield return new WaitForSeconds(smoothness);
        }
        yield return null;
        rotateInProgress = false;

    }
}
