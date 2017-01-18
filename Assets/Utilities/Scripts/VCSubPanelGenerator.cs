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
            var moduleObject = Instantiate(subModulePrefab, transform.localPosition, Quaternion.identity);
            moduleObject.name = module.KeyPhrase;

            // gets newly instantiated GameObject and sets to child of Parent GameObject 
            GameObject currentModule = GameObject.Find(moduleObject.name);
            currentModule.transform.parent = gameObject.transform;

            // sets the proper scale and placement of submodules
            RectTransform rect = currentModule.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(0, 0, 0);
            rect.localScale = new Vector3(1, 1, 1);

            // sets the keyPhrase in the proper text field to be displayed
            var keyPhraseText = currentModule.transform.GetChild(0).GetChild(0).gameObject;
            keyPhraseText.GetComponent<Text>().text = module.KeyPhrase;

            // sets the action in the proper text field to be displayed
            var actionText = currentModule.transform.GetChild(1).GetChild(0).gameObject;
            actionText.GetComponent<Text>().text = module.Action;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
