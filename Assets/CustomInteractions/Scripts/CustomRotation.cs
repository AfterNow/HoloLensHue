using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class CustomRotation : MonoBehaviour {

    [Tooltip("How much to scale each axis of hand movement (camera relative) when manipulating the object")]
    public Vector3 handRotationScale = new Vector3(2.0f, 2.0f, 4.0f);  // Default tuning values, expected to be modified per application

    [Tooltip("Rotation speed - the higher the number the quick the acceleration and maximun speed.")]
    public float RotationSensitivity = 5f;

    public enum ActiveAxis{ x, y, z, xy }
    ActiveAxis currentAxis = ActiveAxis.xy;

    // GameObject to represent NavigationY
    public GameObject lightSphere;

    private Vector3 manipulationPreviousPosition;

    private float rotationFactor;

    private Vector3 initialHandPosition;
    private Vector3 initialObjectPosition;
      
    private Interpolator targetInterpolator;

    private bool Navigating { get; set; }
    private bool FocusedObject { get; set; }

    private void OnEnable()
    {
        if (GestureManager.Instance != null)
        {
            GestureManager.Instance.NavigationStarted += BeginNavigation;
            GestureManager.Instance.NavigationCompleted += EndNavigation;
            GestureManager.Instance.NavigationCanceled += EndNavigation;
        }
        else
        {
            Debug.LogError(string.Format("GestureNavigator enabled on {0} could not find GestureManager instance, navigation will not function", name));
        }
    }

    private void OnDisable() 
    {
        if (GestureManager.Instance)
        {
            GestureManager.Instance.NavigationStarted -= BeginNavigation;
            GestureManager.Instance.NavigationCompleted -= EndNavigation;
            GestureManager.Instance.NavigationCanceled -= EndNavigation;
        }

        Navigating = false;
        FocusedObject = false;
    }

    private void BeginNavigation()
    {
        if (GestureManager.Instance != null && GestureManager.Instance.NavigationInProgress && !StateManager.Instance.Editing)
        {
            if (GazeManager.Instance.HitInfo.collider.name == gameObject.name)
            {
                Navigating = true;

                //targetInterpolator = gameObject.GetComponent<Interpolator>();

                // In order to ensure that any manipulated objects move with the user, we do all our math relative to the camera,
                // so when we save the initial hand position and object position we first transform them into the camera's coordinate space
                //initialHandPosition = Camera.main.transform.InverseTransformPoint(GestureManager.Instance.NavigationHandPosition);
                //initialObjectPosition = Camera.main.transform.InverseTransformPoint(transform.position);
                initialHandPosition = GestureManager.Instance.NavigationHandPosition;
                initialObjectPosition = transform.position;
            }
        }
    }

    private void EndNavigation()
    {
        Navigating = false;
    }

    void Update()
    {
        if (Navigating)
        {
            PerformRotation();
        }
    }

    public void NavigationAxis(ActiveAxis axis)
    {
        currentAxis = axis;
    }

    private void PerformRotation()
    {
        // First step is to figure out the delta between the initial hand position and the current hand position
        //Vector3 localHandPosition = Camera.main.transform.InverseTransformPoint(GestureManager.Instance.NavigationHandPosition);
        //Vector3 initialHandToCurrentHandAdjusted = (localHandPosition - initialHandPosition) * RotationSensitivity;
        Vector3 localHandPosition = GestureManager.Instance.NavigationHandPosition;
        Vector3 initialHandToCurrentHandAdjusted = (localHandPosition - initialHandPosition) * RotationSensitivity;
        Debug.Log("adjusted delta: " + initialHandToCurrentHandAdjusted);

        // When performing a manipulation gesture, the hand generally only translates a relatively small amount.
        // If we move the object only as much as the hand itself moves, users can only make small adjustments before
        // the hand is lost and the gesture completes.  To improve the usability of the gesture we scale each
        // axis of hand movement by some amount (camera relative).  This value can be changed in the editor or
        // at runtime based on the needs of individual movement scenarios.
        //Vector3 scaledLocalHandPositionDelta = Vector3.Scale(initialHandToCurrentHand, handRotationScale);

        // Once we've figured out how much the object should move relative to the camera we apply that to the initial
        // camera relative position.  This ensures that the object remains in the appropriate location relative to the camera
        // and the hand as the camera moves.  The allows users to use both gaze and gesture to move objects.  Once they
        // begin manipulating an object they can rotate their head or walk around and the object will move with them
        // as long as they maintain the gesture, while still allowing adjustment via hand movement.
        // Vector3 localObjectPosition = initialObjectPosition + scaledLocalHandPositionDelta;
        //Vector3 worldObjectPosition = Camera.main.transform.TransformPoint(localObjectPosition);

        // If the object has an interpolator we should use it, otherwise just move the transform directly
        if (targetInterpolator != null)
        {
            //targetInterpolator.SetTargetPosition(worldObjectPosition);
        }
        else
        {
            if (currentAxis == ActiveAxis.x)
            {
                // refers to rotation of hologram where ActiveAxis.x refers to hand movement axis
                transform.Rotate(Vector3.up * initialHandToCurrentHandAdjusted.y * -1);
                //transform.Rotate(Vector3.up * 5 * -1);
            }
            else if (currentAxis == ActiveAxis.y)
            {
                // refers to rotation of hologram where ActiveAxis.y refers to hand movement axis
                transform.Rotate(Vector3.right * initialHandToCurrentHandAdjusted.x * -1);
            }
            else if (currentAxis == ActiveAxis.xy)
            {
                transform.Rotate(Vector3.Scale(new Vector3(0, -1, 0), new Vector3(initialHandToCurrentHandAdjusted.y, initialHandToCurrentHandAdjusted.x, initialHandToCurrentHandAdjusted.z)));
                lightSphere.transform.position = Vector3.Scale(new Vector3(0, 1, 0), new Vector3(initialHandToCurrentHandAdjusted.y, initialHandToCurrentHandAdjusted.x, initialHandToCurrentHandAdjusted.z));
                //transform.Rotate(Vector3.Scale(new Vector3(1, 1, 0), new Vector3(0.6f, 0.2f, 0)));
            }
            else
            {
                // TODO needs to be added to watchers in GestureManager
                transform.Rotate(Vector3.forward * initialHandToCurrentHandAdjusted.z * -1);
            }

        }
    }

}
