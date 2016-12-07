using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightUI
{
    public LightUI(int lightID, bool show, Color orbColor, int brightness, string name)
    {
        this.lightID = lightID;
        this.show = show;
        this.orbColor = orbColor;
        this.brightness = brightness;
        this.name = name;
    }

    //Accessor Functions
    public int LightID
    {
        get
        {
            return lightID;
        }
        set
        {
            lightID = value;
        }

    }

    public bool Show
    {
        get
        {
            return show;
        }
        set
        {
            show = value;
        }

    }

    public Color OrbColor
    {
        get
        {
            return orbColor;
        }
        set
        {
            orbColor = value;
        }
    }

    public int Brightness
    {
        get
        {
            return brightness;
        }
        set
        {
            brightness = value;
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

    // Mutator Functions
    public LightUI(int lightID, bool show)
    {
        this.lightID = lightID;
        this.show = show;
    }

    public LightUI(int lightID, Color orbColor)
    {
        this.lightID = lightID;
        this.orbColor = orbColor;
    }

    public LightUI(int lightID, bool show, Color orbColor)
    {
        this.lightID = lightID;
        this.show = show;
        this.orbColor = orbColor;
    }

    private int lightID;
    private bool show;
    private Color orbColor;
    private int brightness;
    private string name;
}
