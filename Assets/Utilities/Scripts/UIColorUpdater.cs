using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIColorUpdater : MonoBehaviour {

    public float transitionTime = 0.5f; // This will be your time in seconds.

    public float smoothness = 0.02f;

    private Color currentColor;

    private Coroutine coroutine;
    private bool transitionInProgress;

    private Renderer rend;

    // position of light in lights array. Needed as Hue API starts light array at '1'.
    private int arrayId;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();

        if (currentColor != null)
        {
            updateMaterial(arrayId, currentColor);
        }
	}

    void OnEnable()
    {
        LightUIManager.colorChanged += updateMaterial;
    }

    void OnDisable()
    {
        LightUIManager.colorChanged -= updateMaterial;
    }

    void updateMaterial(int id, Color color)
    {
        if (rend != null)
        {
            currentColor = rend.material.color;

            // if transition is already active, we discard the previous transition before we start a new one
            if (transitionInProgress)
            {
                StopCoroutine(coroutine);
            }
            transitionInProgress = true;

            coroutine = StartCoroutine(LerpColor(color, transitionTime));
        }
        else
        {
            if (transform.parent.tag != "Untagged")
            {
                var idTag = transform.parent.tag;

                // Ignores objects that do not have a valid id assigned to tag
                if (int.TryParse(idTag, out arrayId))
                {
                    // adjusted arrayId to compensate for Hue starting index at 1
                    if ((arrayId + 1) == id)
                    {
                        var currentHue = SmartLightManager.lights[arrayId].State.Hue;
                        currentColor = ColorService.GetColorByHue(currentHue);
                    }                   
                }
            }
            else
            {
                Debug.Log("No tag containing arrayId was found on parent.");
            }

        }   
    }

    IEnumerator LerpColor(Color colorTarget, float tTime)
    {
        Debug.Log("Lerp that color was callede!!!!!!!");
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / tTime; //The amount of change to apply.
        while (progress < 1)
        {
            rend.material.color = Color.Lerp(currentColor, colorTarget, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
        yield return null;
        transitionInProgress = false;
    }
}
