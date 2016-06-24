using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetLightColorAction : MonoBehaviour {

    public Color color;
    [Tooltip("GameObject with the HueBridgeManager script")]
    public GameObject HueBridge_gameobject;
    private HueBridgeManager hueBridge;
    private List<HueLamp> lamps;
    private HueLamp lamp;

    // Use this for initialization
    void Start () {
        Renderer rend = GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", color);
        rend.material.SetColor("_Color", color);
        //gameObject.GetComponent<Renderer>().material.color = color;
        hueBridge = HueBridge_gameobject.GetComponent<HueBridgeManager>();

    }
	
    void OnSelect ()
    {
        HueLamp[] lamps = hueBridge.GetComponentsInChildren<HueLamp>();
        foreach (HueLamp lamp in lamps)
        {
            //Debug.Log("lamp before: " + lamp.devicePath + " : " + lamp.on);

            // Get the second lamp using device path.
            if (lamp.devicePath == "2")
            {
                    lamp.on = true;
                    lamp.color = color;
            }

            //Debug.Log("lamp after: " + lamp.devicePath + " : " + lamp.on);

        }

    }
	// Update is called once per frame
	void Update () {


    }
}
