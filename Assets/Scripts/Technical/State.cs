using UnityEngine;
using System.Collections;

/// <summary>
/// Global Game Manager
/// </summary>
public class State : MonoBehaviour
{
    public enum GlobalState
    {
        /// <summary>Game Introduction.</summary>
        Start,
        /// <summary>Setting up stage (spawning enemies etc.).</summary>
        Initialize,
        /// <summary>Gameplay</summary>
        Game,
        /// <summary>(Optional) Game won.</summary>
        Win,
        /// <summary>Game lost.</summary>
        Lose,
        /// <summary>Game Over, move on to new scene.</summary>
        End,
        /// <summary>Paused State for upgrading.</summary>
        Pause,
    }

    public delegate void GlobalStateChangedDelegate(GlobalState prevGlobalState, GlobalState newGlobalState);
    public static event GlobalStateChangedDelegate OnGlobalStateChanged;

    private static GlobalState globalState;

    public static GlobalState Current
    {
        get { return globalState; }
    }

    public static void SetState (GlobalState newState)
    {
        GlobalState prevState = globalState;
        globalState = newState;

      //s  Debug.Log("Changed Global State from " + prevState.ToString() + " to " + newState.ToString());

        if (OnGlobalStateChanged != null) OnGlobalStateChanged(prevState, newState);
    }
}
