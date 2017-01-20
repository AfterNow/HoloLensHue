using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class VCBoardManager : Singleton<VCBoardManager> {

    public delegate void VCBoardOpened();
    public static event VCBoardOpened vcBoardOpened;

    public delegate void VCBoardClosed();
    public static event VCBoardClosed vcBoardClosed;

    public GameObject vcBoard;

    public float resetBoardDistance = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenVCBoard()
    {
        if (vcBoard != null)
        {
            vcBoard.SetActive(true);
            positionBoard();
            if (vcBoardOpened != null)
            {
                vcBoardOpened();
            }
        }
        else
        {
            Debug.Log("No Voice Command Board can be found. Be sure you attached the proper GameObject in the Inspector.");
        }
    }

    public void CloseVCBoard()
    {
        if (vcBoard != null)
        {
            vcBoard.SetActive(false);
            if (vcBoardClosed != null)
            {
                vcBoardClosed();
            }
        }
        else
        {
            Debug.Log("No Voice Command Board can be found. Be sure you attached the proper GameObject in the Inspector.");
        }
    }

    public void ResetVCBoardPosition()
    {
        if (vcBoard.activeSelf)
        {
            positionBoard();
        }
    }

    private void positionBoard()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 camPos = camTransform.position;
        Vector3 camFacing = camTransform.forward;

        vcBoard.transform.position = camPos + camFacing * resetBoardDistance;

        Vector3 lookPosition = camPos - vcBoard.transform.position;

        lookPosition.y = 0;

        var rotation = Quaternion.LookRotation(lookPosition);
        vcBoard.transform.rotation = rotation;
    }
}
