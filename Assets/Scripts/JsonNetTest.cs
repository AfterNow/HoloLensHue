using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class JsonNetTest : MonoBehaviour {

    public string jsonString;

	// Use this for initialization
	void Start () {
        if (jsonString != null)
        {
            List<HueDevice> devices = JsonConvert.DeserializeObject<List<HueDevice>>(jsonString);
            foreach(HueDevice device in devices)
            {
                Debug.Log("Device id: " + device.id);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
