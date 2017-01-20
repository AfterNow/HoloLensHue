using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Input;

public class ActivelyMoving : MonoBehaviour {

    [Tooltip("Attach the GameObject that contains the component you want to enable during manipulation.")]
    public GameObject TargetGameObject;

    public bool FaceWhileMoving;

    private GameObject activeGameObject;

    private ActivelyMovingLookAtEffect lookAt;

    private GestureManager gestureManager;
    private bool manipulating;

    private void Awake()
    {
        gestureManager = GestureManager.Instance;

        if (gestureManager == null)
        {
            Debug.LogError(string.Format("GestureManipulator on {0} could not find GestureManager instance, manipulation will not function", name));
        }
    }

    void Start () {
        // if value is blank in the Inspector, we'll use the GameObject this script is attached to
        if (TargetGameObject == null)
        {
            activeGameObject = this.gameObject;
        }
        else
        {
            activeGameObject = TargetGameObject;
        }
        
        // if selected in the Inspector, we will use LookAt as our action to being moved.
        if (FaceWhileMoving)
        {
            lookAt = GetComponent<ActivelyMovingLookAtEffect>();
        }
	}

    private void OnEnable()
    {
        gestureManager.OnManipulationStarted += BeginManipulation;
        gestureManager.OnManipulationCompleted += EndManipulation;
        gestureManager.OnManipulationCanceled += EndManipulation;
    }

    private void OnDisable()
    {
        gestureManager.OnManipulationStarted -= BeginManipulation;
        gestureManager.OnManipulationCompleted -= EndManipulation;
        gestureManager.OnManipulationCanceled -= EndManipulation;

        //manipulating = false;
    }

    // Update is called once per frame
    void Update () {

    }

    private void BeginManipulation(InteractionSourceKind sourceKind)
    {
        // Check if the gesture manager is not null, we're currently focused on this Game Object, and a current manipulation is in progress.
        if (gestureManager != null && gestureManager.FocusedObject != null && gestureManager.FocusedObject == gameObject && gestureManager.ManipulationInProgress)
        {
            if (FaceWhileMoving)
            {
                lookAt.enabled = true;
            }
        }
    }

    private void EndManipulation(InteractionSourceKind sourceKind)
    {
        if (FaceWhileMoving)
        {
            lookAt.enabled = false;
        }
    }
}
