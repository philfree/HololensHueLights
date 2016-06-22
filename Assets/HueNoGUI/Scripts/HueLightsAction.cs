using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HueLightsAction : MonoBehaviour {

    [Tooltip("GameObject with the HueBridgeManager script")]
    public GameObject HueBridge_gameobject;
    private HueBridgeManager hueBridge;
    private List<HueLamp> lamps;
    private HueLamp lamp;

    // Use this for initialization
    void Start()
    {
        //GameObject HueBridge_gameobject = GameObject.Find("Managers");
        hueBridge = HueBridge_gameobject.GetComponent<HueBridgeManager>();
    }

    void OnSelect()
    {
        HueLamp[] lamps = hueBridge.GetComponentsInChildren<HueLamp>();
        foreach (HueLamp lamp in lamps)
        {
            Debug.Log("lamp check on Update: " + lamp.devicePath + " : " + lamp.on);
        }
        foreach (HueLamp lamp in lamps)
        {
            Debug.Log("lamp before: " + lamp.devicePath + " : " + lamp.on);

            // Get the second lamp using device path.
            if (lamp.devicePath == "2")
            {
                if (lamp.on == true)
                {
                    lamp.on = false;
                }
                else
                {
                    lamp.on = true;
                    lamp.color = Color.green;
                }
            }

            Debug.Log("lamp after: " + lamp.devicePath + " : " + lamp.on);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            HueLamp[] lamps = hueBridge.GetComponentsInChildren<HueLamp>();
            foreach (HueLamp lamp in lamps)
            {
                Debug.Log("lamp check on Update: " + lamp.devicePath + " : " + lamp.on);
            }
            foreach (HueLamp lamp in lamps)
            {
                Debug.Log("lamp before: " + lamp.devicePath + " : " + lamp.on);

                // Get the second lamp using device path.
                if (lamp.devicePath == "2")
                {
                    if (lamp.on == true)
                    {
                        lamp.on = false;
                    }
                    else
                    {
                        lamp.on = true;
                        lamp.color = Color.green;
                    }
                }

                Debug.Log("lamp after: " + lamp.devicePath + " : " + lamp.on);

            }



        }

    }
}
