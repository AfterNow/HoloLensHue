using UnityEngine;

[System.Serializable]
public class Menu
{
    public Menu(string name, int width, int height, bool requiresAction, float expiration)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.requiresAction = requiresAction;
        this.expiration = expiration;
    }

    //Accessor Functions
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

    public int Width
    {
        get
        {
            return width;
        }
        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
        set
        {
            height = value;
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

    public float Expiration
    {
        get
        {
            return expiration;
        }
        set
        {
            expiration = value;
        }
    }

    private string name;
    private int width;
    private int height;
    private bool requiresAction;
    private float expiration;
}
