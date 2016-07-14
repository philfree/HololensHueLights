using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MockSmartLights : MonoBehaviour
{

    public List<SmartLight> lights;
    SmartLight light1;
    SmartLight light2;
    SmartLight light3;

    // Use this for initialization
    void Awake()
    {
        lights = getLights();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<SmartLight> getLights()
    {
        List<SmartLight> mockLights;
        mockLights = new List<SmartLight>();
        light1 = new SmartLight(
            1,
            "kitchen",
            "LC0015",
            new SmartLightState(true, 254, 35000, 254, "none", "none"));
        light2 = new SmartLight(
            2,
            "floorstanding",
            "LC0015",
            new SmartLightState(false, 254, 53000, 254, "none", "none"));
        light3 = new SmartLight(
            3,
            "desktop",
            "LC0015",
            new SmartLightState(true, 100, 65535, 254, "none", "none"));

        mockLights.Add(light1);
        mockLights.Add(light2);
        mockLights.Add(light3);

        return mockLights;
    }
}
