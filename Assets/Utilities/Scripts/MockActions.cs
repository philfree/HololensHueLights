using UnityEngine;
using System.Collections;

public class MockActions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MockGaze()
    {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
            30.0f))
        {

            Debug.Log("hitInfo normal: " + hitInfo.normal);
            //this.transform.position = hitInfo.point;

            //// Rotate this object to face the user.
            //Quaternion toQuat = Camera.main.transform.localRotation;
            //toQuat.x = 0;
            //toQuat.z = 0;
            //this.transform.rotation = toQuat;
        }
    }
}
