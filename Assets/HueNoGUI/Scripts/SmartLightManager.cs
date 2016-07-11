using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartLightManager : MonoBehaviour
{

    public List<SmartLight> lights;
    ColorService colorService;

    HueBridgeManager hueBridgeManager;

    GameObject lightCollection;
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
            currentLight.transform.parent = lightCollection.transform;

            // sets color of light prefab based on current light hue state
            Renderer rend = currentLight.GetComponent<Renderer>();
            Vector4 ledColor = colorService.GetColorByHue(light.getState().hue);
            rend.material.color = ledColor;

            currentLight.GetComponent<Renderer>().enabled = true;
            pos += new Vector3(1, 0.2f, 0);
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
}
