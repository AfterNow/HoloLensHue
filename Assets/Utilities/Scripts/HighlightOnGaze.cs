using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOnGaze : MonoBehaviour {

    [Tooltip("GameObject to highlight. Defaults to GameObject this script is attached to.")]
    public GameObject ObjectToHighlight;

    private GameObject helix;

    private Shader defaultShader;
    private Shader highlightShader;

    public float transitionTime = 0.5f; // This will be your time in seconds.
    private float storedTime;

    public float smoothness = 0.02f;

    private Color defaultColor;
    public Color highlightColor = Color.yellow;

    private Coroutine coroutine;
    private bool transitionInProgress;

    private Renderer rend;

    // Use this for initialization
    void Start () {
        // saves public setting to reset upon cancelling coroutine
        storedTime = transitionTime;

        //highlightShader = Shader.Find("Custom/Highlighted");
        //foreach (Transform child in transform)
        //{
        //    if (child.name == "pHelix1")
        //    {
        //        helix = child.gameObject;
        //        rend = helix.GetComponent<Renderer>();
        //        defaultColor = rend.material.color;
        //    }
        //}

        if (ObjectToHighlight == null)
        {
            ObjectToHighlight = this.gameObject;
        }

        rend = ObjectToHighlight.GetComponent<Renderer>();
        defaultColor = rend.material.color;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGazeEnter()
    {
        if (rend != null)
        {
            // if transition is already active, we discard the previous transition before we start a new one
            if (transitionInProgress)
            {
                StopCoroutine(coroutine);
                //transitionTime = storedTime;
            }
            transitionInProgress = true;
            Color currentColor =  rend.material.color;
            coroutine = StartCoroutine(LerpColor(currentColor, highlightColor, transitionTime));
        }
    }

    void OnGazeLeave()
    {
        if (rend != null)
        {
            // if transition is already active, we discard the previous transition before we start a new one
            if (transitionInProgress)
            {
                StopCoroutine(coroutine);
               // transitionTime = storedTime;
            }
            transitionInProgress = true;
            Color currentColor = rend.material.color;
            coroutine = StartCoroutine(LerpColor(currentColor, defaultColor, transitionTime));
        }
    }

    IEnumerator LerpColor(Color current, Color colorTarget, float tTime)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / tTime; //The amount of change to apply.
        while (progress < 1)
        {
            rend.material.color = Color.Lerp(current, colorTarget, progress);
            rend.material.SetColor("_EmissionColor", rend.material.color);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
        yield return null;
        transitionInProgress = false;
    }
}
