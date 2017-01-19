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

    [Tooltip("Add a keyPhrase and action to the VCSubModule will automatically generate the proper windows and placement")]
    public VCSubModule[] subModules;

    private float positionAdjustment;

    // dynamic sizing of background image. Starts with the VCTitlePanel height
    //private float panelBackingSize = 120f;

    // Use this for initialization
    void Start () {
        VCSubTitleText.GetComponent<Text>().text = VCSubTitle;

        // adjusts where the submodules should be positions. Compensates for rectTransform
        positionAdjustment = 20f * subModules.Length;

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
            rect.localPosition = new Vector3(0, positionAdjustment, 0);
            rect.localRotation = Quaternion.identity;
            rect.localScale = new Vector3(1, 1, 1);

            // sets the keyPhrase in the proper text field to be displayed
            var keyPhraseText = currentModule.transform.GetChild(0).GetChild(0).gameObject;
            keyPhraseText.GetComponent<Text>().text = module.KeyPhrase;

            // sets the action in the proper text field to be displayed
            var actionText = currentModule.transform.GetChild(1).GetChild(0).gameObject;
            actionText.GetComponent<Text>().text = module.Action;

            // gets module height to allow placement just below the previous module
            positionAdjustment -= rect.sizeDelta.y;
            // += rect.sizeDelta.y;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
