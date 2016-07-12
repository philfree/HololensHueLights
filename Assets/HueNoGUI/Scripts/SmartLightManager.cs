using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SmartLightManager : MonoBehaviour
{

    public List<SmartLight> lights;
    ColorService colorService;

    HueBridgeManager hueBridgeManager;

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
                rend.enabled = false;
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

    List<SmartLight> getSmartLightList()
    {
        HueBridgeManager bridge = GetComponentInParent<HueBridgeManager>();
        return bridge.GetLightCollection();
    }

    private IEnumerator updateLight(int lightID)
    {
        HueBridgeManager bridge = GetComponentInParent<HueBridgeManager>();

        //UnityWebRequest www = UnityWebRequest.Put("http://" + bridge.bridgeip + "/api/" + bridge.username + "/lights/" + devicePath + "/state", jsonData);

        SmartLightState testState = lights[lightID].getState();
        testState.isOn(false);

        string request = "http://" + bridge.bridgeip + "/api/" + bridge.username + "/lights/" + lightID.ToString() + "/state";
        Debug.Log("Send triggered to " + request);

        // converts List into json string. String able to be passed as body on PUT request
        string json = JsonUtility.ToJson(testState);

        UnityWebRequest www = UnityWebRequest.Put(request, json);
        yield return www.Send();
    }

    public void UpdateLight(int lightId)
    {
        StartCoroutine(updateLight(lightId));
    }

    public void LightOff(int lightID)
    {
        Debug.Log("light off triggered: " + lightID);
        lights[lightID].getState().isOn(false);
        UpdateLight(lightID);
    }
    public void LightOn(int lightID)
    {
        Debug.Log("light on triggered");
        lights[lightID].getState().isOn(true);
        UpdateLight(lightID);
    }
}
