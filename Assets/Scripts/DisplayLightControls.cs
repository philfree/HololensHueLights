using UnityEngine;
using System.Collections;

public class DisplayLightControls : MonoBehaviour {

    private GameObject controls;
	// Use this for initialization
    
	void Start () {
        controls = GameObject.Find("lampControls");
        Debug.Log("Controls status on start:" + controls.activeSelf);
        controls.SetActive(false);

    }
	
    void OnSelect ()
    {

        if (controls.activeSelf == false)
        {
            controls.SetActive(true);
        } else
        {
            controls.SetActive(false);
        }
        
    }


	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("space"))
        {
            OnSelect();

        }

    }
}
