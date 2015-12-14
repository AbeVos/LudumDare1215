using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static float introTime = 5f;

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
            //State.SetState(State.GlobalState.Initialize);
        }
        else if (newGlobalState == State.GlobalState.Initialize)
        {
            StartCoroutine(SetStateAfterDelay(State.GlobalState.Game, introTime));
            //State.SetState(State.GlobalState.Game);
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

    public static void StartGame ()
    {
        State.SetState(State.GlobalState.Initialize);
    }

    public static void LevelUp ()
    {
        State.SetState(State.GlobalState.Pause);
    }

    public static void Continue ()
    {
        State.SetState(State.GlobalState.Game);
    }

    private IEnumerator SetStateAfterDelay (State.GlobalState newState, float delay)
    {
        yield return new WaitForSeconds(delay);

        State.SetState(newState);

        yield return null;
    }
}
