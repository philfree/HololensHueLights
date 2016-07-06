using UnityEngine;
using System.Collections;

public class SmartLightState
{

    public SmartLightState()
    {
        on = false;
        bri = 254;
        hue = 2762;
        sat = 254;
        alert = "none";
        effect = "none";
}

    public SmartLightState(bool on, int bri, int hue, int sat, string alert, string effect)
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
        bri = setHue;
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