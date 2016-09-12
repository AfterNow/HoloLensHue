using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;

public class SmartLightManager : MonoBehaviour {

    List<SmartLight> lights = new List<SmartLight>();

    private HueBridgeManager hueMgr;

    void Start()
    {
        hueMgr = GetComponent<HueBridgeManager>();
    }

    public void InitSmartLightManager(List<SmartLight> sl)
    {
        lights = sl;
        StateManager.Instance.CurrentState = StateManager.HueAppState.Ready;
    }
}
