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

    [Tooltip("Current state of the application")]
    public string appState;

    public enum HueAppState
    {
        // Overall states
        Starting,
        Ready,
        Configuring,
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

    void Start()
    {
        appState = CurrentStateName;
    }

    private void OnStateChanged(HueAppState state)
    {
        if (state == HueAppState.Configuring)
        {
            if (onConfiguration != null)
            {
                onConfiguration();
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
