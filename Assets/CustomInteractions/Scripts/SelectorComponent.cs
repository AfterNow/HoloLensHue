using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorComponent : MonoBehaviour {

    GameObject colorWheel;

	// Use this for initialization
	void Start () {
        colorWheel = GameObject.Find("RainbowWheel");
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(colorWheel.transform);
        Debug.Log("selector pos" + transform.position);
	}

    void LateUpdate()
    {
        performRayCast();
    }

    void performRayCast()
    {
        var rayDirection = gameObject;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo,
            0.2f))
        {
            Debug.Log("hit info: " + hitInfo.collider.name);
        }
    }
}
