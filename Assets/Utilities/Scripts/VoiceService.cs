using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;

/// <summary>
/// Register all new keywords and phrases in this service class
///
/// IMPORTANT: Please make sure to add the microphone capability in your app, in Unity under
/// Edit -> Project Settings -> Player -> Settings for Windows Store -> Publishing Settings -> Capabilities
/// or in your Visual Studio Package.appxmanifest capabilities.
/// </summary>
/// 
public class VoiceService : MonoBehaviour {

    Dictionary<string, System.Action> keywords;
    KeywordRecognizer keywordRecognizer = null;

    SmartLightManager smartLightManager;
    ColorService colorService;

    List<SmartLight> lights;

    [Tooltip("Brightness range 0 - 254 (Default: 160)")]
    public int dimValue = 160;
    [Tooltip("Brightness range 0 - 254 (Default: 80)")]
    public int dimMoreValue = 80;

    void Start () {
    }

    public void RegisterPhrases()
    {

        // called outside of Start() to ensure the SmartLightManager has been loaded first 
        smartLightManager = gameObject.GetComponent<SmartLightManager>();
        colorService = new ColorService();

        // gets each light in the SmartLight class from the SmartLightManager
        lights = smartLightManager.getSmartLightList();

        keywords = new Dictionary<string, System.Action>();

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
            hue = colorService.GetHueByColor("Red");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Yellow", () =>
        {
            int hue;
            hue = colorService.GetHueByColor("Yellow");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Green", () =>
        {
            int hue;
            hue = colorService.GetHueByColor("Green");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To White", () =>
        {
            int hue;
            hue = colorService.GetHueByColor("White");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Blue", () =>
        {
            int hue;
            hue = colorService.GetHueByColor("Blue");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Purple", () =>
        {
            int hue;
            hue = colorService.GetHueByColor("Purple");

            buildUpdateCall("hue", hue);
        });

        keywords.Add("Set To Pink", () =>
        {
            int hue;
            hue = colorService.GetHueByColor("Pink");

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



        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();


    }

    void buildUpdateCall(string param, int value)
    {
        var focusObject = GestureManager.Instance.FocusedObject;
        if (focusObject != null)
        {
            Debug.Log("focused object: " + focusObject);
            foreach (SmartLight light in lights)
            {
                if (light.getName() == focusObject.name)
                {
                    smartLightManager.UpdateState(light.getID(), param, value);
                }
            }
        }
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        Debug.Log("here is just plain args " + args);
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            Debug.Log("Here is args.text: " + args.text);
            keywordAction.Invoke();
        }
    }


    // Mock keyword testing functions 
    // keyword testing functions
    public void MockFocusedItem()
    {
        buildUpdateCall("On", 0);
    }
}
