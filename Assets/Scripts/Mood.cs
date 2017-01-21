using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mood
{
    public Mood(string mainHue, string secondaryHue, string accentHue, string accentHueAlt)
    {
        this.mainHue = mainHue;
        this.secondaryHue = secondaryHue;
        this.accentHue = accentHue;
        this.accentHueAlt = accentHueAlt;
    }

    //Accessor Functions
    public string MainHue
    {
        get
        {
            return mainHue;
        }
        set
        {
            mainHue = value;
        }
    }

    public string SecondaryHue
    {
        get
        {
            return secondaryHue;
        }
        set
        {
            secondaryHue = value;
        }
    }

    public string AccentHue
    {
        get
        {
            return accentHue;
        }
        set
        {
            accentHue = value;
        }
    }

    public string AccentHueAlt
    {
        get
        {
            return accentHueAlt;
        }
        set
        {
            accentHueAlt = value;
        }
    }

    // Mutator Functions
    public Mood(string mainHue)
    {
        this.mainHue = mainHue;
        secondaryHue = mainHue;
        accentHue = mainHue;
        accentHueAlt = mainHue;
    }

    public Mood(string mainHue, string secondaryHue)
    {
        this.mainHue = mainHue;
        this.secondaryHue = secondaryHue;
        accentHue = mainHue;
        accentHueAlt = secondaryHue;
    }

    public Mood(string mainHue, string secondaryHue, string accentHue)
    {
        this.mainHue = mainHue;
        this.secondaryHue = secondaryHue;
        this.accentHue = accentHue;
        accentHueAlt = mainHue;
    }

    private string mainHue;
    private string secondaryHue;
    private string accentHue;
    private string accentHueAlt;
}
