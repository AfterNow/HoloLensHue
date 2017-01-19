using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCBoardPanelLayout : MonoBehaviour {

    [Header("Attach GameObject Panels")]
    [Space(2)]

    [Tooltip("VC Title Panel gameObject")]
    public GameObject VCTitlePanel;

    [Tooltip("VC Sub Panel gameObject")]
    public GameObject VCSubPanel;

    [Tooltip("VC Sub Panel 1 gameObject")]
    public GameObject VCSubPanelOne;

    [Tooltip("VC Sub Panel 2 gameObject")]
    public GameObject VCSubPanelTwo;

    [Space(8)]
    [Header("Set Layout Values")]
    [Space(2)]

    [Tooltip("Set height of the BoardPanel")]
    public float boardPanelHeight;

    [Tooltip("Space above header")]
    public float headerMarginTop;

    [Tooltip("Space below header")]
    public float headerMarginBottom;

    [Tooltip("Space below each SubPanel")]
    public float subPanelsMarginBottom;

    private float titlePanelHeight;
    private float titlePanelPos;

    // TODO generate dynamically. Will require converting VCSubPanels to dynamic resizing to work properly
    public float subPanelHeight;
    public float subPanelOneHeight;
    public float subPanelTwoHeight;

    private float subPanelPos;
    private float subPanelOnePos;
    private float subPanelTwoPos;

    // Use this for initialization
    void Start () {

        RectTransform boardRect = GetComponent<RectTransform>();
        boardRect.sizeDelta = new Vector2(1200, boardPanelHeight);

        if (VCTitlePanel != null)
        {
            titlePanelHeight = VCTitlePanel.GetComponent<RectTransform>().sizeDelta.y;

            // calculates board height, title panel height, and margins to find proper position
            titlePanelPos = (boardPanelHeight / 2) - (titlePanelHeight / 2) - headerMarginTop;
            VCTitlePanel.GetComponent<RectTransform>().localPosition = new Vector3(0, titlePanelPos, -0.002f);
        }
        else
        {
            Debug.Log("No Voice Command Title Panel can be found. Be sure you attached the GameObject in the Inspector.");
        }

        if (VCSubPanel != null)
        {
            // calculates previous board position and height, this sub panel height, and margins to find proper position
            subPanelPos = titlePanelPos - (titlePanelHeight / 2) - (subPanelHeight / 2) - subPanelsMarginBottom;
            VCSubPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, subPanelPos, -0.002f);
        }
        else
        {
            Debug.Log("No Voice Command Sub Panel can be found. Be sure you attached the GameObject in the Inspector.");
        }

        if (VCSubPanelOne != null)
        {
            // calculates previous board position and height, this sub panel height, and margins to find proper position
            subPanelOnePos = subPanelPos - (subPanelHeight / 2) - (subPanelOneHeight / 2) - (subPanelsMarginBottom * 2);
            VCSubPanelOne.GetComponent<RectTransform>().localPosition = new Vector3(0, subPanelOnePos, -0.002f);
        }

        if (VCSubPanelTwo != null)
        {
            // calculates previous board position and height, this sub panel height, and margins to find proper position
            subPanelTwoPos = subPanelOnePos - (subPanelOneHeight / 2) - (subPanelTwoHeight / 2);
            VCSubPanelTwo.GetComponent<RectTransform>().localPosition = new Vector3(0, subPanelTwoPos, -0.002f);

            VCSubPanelTwo.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, subPanelTwoHeight);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
