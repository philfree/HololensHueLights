using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SmartLightV2
{
    public SmartLightV2()
    {
        id = 0;
        name = "";
        modelid = "";
        state = new State();
    }

    public SmartLightV2(int id, string name, string modelid, State state)
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

    public State getState()
    {
        return state;
    }

    private int id;
    private string name;
    private string modelid;
    private State state;
}

public class State
{

    public State()
    {
        on = false;
        bri = 254;
        hue = 2762;
        sat = 254;
        alert = "none";
        effect = "none";
    }

    public State(bool on, int bri, int hue, int sat, string alert, string effect)
    {
        this.on = on;
        this.bri = bri;
        this.hue = hue;
        this.sat = sat;
        this.alert = alert;
        this.effect = effect;
    }

    //Accessor Functions
    public bool isOn()
    {
        return on;
    }

    public int getBri()
    {
        return bri;
    }

    public int getHue()
    {
        return hue;
    }

    public int getSat()
    {
        return sat;
    }

    public string getAlert()
    {
        return alert;
    }

    public string getEffect()
    {
        return effect;
    }

    //Mutator Functions
    public void isOn(bool isOn)
    {
        on = isOn;
    }

    public void setBri(int setBri)
    {
        bri = setBri;
    }

    public void setHue(int setHue)
    {
        hue = setHue;
    }

    public void setSat(int setSat)
    {
        sat = setSat;
    }

    public void setAlert(string setAlert)
    {
        alert = setAlert;
    }

    public void setEffect(string setEffect)
    {
        effect = setEffect;
    }

    public bool on;
    public int bri;
    public int hue;
    public int sat;
    public string alert;
    public string effect;
}
