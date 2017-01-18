using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VCSubPanel
{
    // Call Hue API on any change to any light/state update. Should pass ID as well. Rate limit should be handled on the Hue API service
    // Use public var accessible from inspector to allow changes to rate limit
    public VCSubPanel()
    {
        title = "untitled";
        subModule = new VCSubModule();
    }

    public VCSubPanel(string title, VCSubModule subModule)
    {
        this.title = title;
        this.subModule = subModule;
    }

    //Accessor Functions
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }

    public VCSubModule SubModule
    {
        get
        {
            return subModule;
        }
        set
        {
            subModule = value;
        }
    }

    public string title;
    public VCSubModule subModule;
}

[System.Serializable]
public class VCSubModule
{
    public VCSubModule()
    {
        keyPhrase = "no keyPhrase specified";
        action = "no action specified";
    }

    public string KeyPhrase
    {
        get
        {
            return keyPhrase;
        }
        set
        {
            keyPhrase = value;
        }
    }

    public string Action
    {
        get
        {
            return action;
        }
        set
        {
            action = value;
        }
    }

    public VCSubModule(string keyPhrase, string action)
    {
        this.keyPhrase = keyPhrase;
        this.action = action;
    }

    public string keyPhrase;
    public string action;
}