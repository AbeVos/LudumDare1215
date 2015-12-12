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
        End
    }

    public delegate void OnGlobalStateChangedHandle(GlobalState prevGlobalState, GlobalState newGlobalState);
    public static event OnGlobalStateChangedHandle OnGlobalStateChanged;

    private static GlobalState globalState;

    public static GlobalState Current
    {
        get { return globalState; }
    }

    public static void ChangeState (GlobalState newState)
    {
        GlobalState prevState = globalState;
        globalState = newState;

        Debug.Log("Changed Global State from " + prevState.ToString() + " to " + newState.ToString());

        if (OnGlobalStateChanged != null) OnGlobalStateChanged(prevState, newState);
    }
}
