using UnityEngine;

[System.Serializable]
public class Menu
{
    public Menu(string name, int width, int height, bool requiresAction, float expiration, bool nextButton, bool backButton, bool saveButton, bool finishButton, float buttonPosY)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.requiresAction = requiresAction;
        this.expiration = expiration;
        this.nextButton = nextButton;
        this.backButton = backButton;
        this.saveButton = saveButton;
        this.finishButton = finishButton;
        this.buttonPosY = buttonPosY;
    }

    // Mutator Functions

    public Menu(string name, int width, int height, bool requiresAction, float expiration, bool nextButton, bool backButton, bool saveButton, float buttonPosY)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.requiresAction = requiresAction;
        this.expiration = expiration;
        this.nextButton = nextButton;
        this.backButton = backButton;
        this.saveButton = saveButton;
        this.buttonPosY = buttonPosY;
    }

    public Menu(string name, int width, int height, bool requiresAction, float expiration, bool nextButton, bool backButton, float buttonPosY)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.requiresAction = requiresAction;
        this.expiration = expiration;
        this.nextButton = nextButton;
        this.backButton = backButton;
        this.buttonPosY = buttonPosY;
    }

    public Menu(string name, int width, int height, bool requiresAction, float expiration)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.requiresAction = requiresAction;
        this.expiration = expiration;
        nextButton = false;
        backButton = false;
    }

    public Menu(string name, int width, int height, bool requiresAction, float expiration, bool nextButton, float buttonPosY)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.requiresAction = requiresAction;
        this.expiration = expiration;
        this.nextButton = nextButton;
        this.buttonPosY = buttonPosY;
        backButton = false;
    }

    public Menu(string name, float expiration, bool requiresAction)
    {
        this.name = name;
        this.expiration = expiration;
        this.requiresAction = requiresAction;
    }

    public Menu(string name)
    {
        this.name = name;
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

    public bool NextButton
    {
        get
        {
            return nextButton;
        }
        set
        {
            nextButton = value;
        }
    }

    public bool BackButton
    {
        get
        {
            return backButton;
        }
        set
        {
            backButton = value;
        }
    }

    public bool SaveButton
    {
        get
        {
            return saveButton;
        }
        set
        {
            saveButton = value;
        }
    }

    public bool FinishButton
    {
        get
        {
            return finishButton;
        }
        set
        {
            finishButton = value;
        }
    }

    public float ButtonPosY
    {
        get
        {
            return buttonPosY;
        }
        set
        {
            buttonPosY = value;
        }
    }

    private string name;
    private int width;
    private int height;
    private bool requiresAction;
    private float expiration;
    private bool nextButton;
    private bool backButton;
    private bool saveButton;
    private bool finishButton;
    private float buttonPosY;
}
