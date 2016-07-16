using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;
using UnityEngine.VR.WSA.Persistence;

using MiniJSON;

public class SmartLightManager : MonoBehaviour
{
    public List<SmartLight> lights;
    ColorService colorService;

    HueBridgeManager hueBridgeManager;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    GameObject lightCollection;
    List<SmartLight> smartLights;
    bool showLights;

    Object lightObject;
    Object lightPrefab;
    Vector3 cameraPos;

    // Use this for initialization
    void Awake()
    {
        hueBridgeManager = new HueBridgeManager();
    }

    void Start()
    {
        lights = new List<SmartLight>();
        colorService = new ColorService();
        lightPrefab = Resources.Load("Prefabs/SmartBulb");
    }

    void Update()
    {

    }

    public void createLights(List<SmartLight> lc)
    {
        lights = lc;
        InstantiateLights();
    }

    // creates smart light game objects and sets color of prefab
    void InstantiateLights()
    {
        lightCollection = new GameObject("LightCollection");
        lightCollection.AddComponent<AdjustmentPlane>();

        Vector3 pos = new Vector3(-1, 0, 2);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, cameraPos);

        foreach (SmartLight light in lights)
        {
            lightObject = Instantiate(lightPrefab, pos, rotation);
            lightObject.name = light.getName();

            // gets newly instantiated GameObject and sets to child of LightCollection 
            GameObject currentLight = GameObject.Find(light.getName());
            //currentLight.AddComponent<SetLightStateAction>();
            currentLight.transform.parent = lightCollection.transform;

            // sets color of light prefab based on current light hue state
            Renderer rend = currentLight.GetComponent<Renderer>();
            Vector4 ledColor = colorService.GetColorByHue(light.getState().hue);
            rend.material.color = ledColor;

            if (!ActivityStateManager.Instance.IsEditMode)
            {
                rend.enabled = true;
            }
            pos += new Vector3(1, 0, 0);
        }
    }

    public void ToggleShowSmartLights()
    {
        ActivityStateManager.Instance.ToggleEditMode();
        if (ActivityStateManager.Instance.IsEditMode || ActivityStateManager.Instance.IsRevealMode)
        {
            foreach (Transform child in lightCollection.transform)
            {
                child.GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            foreach (Transform child in lightCollection.transform)
            {
                child.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    public List<SmartLight> getSmartLightList()
    {
        Debug.Log("I was called");
        return lights;
    }

    private IEnumerator updateLight(int lightID)
    {
        HueBridgeManager bridge = GetComponentInParent<HueBridgeManager>();
        // compensate for array indexing to prevent last light from being out of range
        int adjustedID = (lightID - 1);

        SmartLightState state = lights[adjustedID].getState();

        string request = "http://" + bridge.bridgeip + "/api/" + bridge.username + "/lights/" + lightID.ToString() + "/state";
        Debug.Log("Send triggered to " + request);

        string json = JsonUtility.ToJson(state);
        Debug.Log("here is the json being sent " + json);

        UnityWebRequest www = UnityWebRequest.Put(request, json);
        yield return www.Send();
    }

    public void UpdateLight(int lightId)
    {
        StartCoroutine(updateLight(lightId));
    }

    public void UpdateState(int lightID, string param, int value)
    {
        SmartLightState currentState;
        // compensate for array indexing to prevent last light from being out of range
        int adjustedID = (lightID - 1);

        currentState = lights[adjustedID].getState();
        if (param == "On")
        {
            currentState.isOn(true);
        }
        else if (param == "Off")
        {
            currentState.isOn(false);
        }
        else if (param == "hue")
        {
            currentState.setHue(value);
            currentState.setSat(254);
        }
        else if (param == "bri")
        {
            currentState.setBri(value);
        }
        else if (param == "alert")
        {
            if (value == 0)
            {
                Debug.Log("OK phrase understood");
                currentState.setAlert("none");
            }
            else
            {
                currentState.setAlert("lselect");
            }
        }
        UpdateLight(lightID);
    }
}
