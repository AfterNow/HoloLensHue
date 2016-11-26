using UnityEngine;

[System.Serializable]
public class Notification
{
    public Notification(string type, string message, bool sendToConsole, bool requiresAction)
    {
        this.type = type;
        this.message = message;
        this.sendToConsole = sendToConsole;
        this.requiresAction = requiresAction;    
    }

    //Mutator Functions
    public Notification(string type, string message, bool sendtoConsole)
    {
        this.type = type;
        this.message = message;
        this.sendToConsole = sendtoConsole;
        this.requiresAction = false;
    }

    public Notification(string type, string message)
    {
        this.type = type;
        this.message = message;
        this.sendToConsole = true;
        this.requiresAction = false;
    }

    //Accessor Functions
    public string Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

    public string Message
    {
        get
        {
            return message;
        }
        set
        {
            message = value;
        }
    }

    public bool SendToConsole
    {
        get
        {
            return sendToConsole;
        }
        set
        {
            sendToConsole = value;
        }
    }

    public bool RequiresAction
    {
        get
        {
            return requiresAction;
        }
        set
        {
            requiresAction = value;
        }
    }

    private string type;
    private string message;
    private bool sendToConsole;
    private bool requiresAction;
}
