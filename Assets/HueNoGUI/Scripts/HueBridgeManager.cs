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
    public List<SmartLight> smartLights = null;
    MockSmartLights mockLights;


    void Awake()
    {
        smartLights = new List<SmartLight>();
    }
    void Start () {
        // MOCK smart lights for testing
        //mockLights = new MockSmartLights();
        //smartLights = mockLights.getLights();
        //convertLightData();
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

    public IEnumerator DiscoverLights(Action nextAction)
    {
        lights_json = UnityWebRequest.Get("http://" + bridgeip + "/api/" + username + "/lights");
        Debug.Log(lights_json.error);
        yield return lights_json.Send();
        nextAction();

        Debug.Log("http" + bridgeip + portNumber + "/api/" + username + "/lights");

        //var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);
        //foreach (string key in lights.Keys)
        //{

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

    public List<SmartLight> GetLightCollection()
    {
        return smartLights;
    }

    void convertLightData()
    {
        var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);
        foreach (string key in lights.Keys)
        {
            // init state types
            bool on;
            int bri, hue, sat;
            string effect, alert;

            Debug.Log("made it to the foreach loop "+ key);

            var light = (Dictionary<string, object>)lights[key];
            var state = (Dictionary<string, dynamic>)light["state"];

            // converting needs to be done prior to instantiating new SmartLightState
            on = Convert.ToBoolean(state["on"]);
            bri = Convert.ToInt32(state["bri"]);
            hue = Convert.ToInt32(state["hue"]);
            sat = Convert.ToInt32(state["sat"]);
            effect = Convert.ToString(state["effect"]);
            alert = Convert.ToString(state["alert"]);

            SmartLightState smartLightState = new SmartLightState(on, bri, hue, sat, effect, alert);
            smartLights.Add(new SmartLight(light["name"].ToString(), light["modelid"].ToString(), smartLightState));
        }
        SendMessage("createLights", smartLights);
    }

    public void TestPut()
    {

        SmartLightState testState = smartLights[0].getState();
        testState.isOn(false);
        string request = "http://" + bridgeip + "/api/" + username + "/lights/1/state";
        string json = JsonUtility.ToJson(testState);
        JsonUtility.FromJson<SmartLightState>(json);

        UnityWebRequest setLight = UnityWebRequest.Put(request, json);
        Debug.Log("Send triggered to " + request);
        setLight.Send();

    }
}
