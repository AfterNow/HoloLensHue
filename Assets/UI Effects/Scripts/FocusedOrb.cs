using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedOrb : MonoBehaviour {

    public GameObject Icon;

    public float fadeInTime = 0.5f; // This will be your time in seconds.
    public float fadeOutTime = 0.5f;

    public float smoothness = 0.02f;

    private bool toggle = false;

    private Material material;
    private Color currentColor;

    private Coroutine coroutine;
    private bool fadeInProgress;

    // Use this for initialization
    void Start () {
        Icon.SetActive(true);

        material = Icon.GetComponent<Renderer>().material;
        material.color = Color.black;
	}

    void OnGazeEnter()
    {
        // if transition is already active, we discard the previous transition before we start a new one
        if (fadeInProgress)
        {
            StopCoroutine(coroutine);
        }
        fadeInProgress = true;

        coroutine = StartCoroutine(LerpColor(Color.white, fadeInTime));
    }

    void OnGazeLeave()
    {
        StartCoroutine(LerpColor(Color.black, fadeOutTime));
    }

    IEnumerator LerpColor(Color colorTarget, float transitionTime)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / transitionTime; //The amount of change to apply.
        while (progress < 1)
        {
            material.color = Color.Lerp(material.color, colorTarget, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
        yield return null;
        fadeInProgress = false;
    }
}
