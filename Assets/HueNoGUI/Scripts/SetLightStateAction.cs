using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class SetLightStateAction : MonoBehaviour {

    public int lightID;
    public SmartLightManager smartLightManager;

	// Use this for initialization
	void Start () {
        Debug.Log("this was STARTED");
        // TODO Temp Light ID assigment
        string go = gameObject.name;
        if (go == "Hue color lamp 1")
        {
            lightID = 1;
        }
        else if (go == "Hue color lamp 2")
        {
            Debug.Log("yes");
            lightID = 2;
        }
        else if (go  == "Hue color lamp 3")
        {
            lightID = 3;
        }
        else if (go == "Hue color lamp 4")
        {
            lightID = 4;
        }
        else if (go == "Hue bloom 1")
        {
            lightID = 5;
        }
        Debug.Log("this is GO: " + go);

    }
	
	// Update is called once per frame

    void LateUpdate()
    {
        
    }

    public void lightOff()
    {
        GameObject manager = GameObject.Find("Manager");
        smartLightManager = manager.GetComponent<SmartLightManager>();
        Debug.Log("wtf" + lightID);
        if (GazeManager.Instance.Hit)
        {
            if (GazeManager.Instance.HitInfo.collider)
            {
                smartLightManager.LightOff(lightID);
            }
        }
    }
    public void lightOn()
    {
        GameObject manager = GameObject.Find("Manager");
        smartLightManager = manager.GetComponent<SmartLightManager>();

        Debug.Log(smartLightManager);
        if (GazeManager.Instance.Hit)
        {
            if (GazeManager.Instance.HitInfo.collider)
            {
                smartLightManager.LightOn(lightID);
            }
        }
    }
}
