using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //////////////////////////
    //  Built-in Functions  //
    //////////////////////////

    void Awake ()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;
    }

    void Start ()
    {
        //  Start the game.
        State.SetState(State.GlobalState.Start);
    }

    //////////////////////////
    //  Delegate Functions  //
    //////////////////////////

    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        if (newGlobalState == State.GlobalState.Start)
        {
            Debug.Log("Started the game.");
            State.SetState(State.GlobalState.Initialize);
        }
        else if (newGlobalState == State.GlobalState.Initialize)
        {
            State.SetState(State.GlobalState.Game);
        }
        else if (newGlobalState == State.GlobalState.Lose)
        {
            Debug.Log("Game Lost!");
        }
    }

    public static void PlayerHit (int damage)
    {
        Debug.Log("Auw hoor!");
        CameraBehaviour.ScreenShake(1f, 1f, false);

        if (Dragon.Health <= 0)
        {
            State.SetState(State.GlobalState.Lose);
        }
    }
}
