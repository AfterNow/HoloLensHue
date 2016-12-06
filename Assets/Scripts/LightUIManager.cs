using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class LightUIManager : Singleton<LightUIManager> {

    private static LightUIManager lightUIManager;

    void Awake()
    {
        Debug.Log("LightUIMgr Awake");
        lightUIManager = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
