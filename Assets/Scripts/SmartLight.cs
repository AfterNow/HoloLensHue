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
        state = new State();
    }

    public SmartLight(int id, string name, string modelid, State state)
    {
        this.id = id;
        this.name = name;
        this.modelid = modelid;
        this.state = state;
    }

    //Accessor Functions
    public int ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public string Modelid
    {
        get
        {
            return modelid;
        }
        set
        {
            modelid = value;
        }
    }

    public State State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
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
        //EventManager.TriggerEvent("test");
    }

    //Accessor Functions
    public bool On
    {
        get
        {
            return on;
        }
        set
        {
            on = value;
        }
    }

    public int Bri
    {
        get
        {
            return bri;
        }
        set
        {
            bri = value;
        }
    }

    public int Hue
    {
        get
        {
            return hue;
        }
        set
        {
            hue = value;
        }
    }

    public int Sat
    {
        get
        {
            return sat;
        }
        set
        {
            sat = value;
        }
    }

    public string Alert
    {
        get
        {
            return alert;
        }
        set
        {
            alert = value;
        }
    }

    public string Effect
    {
        get
        {
            return effect;
        }
        set
        {
            effect = value;
        }
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

