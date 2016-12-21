using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

/// <summary>
/// Manages the current state of the application.
/// </summary>
public class StateManager : Singleton<StateManager>
{
    public delegate void OnConfiguration();
    public static event OnConfiguration onConfiguration;

    public delegate void OnReady();
    public static event OnReady onReady;

    public delegate void OnSetup();
    public static event OnSetup onSetup;

    [Tooltip("Current state of the application")]
    public string appState;

    public enum HueAppState
    {
        // Overall states
        Starting,
        Ready,
        Configuring,
        // Walks through setup menu with user
        SetupMode,
        // IoT Devices values
        ConnectedDevices_Initializing,
        ConnectedDevices_Initialized,
        ConnectedDevices_Failed,
        // Interface States
        LightUI_Active
    }

    HueAppState currentState = HueAppState.Starting;

    public HueAppState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
            appState = CurrentStateName;
            OnStateChanged(currentState);
        }
    }

    public string CurrentStateName
    {
        get
        {
            return currentState.ToString();
        }
    }

    public bool Configuring
    {
        get
        {
            return currentState == HueAppState.Configuring;
        }
    }

    public bool Starting
    {
        get
        {
            return currentState == HueAppState.Starting;
        }
    }

    public bool ConnectedDevices_Initialized
    {
        get
        {
            return currentState == HueAppState.ConnectedDevices_Initialized;
        }
    }

    public bool SearchingForDevices
    {
        get
        {
            return currentState == HueAppState.ConnectedDevices_Initialized;
        }
    }

    public bool DevicesFound
    {
        get
        {
            return currentState == HueAppState.Ready;
        }
    }

    public bool SetupMode
    {
        get
        {
            return currentState == HueAppState.SetupMode;
        }
    }

    void Start()
    {
        appState = CurrentStateName;
    }

    private void OnStateChanged(HueAppState state)
    {
        if (state == HueAppState.Configuring || SetupMode)
        {
            if (onConfiguration != null)
            {
                onConfiguration();
            }

            if (SetupMode)
            {
                if (onSetup != null)
                {
                    onSetup();
                }
            }
        }

        if (state == HueAppState.Ready)
        {
            if (onReady != null)
            {
                onReady();
            }
        }
    }
    
}
