using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using MiniJSON;
using Newtonsoft.Json;
using System.Text;

public class HueBridgeManager : MonoBehaviour {

    [Tooltip("IP address of the hue bridge: https://www.meethue.com/api/nupnp")]
    public string bridgeip = "127.0.0.1";
    public int portNumber = 8000;
    [Tooltip("Developer username")]
    public string username = "newdeveloper";

    public List<HueDevice> devices;

    private List<SmartLight> smartLights;

    private string parsedBridgeip;
    private bool bridgeFound;

    private string bridgeIpUsername;

    private GameObject hologramCollection;
    private SmartLightManager slm;

    private bool awaitingBridgeLink;

    // TODO remove mock data
    MockSmartLights mockLights;

    void Awake()
    {
        Debug.Log("HueBridgeMgr Awake");
        smartLights = new List<SmartLight>();
        if (GameObject.Find("HologramCollection") != null)
        {
            hologramCollection = GameObject.Find("HologramCollection");
            slm = hologramCollection.GetComponent<SmartLightManager>();
        }
        else
        {
            Debug.LogError("No GameObject name HologramCollection can be found. This object should contain all holograms and the SmartLightManager");
        }   
    }

    void OnEnable()
    {
        Debug.Log("HueBridgeMgr OnEnable");
        NotificationManager.notificationCanceled += NotificationExpired;
    }

    void OnDisable()
    {
        NotificationManager.notificationCanceled -= NotificationExpired;
    }

    void Start()
    {
        Debug.Log("HueBridgeMgr Start");
    }

    public void InitHueBridgeManager()
    {
        // MOCK smart lights for testing
        //mockLights = new MockSmartLights();
        //smartLights = mockLights.getLights();
        //slm.InitSmartLightManager(smartLights);
        //convertLightData();
        // MOCK end mock setup

        if ((bridgeip != "127.0.0.1" && bridgeip != "") && (username != "newdeveloper" && username != ""))
        {
            if (StateManager.Instance.Starting)
            {
                bridgeReady();
            }
            else
            {
                Notification notification = new Notification("error", "There was an error with the app startup state.");
                NotificationManager.DisplayNotification(notification);
            }
        }
        else
        {
            StartCoroutine(CheckOrGetBridgeIP());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            SmartLightManager.lights[0].State.Bri = 200;
            SmartLightManager.UpdateLightState(0);

            Notification notification = new Notification("error", "There was an error with the app startup state.");
            NotificationManager.DisplayNotification(notification);
        }
        if (Input.GetKeyDown("w"))
        {
            //SmartLightManager.lights[0].State.Bri = 155;
            //SmartLightManager.UpdateLightState(0);

            //Notification notification = new Notification("alert", "Please press the link button on your Bridge and try again.");
            //NotificationManager.DisplayNotification(notification);

            StateManager.Instance.CurrentState = StateManager.HueAppState.Configuring;
        }
    }

    public IEnumerator CheckOrGetBridgeIP()
    {
        string url = "https://www.meethue.com/api/nupnp";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.Send();

        if (request.isError)
        {
            Notification notification = new Notification("error", "There was an error attempting to discover bridge ip.");
            NotificationManager.DisplayNotification(notification);
            yield break;
        }
        Debug.Log(request.downloadHandler.text);

        if (request.downloadHandler.text != "[]")
        {
            bridgeFound = true;
        }

        if (bridgeFound)
        {
            string bridgeJsonValue = request.downloadHandler.text;

            if (bridgeJsonValue != null)
            {
                devices = JsonConvert.DeserializeObject<List<HueDevice>>(bridgeJsonValue);

                // TODO - only sending the ip of the first bridge found. Will need to handle multiple briges found cases
                string firstBridgeIP = devices[0].internalipaddress;
                bridgeip = firstBridgeIP;
                StartCoroutine(CheckOrCreateBridgeUser(bridgeip));
            }
            else
            {
                Debug.Log("no JSON data was returned from the bridge");
            }
        }
        else
        {
            Notification notification = new Notification("error", "No bridge was discovered on the current network. Refer to https://www.meethue.com/api/nupnp");
            NotificationManager.DisplayNotification(notification);
        }
    }

    public IEnumerator CheckOrCreateBridgeUser(string ip)
    {
        // checks that a username has not been manually added to the inspector. If one has, we want to bypass creating a new one
        if (username == null || username == "newdeveloper" || username == "")
        {
            // check if this Hue Bridge already has a valid username
            StartCoroutine(GetUsername("cats"));
            // if a valid username is saved to the device, set as current user
            if (bridgeIpUsername != null && bridgeIpUsername != "newdeveloper" && bridgeIpUsername != "")
            {
                username = bridgeIpUsername;
                Debug.Log("An existing username has been retrieved from the device");
            }
            // if no username was associated with the Hue Bridge, create and store one
            else
            {
                string url = "http://" + ip + "/api";
                UnityWebRequest request = new UnityWebRequest(url, "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"devicetype\": \"hololenshue#hololens\"}");
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.Send();

                if (request.isError)
                {
                    Notification notification = new Notification("error", "The request timed out. Please check your Bridge IP and internet connection.");
                    NotificationManager.DisplayNotification(notification);
                    yield break;
                }

                if (request.downloadHandler.text.Contains("\"error\":{\"type\":101"))
                {
                    MenuStateManager.Instance.CurrentState = MenuStateManager.MenuState.TT_Hotspot;
                    
                    // while true, a request to the bridge will be sent during specified intervals
                    awaitingBridgeLink = true;

                    StartCoroutine(AwaitingBridgeButtonPress(ip));
                    yield break;
                }
                storeHueUser(ip, request.downloadHandler.text);
            }          
        }
        // Bridgeid and username are both found and valid. Ready to make lighting requests
        bridgeReady();
    }

    private void storeHueUser(string ip, string json)
    {
        List<HueUser> users = JsonConvert.DeserializeObject<List<HueUser>>(json);
        if (users[0].success != null)
        {
            username = users[0].success.username;
        }
        StartCoroutine(SetUsername(ip, username));
        Debug.Log("A new username has been created: " + bridgeIpUsername);
    }

    private void bridgeReady()
    {
        StateManager.Instance.CurrentState = StateManager.HueAppState.ConnectedDevices_Initializing;
        StartCoroutine(DiscoverLights());
    }

    public IEnumerator DiscoverLights()
    {
        string url = "http://" + bridgeip + "/api/" + username + "/lights";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.Send();

        if (request.isError)
        {
            Notification notification = new Notification("error", "The request timed out. Please check your Bridge IP and internet connection.");
            NotificationManager.DisplayNotification(notification);
            yield break;
        }

        string json = request.downloadHandler.text;
        if (json.Contains("error"))
        {
            if (json.Contains("unauthorized"))
            {
                Notification notification = new Notification("alert", "Unauthorized user. Please add valid Username to Hue Bridge Manager.");
                NotificationManager.DisplayNotification(notification);
            }
            else
            {
                Notification notification = new Notification("error", "A bridge could not be found or could not be accessed at this time.");
                NotificationManager.DisplayNotification(notification);
                StateManager.Instance.CurrentState = StateManager.HueAppState.ConnectedDevices_Failed;
            }
        }
        else
        {
            StateManager.Instance.CurrentState = StateManager.HueAppState.ConnectedDevices_Initialized;

            Notification notification = new Notification("alert", "Lights have been discovered! To configure setup, say \"Configure Room.\" Otherwise, enjoy!");
            notification.SendToConsole = false;
            NotificationManager.DisplayNotification(notification);
            //GetComponent<VoiceManager>().RegisterPhrases();
            convertLightData(json);
        }
    }

    private void convertLightData(string json)
    {
        if (StateManager.Instance.ConnectedDevices_Initialized && json != null)
        {
            var lights = (Dictionary<string, object>)Json.Deserialize(json);

            foreach (string key in lights.Keys)
            {
                // init state types
                bool on;
                int id, bri, hue, sat;
                string effect, alert;

                var light = (Dictionary<string, object>)lights[key];

                var state = (Dictionary<string, object>)light["state"];

                on = Convert.ToBoolean(state["on"]);
                bri = Convert.ToInt32(state["bri"]);
                hue = Convert.ToInt32(state["hue"]);

                // TODO - remove when ability to adjust saturation is added in the future
                sat = 254;
                //sat = Convert.ToInt32(state["sat"]);

                effect = Convert.ToString(state["effect"]);
                alert = Convert.ToString(state["alert"]);

                id = Convert.ToInt32(key);

                State smartLightState = new State(on, bri, hue, sat, effect, alert);
                smartLights.Add(new SmartLight(id, light["name"].ToString(), light["modelid"].ToString(), smartLightState));
            }
            // sends collection of lights to the SmartLightManager to handle all future changes
            if (slm != null)
            {
                // TODO - should be able to change this to the instance instead of direct reference
                slm.InitSmartLightManager(smartLights);
                LightUIManager.InitLightUI();
            }
            else
            {
                Debug.LogError("No SmartLightManager was found. Please check inspector and be sure the correct GameObject is set on the HueBridgeManager");
            }
        }
        else
        {
            Notification notification = new Notification("alert", "No lights can be found. Please ensure the Bridge IP is set and your lighting system is functioning properly.");
            NotificationManager.DisplayNotification(notification);
        }
    }

    IEnumerator AwaitingBridgeButtonPress(string ip)
    {
        if (awaitingBridgeLink)
        {
            string url = "http://" + ip + "/api";
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"devicetype\": \"hololenshue#hololens\"}");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.Send();

            if (request.isError)
            {
                Notification notification = new Notification("error", "The request timed out. Please check your Bridge IP and internet connection.");
                NotificationManager.DisplayNotification(notification);
                yield break;
            }

            if (request.downloadHandler.text.Contains("\"error\":{\"type\":101"))
            {
                StartCoroutine(AwaitingButtonPressInterval(ip));
                yield break;
            }

            NotificationManager.CancelNotification();
            storeHueUser(ip, request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Bridge link button was not pressed within the time specified by counter");
        }
    }

    IEnumerator AwaitingButtonPressInterval(string ip)
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(AwaitingBridgeButtonPress(ip));
    }

    IEnumerator GetUsername(string ipKey)
    {
        try
        {
            bridgeIpUsername = PlayerPrefs.GetString(ipKey);
        }
        // handle error if exists
        catch (Exception err)
        {
            Debug.Log("The following error occurred when retrieving username from device: " + err);
        }
        yield return bridgeIpUsername;
    }

    IEnumerator SetUsername(string ipKey, string name)
    {
        try
        {
            PlayerPrefs.SetString(ipKey, name);
        }
        // handle error if exists
        catch (Exception err)
        {
            Debug.Log("The following error occurred when saving username to device: " + err);
        }
        StartCoroutine(GetUsername(ipKey));
        bridgeReady();
        yield return null;
    }

    private void NotificationExpired()
    {
        awaitingBridgeLink = false;
    }

    public void RecheckOrGetBridgeIP()
    {
        if (StateManager.Instance.Starting && !awaitingBridgeLink)
        {
            Debug.Log("rechdck bridge cond met");
            StartCoroutine(CheckOrGetBridgeIP());
        }
    }

    public void RecheckOrCreateBridgeUser()
    {
        if (StateManager.Instance.Starting && !awaitingBridgeLink)
        {
            Debug.Log("rechdck username cond met");
            StartCoroutine(CheckOrCreateBridgeUser(bridgeip));
        }
    }

    public void TestPut()
    {
        //State testState = smartLights[0].getState();
        //testState.isOn(false);
        //string request = "http://" + bridgeip + "/api/" + username + "/lights/1/state";
        //string json = JsonUtility.ToJson(testState);
        //JsonUtility.FromJson<State>(json);

        //UnityWebRequest setLight = UnityWebRequest.Put(request, json);
        //Debug.Log("Send triggered to " + request);
        //setLight.Send();
    }

}
