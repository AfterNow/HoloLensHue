using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System.Linq;

public class VoiceManager : MonoBehaviour {

    Dictionary<string, System.Action> keywords;
    KeywordRecognizer keywordRecognizer = null;

    public delegate void VoiceChangedColor(int id, Color color);
    public static event VoiceChangedColor voiceChangedColor;

    private GameObject hologramCollection;
    private SmartLightManager slm;

    [Tooltip("Brightness range 0 - 254 (Default: 160)")]
    public int dimValue = 100;
    [Tooltip("Brightness range 0 - 254 (Default: 80)")]
    public int dimMoreValue = 50;

    // assigned upon initialization of initBrightness call from SmartLightManager
    private SmartLight currentLight;
    // position of light in lights array. Needed as Hue API starts light array at '1'.
    private int arrayId;

    private List<string> colorList;

    private int wrapperLayerMask = 1 << 15;
    private RaycastHit hitInfo;

    // referencing MenuStates for convenience with Gaze and Speak Commands
    //private MenuStateManager.MenuState hiddenMenu;
    private MenuStateManager.MenuState mainMenuRef;
    private MenuStateManager.MenuState linkButtonRef;
    private MenuStateManager.MenuState linkSuccessRef;
    private MenuStateManager.MenuState identifyRef;
    private MenuStateManager.MenuState repeatRef;
    private MenuStateManager.MenuState setupFinishedRef;
    private MenuStateManager.MenuState tt_interactionsRef;
    private MenuStateManager.MenuState tt_voiceRef;
    private MenuStateManager.MenuState tt_gestureRef;
    private MenuStateManager.MenuState tt_hotspotRef;
    private MenuStateManager.MenuState ttFinishedRef;

    // Use this for initialization
    void Start () {
        Debug.Log("VoiceMgr Started");

        initColorList();

        if (GameObject.Find("HologramCollection") != null)
        {
            hologramCollection = GameObject.Find("HologramCollection");
        }
        else
        {
            Debug.LogError("No GameObject name HologramCollection can be found. This object should contain all holograms and the SmartLightManager");
        }

        RegisterPhrases();

        // referencing for convenience
        mainMenuRef = MenuStateManager.MenuState.MainMenu;
        linkButtonRef = MenuStateManager.MenuState.LinkButton;
        linkSuccessRef = MenuStateManager.MenuState.LinkSuccess;
        identifyRef = MenuStateManager.MenuState.Identify;
        repeatRef = MenuStateManager.MenuState.Repeat;
        setupFinishedRef = MenuStateManager.MenuState.SetupFinished;
        tt_interactionsRef = MenuStateManager.MenuState.TT_Interactions;
        tt_voiceRef = MenuStateManager.MenuState.TT_Voice;
        tt_gestureRef = MenuStateManager.MenuState.TT_Gesture;
        tt_hotspotRef = MenuStateManager.MenuState.TT_Hotspot;
        ttFinishedRef = MenuStateManager.MenuState.TTFinished;
    }
	
	// Update is called once per frame
	void Update () {
        var focusObject = GestureManager.Instance.FocusedObject;
        if (focusObject != null)
        {
            //slm.UpdateLightState(focusObject.name, param, value);
            //SmartLightManager.light
            if (focusObject.tag != "Untagged")
            {
                var idTag = focusObject.tag;
                //// Ignores focusObject if it does not have a valid id assigned to tag
                if (int.TryParse(idTag, out arrayId))
                {
                    currentLight = SmartLightManager.lights[arrayId];
                }
            }
        }
    }

    private void initColorList()
    {
        colorList = new List<string>();
        colorList.Add("Red");
        colorList.Add("Orange");
        colorList.Add("Yellow");
        colorList.Add("Green");
        colorList.Add("Blue");
        colorList.Add("Indigo");
        colorList.Add("Violet");
        colorList.Add("White");
        colorList.Add("Pink");
        colorList.Add("Purple"); 
    }

    public void RegisterPhrases()
    {
        // called outside of Start() to ensure the SmartLightManager has been loaded first 
        slm = hologramCollection.GetComponent<SmartLightManager>();

        keywords = new Dictionary<string, System.Action>();

        /// <summary>
        /// Global systemwide voice commands
        /// </summary>
        ///

        // shows and hides the Voice Command Board with keyPhrases and associated actions
        keywords.Add("Show Voice Commands", () =>
        {
            SendMessage("OpenVCBoard", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Show The Voice Commands", () =>
        {
            SendMessage("OpenVCBoard", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Show Voice Command Menu", () =>
        {
            SendMessage("OpenVCBoard", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Show Voice Menu", () =>
        {
            SendMessage("OpenVCBoard", SendMessageOptions.DontRequireReceiver);
        });
        // Hides Voice Command Board
        keywords.Add("Hide Voice Commands", () =>
        {
            SendMessage("CloseVCBoard", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Hide The Voice Commands", () =>
        {
            SendMessage("CloseVCBoard", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Hide Voice Command Menu", () =>
        {
            SendMessage("CloseVCBoard", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Hide Voice Menu", () =>
        {
            SendMessage("CloseVCBoard", SendMessageOptions.DontRequireReceiver);
        });
        // Repositions the Voice Command Board in front of user 
        keywords.Add("Reposition Voice Commands", () =>
        {
            SendMessage("ResetVCBoardPosition", SendMessageOptions.DontRequireReceiver);
        });

        // reset app back to starting state
        keywords.Add("Reset And Search For Bridge", () =>
        {
            SendMessage("RetrySetup", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Reset And Search For A Bridge", () =>
        {
            SendMessage("RetrySetup", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Try Searching Again", () =>
        {
            SendMessage("RetrySetup", SendMessageOptions.DontRequireReceiver);
        });

        // opens the main menu of the app
        keywords.Add("Show Main Menu", () =>
        {
            SendMessage("InitMainMenu", SendMessageOptions.DontRequireReceiver);
        });
        keywords.Add("Show The Main Menu", () =>
        {
            SendMessage("InitMainMenu", SendMessageOptions.DontRequireReceiver);
        });
        // closes the main menu of the app
        keywords.Add("Hide Main Menu", () =>
        {
            NotificationManager.Instance.DismissAction();;
        });
        keywords.Add("Hide The Main Menu", () =>
        {
            NotificationManager.Instance.DismissAction();
        });
        keywords.Add("Dismiss Menu", () =>
        {
            NotificationManager.Instance.DismissAction();
        });

        // runs a search function to discover Hue Bridges on the same network.
        keywords.Add("Check For A Bridge", () =>
        {
            SendMessage("RecheckOrGetBridgeIP", SendMessageOptions.DontRequireReceiver);
        });

        // runs a search for existing usernames associated with the Bridge ip. If none are found, creates one.
        keywords.Add("Link To Bridge", () =>
        {
            SendMessage("RecheckOrCreateBridgeUser", SendMessageOptions.DontRequireReceiver);
        });

        // Changes current state of app. The Configuration state displays all found lights and their position
        keywords.Add("Configure Room", () =>
        {
            StateManager.Instance.CurrentState = StateManager.HueAppState.Configuring;
        });
        keywords.Add("Configure The Room", () =>
        {
            StateManager.Instance.CurrentState = StateManager.HueAppState.Configuring;
        });
        keywords.Add("Setup The Room", () =>
        {
            StateManager.Instance.CurrentState = StateManager.HueAppState.Configuring;
        });
        keywords.Add("Show All Lights", () =>
        {
            StateManager.Instance.CurrentState = StateManager.HueAppState.Configuring;
        });

        // Changes current state of app. Saves configuration and switches back into main mode - Ready
        keywords.Add("Save The Room", () =>
        {
            StateManager.Instance.CurrentState = StateManager.HueAppState.Ready;
        });
        keywords.Add("Save Room", () =>
        {
            StateManager.Instance.CurrentState = StateManager.HueAppState.Ready;
        });
        keywords.Add("Hide All Lights", () =>
        {
            StateManager.Instance.CurrentState = StateManager.HueAppState.Ready;
        });

        /// <summary>
        /// Collection of lights voice commands
        /// </summary>
        /// 
        // resets all lights back to default hue, full saturation, and full brightness.
        keywords.Add("Normal Lights", () =>
        {
            SmartLightManager.Instance.SetLightsToDefault();
        });
        keywords.Add("Normal Lighting", () =>
        {
            SmartLightManager.Instance.SetLightsToDefault();
        });
        keywords.Add("Reset Lights", () =>
        {
            SmartLightManager.Instance.SetLightsToDefault();
        });
        keywords.Add("Reset Lighting", () =>
        {
            SmartLightManager.Instance.SetLightsToDefault();
        });
        keywords.Add("Regular Lights", () =>
        {
            SmartLightManager.Instance.SetLightsToDefault();
        });
        keywords.Add("Regular Lighting", () =>
        {
            SmartLightManager.Instance.SetLightsToDefault();
        });
        keywords.Add("Natural Lights", () =>
        {
            SmartLightManager.Instance.SetLightsToDefault();
        });
        keywords.Add("Natural Lighting", () =>
        {
            SmartLightManager.Instance.SetLightsToDefault();
        });

        // turns off all available lights
        keywords.Add("Turn Off All Light", () =>
        {
            SmartLightManager.Instance.TurnOffAllLights();
        });
        keywords.Add("Turn Off The Lights", () =>
        {
            SmartLightManager.Instance.TurnOffAllLights();
        });

        // turns on all available lights
        keywords.Add("Turn On All Light", () =>
        {
            SmartLightManager.Instance.TurnOnAllLights();
        });
        keywords.Add("Turn On The Lights", () =>
        {
            SmartLightManager.Instance.TurnOnAllLights();
        });
        
        // sets all available lights' brightness to dim value
        keywords.Add("Dim All Lights", () =>
        {
            SmartLightManager.Instance.DimAllLights();
        });
        keywords.Add("Dim All The Lights", () =>
        {
            SmartLightManager.Instance.DimAllLights();
        });
        // sets all available lights' brightness to full value
        keywords.Add("Undim All Lights", () =>
        {
            SmartLightManager.Instance.UndimAllLights();
        });
        keywords.Add("Undim All The Lights", () =>
        {
            SmartLightManager.Instance.UndimAllLights();
        });

        foreach (string color in colorList)
        {
            string colorCommand = "Turn All Lights " + color;

            keywords.Add(colorCommand, () =>
            {
                int hue;
                hue = ColorService.GetHueByColor(color);

                SmartLightManager.Instance.TurnAllLightsToColor(hue);
            });
        }

        /// <summary>
        /// Individual light voice commands
        /// </summary>
        /// 
        // On/Off commands
        keywords.Add("Light On", () =>
        {
            buildUpdateCall("On", 0);
        });
        keywords.Add("Turn On", () =>
        {
            buildUpdateCall("On", 0);
        });

        keywords.Add("Light Off", () =>
        {
            buildUpdateCall("Off", 0);
        });
        keywords.Add("Turn Off", () =>
        {
            buildUpdateCall("Off", 0);
        });

        // color change commands 
        foreach (string color in colorList)
        {
            string colorCommand = "Turn Light " + color;

            keywords.Add(colorCommand, () =>
            {
                int hue;
                hue = ColorService.GetHueByColor(color);

                buildUpdateCall("hue", hue);
            });
        }
        foreach (string color in colorList)
        {
            string colorCommand = "Set To " + color;

            keywords.Add(colorCommand, () =>
            {
                int hue;
                hue = ColorService.GetHueByColor(color);

                buildUpdateCall("hue", hue);
            });
        }

        // Brightness adjustment commands
        keywords.Add("Dim Light", () =>
        {
            buildUpdateCall("bri", dimValue);
        });
        keywords.Add("Dim The Light", () =>
        {
            buildUpdateCall("bri", dimValue);
        });

        keywords.Add("Dim Light More", () =>
        {
            buildUpdateCall("bri", dimMoreValue);
        });

        keywords.Add("Full Brightness", () =>
        {
            buildUpdateCall("bri", 254);
        });
        keywords.Add("Undim the light", () =>
        {
            buildUpdateCall("bri", 254);
        });

        // flashes the corresponding light of the currently focused gameObject
        keywords.Add("Identify Light", () =>
        {
            buildUpdateCall("alert", 1);
        });
        keywords.Add("What Light Am I", () =>
        {
            buildUpdateCall("alert", 1);
        });

        // stops flashing of light prior to 15 second default time
        keywords.Add("OK That's Enough", () =>
        {
            buildUpdateCall("alert", 0);
        });
        keywords.Add("That's Enough", () =>
        {
            buildUpdateCall("alert", 0);
        });
        keywords.Add("Stop Blinking", () =>
        {
            buildUpdateCall("alert", 0);
        });

        /// <summary>
        /// Hotspots Commands
        /// </summary>
        /// 

        keywords.Add("Easter Egg", () =>
        {
            HotspotManager.Instance.EnableHotspots();
        });

        // Deactivates hotspots and removes all hotspot gameObjects
        keywords.Add("Destroy Easter Egg", () =>
        {
            HotspotManager.Instance.DisableHotspots();
        });

        // Shows all hotspots by enabling their mesh
        keywords.Add("Show All Hot Spots", () =>
        {
            HotspotManager.Instance.ShowAllHotspots();
        });

        // Hides all hotspots by disabling their mesh
        keywords.Add("Hide All Hot Spots", () =>
        {
            HotspotManager.Instance.HideAllHotspots();
        });


        /// <summary>
        /// "Gaze it say it" commands
        /// </summary>
        /// 

        keywords.Add("Tutorial", () =>
        {
            // TODO make a more robust and less brittle solution
            if (MenuStateManager.Instance.CurrentState == mainMenuRef)
            {
                bool hitMenuWrapper;
                hitMenuWrapper = raycastHitMenuWrapper();

                if (hitMenuWrapper)
                {
                    NotificationManager.Instance.TutorialAction();
                }
            }
        });

        keywords.Add("Next", () =>
        {
            // TODO make a more robust and less brittle solution
            if (MenuStateManager.Instance.CurrentState == linkSuccessRef || MenuStateManager.Instance.CurrentState == identifyRef
            || MenuStateManager.Instance.CurrentState == tt_interactionsRef || MenuStateManager.Instance.CurrentState == tt_voiceRef
            || MenuStateManager.Instance.CurrentState == tt_gestureRef)
            {
                bool hitMenuWrapper;
                hitMenuWrapper = raycastHitMenuWrapper();

                if (hitMenuWrapper)
                {
                    NotificationManager.Instance.NextAction();
                }
            }       
        });

        keywords.Add("Back", () =>
        {
            // TODO make a more robust and less brittle solution
            if (MenuStateManager.Instance.CurrentState == identifyRef || MenuStateManager.Instance.CurrentState == repeatRef
            || MenuStateManager.Instance.CurrentState == tt_voiceRef || MenuStateManager.Instance.CurrentState == tt_gestureRef
            || MenuStateManager.Instance.CurrentState == tt_hotspotRef)
            {
                bool hitMenuWrapper;
                hitMenuWrapper = raycastHitMenuWrapper();

                if (hitMenuWrapper)
                {
                    NotificationManager.Instance.BackAction();
                }
            }
        });

        keywords.Add("Finish Tutorial", () =>
        {
            // TODO make a more robust and less brittle solution
            if (MenuStateManager.Instance.CurrentState == tt_hotspotRef)
            {
                bool hitMenuWrapper;
                hitMenuWrapper = raycastHitMenuWrapper();

                if (hitMenuWrapper)
                {
                    NotificationManager.Instance.FinishAction();
                }
            }
        });

        keywords.Add("Setup", () =>
        {
            // TODO make a more robust and less brittle solution
            if (MenuStateManager.Instance.CurrentState == mainMenuRef)
            {
                bool hitMenuWrapper;
                hitMenuWrapper = raycastHitMenuWrapper();

                if (hitMenuWrapper)
                {
                    NotificationManager.Instance.SetupAction();
                }
            }
        });

        keywords.Add("Save Configuration", () =>
        {
            // TODO make a more robust and less brittle solution
            if (MenuStateManager.Instance.CurrentState == repeatRef)
            {
                bool hitMenuWrapper;
                hitMenuWrapper = raycastHitMenuWrapper();

                if (hitMenuWrapper)
                {
                    NotificationManager.Instance.SaveAction();
                }
            }
        });

        /// <summary>
        /// Hidden color commands
        /// </summary>
        /// 

        // Easter Egg color themes by keyPhrase
        keywords.Add("Set To Disco", () =>
        {
            string[] themeColors = new string[5];
            themeColors[0] = "Blue";
            themeColors[1] = "Pink";
            themeColors[2] = "Red";
            themeColors[3] = "Yellow";
            themeColors[4] = "Orange";
            SmartLightManager.Instance.ColorTheme(themeColors, true);
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            Debug.Log("Here is args.text: " + args.text);
            keywordAction.Invoke();
        }
    }

    void buildUpdateCall(string param, int value)
    {
        var focusedObject = GestureManager.Instance.FocusedObject;
        //Debug.Log("here is focused object: " + focusObject);
        //if (focusObject != null)
        //{
        //    slm.UpdateLightState(focusObject.name, param, value);
        //    //SmartLightManager.light
        //}
        if (focusedObject != null)
        {
            // retrieves array index (arrayId) from the tag assigned in SmartLightManager
            if (focusedObject.tag != "Untagged")
            {
                var idTag = focusedObject.tag;
                //// Ignores focusedObject if it does not have a valid id assigned to tag
                if (int.TryParse(idTag, out arrayId))
                {
                    currentLight = SmartLightManager.lights[arrayId];
                    if (param == "On")
                    {
                        currentLight.State.On = true;
                    }
                    else if (param == "Off")
                    {
                        currentLight.State.On = false;
                    }
                    else if (param == "hue")
                    {
                        // auto sets saturation to full to show vibrant colors. Will replace when Saturation UI is added in another version
                        currentLight.State.Sat = 254;

                        currentLight.State.Hue = value;

                        if (voiceChangedColor != null)
                        {
                            // arrayID is adjusted to compensate for diff in Hue id
                            voiceChangedColor(arrayId + 1, ColorService.GetColorByHue(value));
                        }
                    }
                    else if (param == "bri")
                    {
                        currentLight.State.Bri = value;
                    }
                    else if (param == "alert")
                    {
                        if (value == 0)
                        {
                            currentLight.State.Alert = "none";
                        }
                        else
                        {
                            currentLight.State.Alert = "lselect";
                        }
                    }
                    // hueAPI.UpdateLight(currentLight);
                    SmartLightManager.UpdateLightState(arrayId);
                    currentLight.State.Alert = "none";
                }
                else
                {
                    Debug.Log("a tag with a valid array index (arrayId) could not be found on focusedObject");
                }
            }
            else
            {
                Debug.Log("No tag containing arrayId was found on this focusedObject.");
            }
        }
    }

    private bool raycastHitMenuWrapper()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo,
            5f, wrapperLayerMask))
        {
            if (hitInfo.collider.name == "MenuWrapper")
            {
                return true;
            }
        }
        return false;
    }
}
