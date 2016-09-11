using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

/// <summary>
/// Manages the current state of the application.
/// </summary>
public class StateManager : Singleton<StateManager>
{
    [Tooltip("Current state of the application")]
    public string appState;

    public enum HueAppState
    {
        // Overall states
        Start,
        Ready,
        Editing,
        // IoT Devices values
        ConnectedDevices_Initializing,
        ConnectedDevices_Initialized,
        ConnectedDevices_Failed
    }

    HueAppState currentState = HueAppState.Start;

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
        }
    }

    public string CurrentStateName
    {
        get
        {
            return currentState.ToString();
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
}
