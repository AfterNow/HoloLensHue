using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;

public class IoTAnchor : MonoBehaviour {

    [Tooltip("Should this GameObject retain original rotation.")]
    public bool BypassRotation = true;

    private string anchorName;

    /// <summary>
    /// Manages persisted anchors.
    /// </summary>
    private WorldAnchorManager anchorManager;

    private SpatialMappingManager spatialMappingManager;

    private GestureManager gestureManager;

    private void Awake()
    {
        gestureManager = GestureManager.Instance;
    
        if (gestureManager == null)
        {
            Debug.LogError(string.Format("GestureManipulator on {0} could not find GestureManager instance, manipulation will not function", name));
        }
    }

    // Use this for initialization
    void Start () {

        anchorManager = WorldAnchorManager.Instance;
        anchorName = gameObject.name;
        if (anchorManager == null)
        {
            Debug.LogError("This script expects that you have a WorldAnchorManager component in your scene.");
        }

        spatialMappingManager = SpatialMappingManager.Instance;
        if (spatialMappingManager == null)
        {
            Debug.LogError("This script expects that you have a SpatialMappingManager component in your scene.");
        }

        if (anchorManager != null && spatialMappingManager != null)
        {
            anchorManager.AttachAnchor(this.gameObject, anchorName);

            // ensures light UI is set at the proper position and rotation relative to parent

            foreach (Transform child in transform)
            {
                if (child.name == "HoloLightContainer(Clone)")
                {
                    var defaultHeight = child.localPosition.y;

                    child.localRotation = Quaternion.Euler(new Vector3(0, child.rotation.y, child.rotation.z));
                    child.localPosition = new Vector3(0, defaultHeight, 0);
                }
            }
            //if (gameObject.transform.GetChild(0)) {
            //    var child = this.gameObject.transform.GetChild(0);
            //    Debug.Log("here is child name: " + child.name);
            //    var defaultHeight = child.localPosition.y;

            //    child.localRotation = Quaternion.Euler(new Vector3(0, child.rotation.y, child.rotation.z));
            //    child.localPosition = new Vector3(0, defaultHeight, 0);
            //}; 
        }
        else
        {
            // If we don't have what we need to proceed, we may as well remove ourselves.
            Destroy(this);
        }
    }

    public void RemoveAnchor(InteractionSourceKind sourceKind)
    {
        anchorManager.RemoveAnchor(gameObject);
    }

    public void AddAnchor(InteractionSourceKind sourceKind)
    {
        gameObject.transform.localRotation = Quaternion.identity;
        anchorManager.AttachAnchor(gameObject, anchorName);
    }
}
