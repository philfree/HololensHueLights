using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using MiniJSON;

public class HueBridgeManagerV2 : MonoBehaviour
{

    [Tooltip("IP address of the hue bridge: https://www.meethue.com/api/nupnp")]
    public string bridgeip = "127.0.0.1";
    public int portNumber = 8000;
    [Tooltip("Developer username")]
    public string username = "newdeveloper";

    UnityWebRequest lights_json;
    UnityWebRequest bridge_json;

    public List<SmartLightV2> smartLights = new List<SmartLightV2>();
    MockSmartLights mockLights;

    void Start()
    {
        // MOCK smart lights for testing
        //mockLights = new MockSmartLights();
        //smartLights = mockLights.getLights();
        //convertLightData();
        if ((!bridgeip.Equals("127.0.0.1")) && (!username.Equals("newdeveloper")))
        {
            Debug.Log("co called");
            StartCoroutine(DiscoverLights(convertLightData));
        }
        else
        {
            Debug.Log("Please enter your bridge IP and username");
        }
    }

    // pass in GetIP or GetSetIP as nextAction
    public IEnumerator DiscoverBridge(Action nextAction)
    {
        bridge_json = UnityWebRequest.Get("https://www.meethue.com/api/nupnp");

        yield return bridge_json.Send();
        nextAction();
    }

    public void GetBridgeIP()
    {
        StartCoroutine(DiscoverBridge(GetIP));
    }

    void GetIP()
    {
        var ip = ParseIP();
        Debug.Log("Bridge IP found: " + ip);
    }

    void GetSetIP()
    {
        bridgeip = ParseIP();
        StartCoroutine(DiscoverLights(convertLightData));
        Debug.Log("Bridge IP has been set: " + bridgeip);
    }

    string ParseIP()
    {
        var bridge = bridge_json.downloadHandler.text;

        // removes uneeded array brackets. *Warning*, will not work if more than one bridge is found on network.
        //TODO parse json response into array of strings
        bridge = bridge.Substring(1, bridge.Length - 2);
        HueBridgeV2 bridgeData = JsonUtility.FromJson<HueBridgeV2>(bridge);
        return bridgeData.internalipaddress;
    }

    public IEnumerator DiscoverLights(Action nextAction)
    {
        lights_json = UnityWebRequest.Get("http://" + bridgeip + "/api/" + username + "/lights");
        Debug.Log("Hue Response Errors: " + lights_json.error);
        yield return lights_json.Send();

        nextAction();

        Debug.Log("http" + bridgeip + portNumber + "/api/" + username + "/lights");
    }

    void convertLightData()
    {
        var lights = (Dictionary<string, object>)Json.Deserialize(lights_json.downloadHandler.text);
        foreach (string key in lights.Keys)
        {
            // init state types
            bool on;
            int id, bri, hue, sat;
            string effect, alert;

            //Debug.Log("made it to the foreach loop "+ key);

            var light = (Dictionary<string, object>)lights[key];
            var state = (Dictionary<string, dynamic>)light["state"];

            // converting needs to be done prior to instantiating new SmartLightState
            on = Convert.ToBoolean(state["on"]);
            bri = Convert.ToInt32(state["bri"]);
            hue = Convert.ToInt32(state["hue"]);
            sat = Convert.ToInt32(state["sat"]);
            effect = Convert.ToString(state["effect"]);
            alert = Convert.ToString(state["alert"]);

            id = Convert.ToInt32(key);

            var smartLightState = new State(on, bri, hue, sat, effect, alert);
            smartLights.Add(new SmartLightV2(id, light["name"].ToString(), light["modelid"].ToString(), smartLightState));
        }
        SendMessage("createLights", smartLights);

        // Wait till response from API to register voice control
        VoiceService voiceService = gameObject.GetComponent<VoiceService>();
        voiceService.RegisterPhrases();
    }

    public void GetHueBridgeIP()
    {
        StartCoroutine(DiscoverBridge(GetIP));
    }

    public void SetHueBridgeIP()
    {
        StartCoroutine(DiscoverBridge(GetSetIP));
    }

    public void TestPut()
    {

        State testState = smartLights[0].getState();
        testState.isOn(false);
        string request = "http://" + bridgeip + "/api/" + username + "/lights/1/state";
        string json = JsonUtility.ToJson(testState);
        JsonUtility.FromJson<SmartLightState>(json);

        UnityWebRequest setLight = UnityWebRequest.Put(request, json);
        Debug.Log("Send triggered to " + request);
        setLight.Send();

    }
}
