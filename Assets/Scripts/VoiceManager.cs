using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System.Linq;

public class VoiceManager : MonoBehaviour {

    Dictionary<string, System.Action> keywords;
    KeywordRecognizer keywordRecognizer = null;

    private GameObject hologramCollection;
    private SmartLightManager slm;

    [Tooltip("Brightness range 0 - 254 (Default: 160)")]
    public int dimValue = 100;
    [Tooltip("Brightness range 0 - 254 (Default: 80)")]
    public int dimMoreValue = 50;

    // Use this for initialization
    void Start () {

        if (GameObject.Find("HologramCollection") != null)
        {
            hologramCollection = GameObject.Find("HologramCollection");
        }
        else
        {
            Debug.LogError("No GameObject name HologramCollection can be found. This object should contain all holograms and the SmartLightManager");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RegisterPhrases()
    {
        // called outside of Start() to ensure the SmartLightManager has been loaded first 
        slm = hologramCollection.GetComponent<SmartLightManager>();

        keywords = new Dictionary<string, System.Action>();

        // Global light commands
        keywords.Add("Normal Lights", () =>
        {
            slm.SetLightsToDefault();
        });

        // On/Off commands
        keywords.Add("Light On", () =>
        {
            buildUpdateCall("On", 0);
        });

        keywords.Add("Light Off", () =>
        {
            buildUpdateCall("Off", 0);
        });

        // color change commands
        keywords.Add("Set To Red", () =>
        {
            int hue;
            hue = ColorService.GetHueByColor("Red");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Orange", () =>
        {
            int hue;
            hue = ColorService.GetHueByColor("Orange");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Yellow", () =>
        {
            int hue;
            hue = ColorService.GetHueByColor("Yellow");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Green", () =>
        {
            int hue;
            hue = ColorService.GetHueByColor("Green");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To White", () =>
        {
            int hue;
            hue = ColorService.GetHueByColor("White");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Blue", () =>
        {
            int hue;
            hue = ColorService.GetHueByColor("Blue");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Purple", () =>
        {
            int hue;
            hue = ColorService.GetHueByColor("Purple");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Pink", () =>
        {
            int hue;
            hue = ColorService.GetHueByColor("Pink");

            buildUpdateCall("hue", hue);
        });

        // Brightness adjustment commands
        keywords.Add("Dim Light", () =>
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

        // flashes the corresponding light of the currently focused gameObject
        keywords.Add("Identify Light", () =>
        {
            buildUpdateCall("alert", 1);
        });

        // stops flashing of light prior to 15 second default time
        keywords.Add("OK That's Enough", () =>
        {
            buildUpdateCall("alert", 0);
        });

        // system voice commands
        keywords.Add("Show Voice Menu", () =>
        {
            //showVCMenu(true);
        });

        keywords.Add("Hide Voice Menu", () =>
        {
            //showVCMenu(false);
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        // populates voice control help menu with available commands
        //buildMenu(keywords);
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
        var focusObject = GestureManager.Instance.FocusedObject;
        if (focusObject != null)
        {
            slm.UpdateLightState(focusObject.name, param, value);
        }
    }
}
