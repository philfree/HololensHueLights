// OLDER CLASS VERSION. May need if other scene is broken
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartLight
{
    public SmartLight()
    {
        id = 0;
        name = "";
        modelid = "";
        state = null;
    }

    public SmartLight(int id, string name, string modelid, SmartLightState state)
    {
        this.id = id;
        this.name = name;
        this.modelid = modelid;
        this.state = state;
    }

    //Accessor Functions
    public int getID()
    {
        return id;
    }

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

    private int id;
    private string name;
    private string modelid;
    private SmartLightState state;
}
