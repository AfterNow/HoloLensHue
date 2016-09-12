using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PhilipsHueAPI : MonoBehaviour {

    private UnityAction testListener;
    private HueBridgeManager hueMgr;

    public GameObject bridge;

    void Awake()
    {
        testListener = new UnityAction(UpdateHueLights);
        hueMgr = gameObject.GetComponent<HueBridgeManager>();
    }

    void OnEnable()
    {
        EventManager.StartListening("test", testListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("test", testListener);
    }

    void Destroy()
    {
        EventManager.StopListening("test", testListener);
    }

    void UpdateHueLights()
    {
        Debug.Log("Test function was called");
        Debug.Log("hue me" + hueMgr);
    }
}
