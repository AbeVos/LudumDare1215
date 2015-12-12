using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    void Awake ()
    {
        State.OnGameStateChanged += State_OnGameStateChanged;

        //  Start the game.
        State.ChangeState(State.GlobalState.Start);
    }

    private void State_OnGameStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        Debug.Log("Started the game.");
    }
}
