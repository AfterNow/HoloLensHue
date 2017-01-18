using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCSubPanelGenerator : MonoBehaviour {

    [Tooltip("VC SubTitle Panel gameObject")]
    public GameObject VCSubTitlePanel;

    [Tooltip("VC SubTitle Panel Text gameObject")]
    public GameObject VCSubTitleText;

    [Tooltip("Name of the voice command menu section")]
    public string VCSubTitle = "untitled";

    public VCSubModule[] subModules;

    // Use this for initialization
    void Start () {
        VCSubTitleText.GetComponent<Text>().text = VCSubTitle;

        var subModulePrefab = (GameObject)Resources.Load("Prefabs/VCSubModule");

        foreach (VCSubModule module in subModules)
        {
            var moduleObject = Instantiate(subModulePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            moduleObject.name = module.KeyPhrase;

            // gets newly instantiated GameObject and sets to child of Parent GameObject 
            GameObject currentModule = GameObject.Find(module.KeyPhrase);
            currentModule.transform.parent = gameObject.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
