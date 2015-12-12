using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //////////////////////////
    //  Built-in Functions  //
    //////////////////////////

    void Awake ()
    {
        State.OnGlobalStateChanged += State_OnGameStateChanged;

        //  Start the game.
        State.ChangeState(State.GlobalState.Start);
    }

    //////////////////////////
    //  Delegate Functions  //
    //////////////////////////

    private void State_OnGameStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        if (newGlobalState == State.GlobalState.Start)
        {
            Debug.Log("Started the game.");
            State.ChangeState(State.GlobalState.Initialize);
        }
        else if (newGlobalState == State.GlobalState.Initialize)
        {
            State.ChangeState(State.GlobalState.Game);
        }
    }
}
