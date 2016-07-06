using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartLight
{
    public SmartLight()
    {
        name = "";
        modelid = "";
        state = null;
    }

    public SmartLight(string name, string modelid, SmartLightState state)
    {
        this.name = name;
        this.modelid = modelid;
        this.state = state;
    }

    //Accessor Functions
    public string getName()
    {
        return name;
    }
    //public string getName { get { return name; } }

    public string getModelid()
    {
        return modelid;
    }

    public SmartLightState getState()
    {
        return state;
    }

    private string name;
    private string modelid;
    private SmartLightState state;
}
