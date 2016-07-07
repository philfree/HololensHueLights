using UnityEngine;
using System;
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

    public UnityWebRequest lights_json;
    List<SmartLight> smartLights = null;

    void Awake()
    {
            smartLights = new List<SmartLight>();
    }
    void Start () {
        if ((!bridgeip.Equals("127.0.0.1")) && (!username.Equals("newdeveloper")))
        {
            Debug.Log("co called");
            StartCoroutine(DiscoverLights(convertLightData));
        }
    }

    //public IEnumerator DiscoverLights()
    //{
    //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create();
    //    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

    //    UnityWebRequest lights_json = UnityWebRequest.Get("http://" + bridgeip + "/api/" + username + "/lights");
    //    yield return lights_json.Send();

    //    Debug.Log("http" + bridgeip + portNumber + "/api/" + username + "/lights");

    //    //System.IO.Stream stream = response.GetResponseStream();
    //   // System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);

    //    var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);
    //    foreach (string key in lights.Keys)
    //    {
    //        var light = (Dictionary<string, object>)lights[key];

    //        foreach (HueLamp hueLamp in GetComponentsInChildren<HueLamp>())
    //        {
    //            if (hueLamp.devicePath.Equals(key)) goto Found;
    //        }

    //        if (light["type"].Equals("Extended color light"))
    //        {

    //            GameObject gameObject = new GameObject();
    //            gameObject.name = (string)light["name"];
    //            gameObject.transform.parent = transform;
    //            gameObject.AddComponent<HueLamp>();
    //            HueLamp lamp = gameObject.GetComponent<HueLamp>();
    //            lamp.devicePath = key;

    //        }

    //    Found:
    //        ;
    //    }
    //}

    public IEnumerator DiscoverLights(Action action)
    {
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create();
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        lights_json = UnityWebRequest.Get("http://" + bridgeip + "/api/" + username + "/lights");
        yield return lights_json.Send();
        action();

        //Debug.Log("http" + bridgeip + portNumber + "/api/" + username + "/lights");

        //System.IO.Stream stream = response.GetResponseStream();
       // System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);

        //var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);
        //foreach (string key in lights.Keys)
        //{
        //    var light = (Dictionary<string, object>)lights[key];
        //    var state = (Dictionary<string, dynamic>)light["state"];

        //    SmartLightState smartLightState = new SmartLightState(
        //        Convert.ToBoolean(state["on"]),
        //        Convert.ToInt32(state["bri"]),
        //        Convert.ToInt32(state["hue"]),
        //        Convert.ToInt32(state["sat"]),
        //        Convert.ToString(state["effect"]),
        //        Convert.ToString(state["alert"])
        //        );

        //    smartLights.Add(new SmartLight(light["name"].ToString(), light["modelid"].ToString(), smartLightState));
        //    Debug.Log("inside co now");
        //    yield return smartLights;

        //    //foreach (HueLamp hueLamp in GetComponentsInChildren<HueLamp>())
        //    //{

        //    //    if (hueLamp.devicePath.Equals(key)) goto Found;
        //    //}

        //    //if (light["type"].Equals("Extended color light"))
        //    //{

        //    //    GameObject gameObject = new GameObject();
        //    //    gameObject.name = (string)light["name"];
        //    //    gameObject.transform.parent = transform;
        //    //    gameObject.AddComponent<HueLamp>();
        //    //    HueLamp lamp = gameObject.GetComponent<HueLamp>();
        //    //    lamp.devicePath = key;

        //    //}

        //    Found:
        //    ;

        //}
    }

    private List<SmartLight> getAllLights()
    {
        Debug.Log("getAll called");
        return smartLights;
    }

    public List<SmartLight> GetLights()
    {
        Debug.Log("should be last");
        if (smartLights.Count < 0)
        {
            Debug.Log("inside if");
            return null;
        }
        Debug.Log("outside if");
        return smartLights;
    }

    void convertLightData()
    {
        var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);
        foreach (string key in lights.Keys)
        {
            var light = (Dictionary<string, object>)lights[key];
            var state = (Dictionary<string, dynamic>)light["state"];

            SmartLightState smartLightState = new SmartLightState(
                Convert.ToBoolean(state["on"]),
                Convert.ToInt32(state["bri"]),
                Convert.ToInt32(state["hue"]),
                Convert.ToInt32(state["sat"]),
                Convert.ToString(state["effect"]),
                Convert.ToString(state["alert"])
                );
            //Debug.Log(state["bri"]);

            smartLights.Add(new SmartLight(light["name"].ToString(), light["modelid"].ToString(), smartLightState));
            //Debug.Log("should be first");
        }
        SendMessage("createLights", smartLights);
    }
}
