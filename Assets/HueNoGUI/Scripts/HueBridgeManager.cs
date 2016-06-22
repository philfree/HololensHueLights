using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using MiniJSON;

public class HueBridgeManager : MonoBehaviour {

    [Tooltip("IP address of the hue bridge: https://www.meethue.com/api/nupnp")]
    public string bridgeip = "127.0.0.1";
    public int portNumber = 8000;
    [Tooltip("Developer username")]
    public string username = "newdeveloper";
    // Use this for initialization

    void Start () {
        if ((!bridgeip.Equals("127.0.0.1")) && (!username.Equals("newdeveloper")))
        {
            StartCoroutine(DiscoverLights());
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator DiscoverLights()
    {
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create();
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        UnityWebRequest lights_json = UnityWebRequest.Get("http://" + bridgeip + "/api/" + username + "/lights");
        yield return lights_json.Send();

        Debug.Log("http" + bridgeip + portNumber + "/api/" + username + "/lights");

        //System.IO.Stream stream = response.GetResponseStream();
       // System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);

        var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);
        foreach (string key in lights.Keys)
        {
            var light = (Dictionary<string, object>)lights[key];

            foreach (HueLamp hueLamp in GetComponentsInChildren<HueLamp>())
            {
                if (hueLamp.devicePath.Equals(key)) goto Found;
            }

            if (light["type"].Equals("Extended color light"))
            {

                GameObject gameObject = new GameObject();
                gameObject.name = (string)light["name"];
                gameObject.transform.parent = transform;
                gameObject.AddComponent<HueLamp>();
                HueLamp lamp = gameObject.GetComponent<HueLamp>();
                lamp.devicePath = key;

            }

        Found:
            ;
        }
    }
}
