using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class JsonNetTest : MonoBehaviour {

    public string jsonString;

	// Use this for initialization
	void Start () {
        if (jsonString != null)
        {
            //List<HueDevice> devices = JsonConvert.DeserializeObject<List<HueDevice>>(jsonString);
            //foreach(HueDevice device in devices)
            //{
            //    Debug.Log("Device id: " + device.id);
            //}

            List<HueUser> users = JsonConvert.DeserializeObject<List<HueUser>>(jsonString);
            foreach(HueUser user in users)
            {
                Debug.Log("user success: " + user.success.username);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
