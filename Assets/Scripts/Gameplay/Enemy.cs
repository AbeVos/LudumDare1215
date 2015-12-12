using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    private enum State
    {
        /// <summary>Initial state.</summary>
        Spawn,
        /// <summary>Idle state, looking for target.</summary>
        Idle,
        /// <summary>Target found, getting ready for attack.</summary>
        Alarmed,
        /// <summary>Attacking.</summary>
        Attack,
        /// <summary>Dying and despawning state.</summary>
        Death
    }

    private State currentState;

    void Awake ()
    {
        SetState(State.Spawn);
    }

    void Update ()
    {

    }

    void OnCollisionEnter2D (Collision2D collision)
    {
    }

    private void SetState (State newState)
    {
        State prevState = currentState;
        currentState = newState;
    }
}
