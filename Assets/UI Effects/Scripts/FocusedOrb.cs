using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedOrb : MonoBehaviour {

    public GameObject Icon;
    bool toggle = false;
	// Use this for initialization
	void Start () {
        Icon.SetActive(toggle);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("y"))  
        {
            toggle = !toggle;
            Icon.SetActive(toggle);
        }

    }

    
}
