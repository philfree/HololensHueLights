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

    void Start () {
    }

    public void RegisterPhrases()
    {
        smartLightManager = gameObject.GetComponent<SmartLightManager>();
        colorService = new ColorService();

        // gets each light in the SmartLight class from the SmartLightManager
        lights = smartLightManager.getSmartLightList();

        keywords = new Dictionary<string, System.Action>();
        keywords.Add("Light On", () =>
        {
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        Debug.Log("Match!!!: " + focusObject.name);
                        smartLightManager.LightOn(light.getID());
                    }
                }
            }
        });

        keywords.Add("Light Off", () =>
        {
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        smartLightManager.LightOff(light.getID());
                    }
                }
            }
        });

        // TODO refactor to one phrase with color as the variable. This is not DRY...at all
        keywords.Add("Set To Red", () =>
        {
            int hue;
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        hue = colorService.GetHueByColor("Red");
                        smartLightManager.UpdateColor(light.getID(), hue);
                    }
                }
            }
        });

        keywords.Add("Set To Yellow", () =>
        {
            int hue;
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        hue = colorService.GetHueByColor("Yellow");
                        smartLightManager.UpdateColor(light.getID(), hue);
                    }
                }
            }
        });

        keywords.Add("Set To Green", () =>
        {
            int hue;
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        hue = colorService.GetHueByColor("Green");
                        smartLightManager.UpdateColor(light.getID(), hue);
                    }
                }
            }
        });

        keywords.Add("Set To White", () =>
        {
            int hue;
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        hue = colorService.GetHueByColor("White");
                        smartLightManager.UpdateColor(light.getID(), hue);
                    }
                }
            }
        });

        keywords.Add("Set To Blue", () =>
        {
            int hue;
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        hue = colorService.GetHueByColor("Blue");
                        smartLightManager.UpdateColor(light.getID(), hue);
                    }
                }
            }
        });

        keywords.Add("Set To Purple", () =>
        {
            int hue;
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        hue = colorService.GetHueByColor("Purple");
                        smartLightManager.UpdateColor(light.getID(), hue);
                    }
                }
            }
        });

        keywords.Add("Set To Pink", () =>
        {
            int hue;
            var focusObject = GestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Debug.Log("FO: " + focusObject);
                foreach (SmartLight light in lights)
                {
                    if (light.getName() == focusObject.name)
                    {
                        hue = colorService.GetHueByColor("Pink");
                        smartLightManager.UpdateColor(light.getID(), hue);
                    }
                }
            }
        });




        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();


    }

    // Update is called once per frame
    void Update () {
	
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
        var focusObject = GestureManager.Instance.FocusedObject;
        if (focusObject != null)
        {
            lights = smartLightManager.getSmartLightList();
            Debug.Log("FO: " + focusObject);
            foreach (SmartLight light in lights)
            {
                if (light.getName() == focusObject.name)
                {
                    Debug.Log("Match!!!: " + focusObject.name);
                    smartLightManager.LightOn(light.getID());
                }
            }
        }
    }
}
