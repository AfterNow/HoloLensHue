using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivelyMovingLookAtEffect : MonoBehaviour {

    private float angleDiff;
    private float adjustedAngle;

    Transform camTransform;

    // Use this for initialization
    void Start () {
		
	}
	
    void OnEnable()
    {
        camTransform = Camera.main.transform;
        angleDiff = Vector3.Angle(camTransform.forward, transform.forward);

        if (angleDiff < 90)
        {
            adjustedAngle = -1;
        }
        else
        {
            adjustedAngle = 1;     
        }
    }

    void OnDisable()
    {

    }

	// Update is called once per frame
	void Update () {

        camTransform = Camera.main.transform;
        Vector3 headPosition = camTransform.position * adjustedAngle;
        Vector3 lookPosition = headPosition - transform.position * adjustedAngle;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = rotation;
    }
}
