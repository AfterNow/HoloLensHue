using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class MenuStateManager : Singleton<MenuStateManager> {

    //public delegate void OnLinkButtonDisplayed(Menu menu);
    //public static event OnLinkButtonDisplayed onLinkButtonDisplayed;

    public delegate void OnMenuChanged(Menu menu);
    public static event OnMenuChanged onMenuChanged;

    [Tooltip("Current state of the menu")]
    public string menuState;

    public enum MenuState
    {
        // Overall states
        Hidden,
        MainMenu,
        LinkButton,
        LinkSuccess,
        Identify,
        TapDragAlign,
        SetupFinished,
        TT_Interactions,
        TT_Voice,
        TT_Gesture,
        TT_Hotspot
        // IoT Devices values
        //ConnectedDevices_Initializing,
    }

    private MenuState currentState = MenuState.Hidden;

    public MenuState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
            Menu menu = menuStateToMenu(currentState);

            OnMenuStateChanged(menu);

            menuState = MenuStateName;
        }
    }

    public string MenuStateName
    {
        get
        {
            return currentState.ToString();
        }
    }

    public bool Hidden
    {
        get
        {
            return currentState == MenuState.Hidden;
        }
    }

    //private Menu LinkButton
    //{
    //    get
    //    {
    //        return new Menu("PressLink", 400, 240, false, 30f);
    //    }
    //}

    public bool MainMenu
    {
        get
        {
            return currentState == MenuState.MainMenu;
        }
    }

    public bool LinkButton
    {
        get
        {
            return currentState == MenuState.LinkButton;
        }
    }

    void Start()
    {
        menuState = MenuStateName;
    }

    private void OnMenuStateChanged(Menu newMenu)
    {
        //if (onLinkButtonDisplayed != null)
        //{
        //    onLinkButtonDisplayed(newMenu);
        //}

        if (onMenuChanged != null)
        {
            onMenuChanged(newMenu);
        }

        //if (state == MenuState.Ready)
        //{
        //    if (onReady != null)
        //    {
        //        onReady();
        //    }
        //}
    }

    private Menu menuStateToMenu(MenuState state)
    {
        if (state == MenuState.Hidden)
        {
            return new Menu("HideMenu");
        }
        else if (state == MenuState.MainMenu)
        {
            return new Menu("MainMenu", 0f, true);
        }
        else if (state == MenuState.LinkButton)
        {
            return new Menu("PressLink", 400, 240, false, 30f);
        }
        else if (state == MenuState.LinkSuccess)
        {
            return new Menu("LinkSuccess", 400, 100, false, 30f, true, false, -90f);
        }
        else if (state == MenuState.Identify)
        {
            return new Menu("IdentifyLight", 400, 280, false, 30f, true, true, -180f);
        }
        else if (state == MenuState.TapDragAlign)
        {
            return new Menu("TapDragAlign", 400, 240, false, 30f, false, true, true, -160f);
        }
        else if (state == MenuState.SetupFinished)
        {
            return new Menu("SetupFinished", 200, 120, false, 30f);
        }
        else if (state == MenuState.TT_Interactions)
        {
            return new Menu("TT_Interactions", 400, 250, false, 30f, true, false, false, -165f);
        }
        else if (state == MenuState.TT_Voice)
        {
            return new Menu("TT_Voice", 400, 298, false, 30f, true, true, false, -192f);
        }
        else if (state == MenuState.TT_Gesture)
        {
            return new Menu("TT_Gesture", 400, 190, false, 30f, true, true, false, -132f);
        }
        else if (state == MenuState.TT_Hotspot)
        {
            return new Menu("TT_Hotspot", 400, 190, false, 30f, false, true, false, true, -132f);
        }

        return null;
    }
}
