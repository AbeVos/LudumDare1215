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

    [SerializeField]
    private int healthPoints = 10;
    [SerializeField]
    private float detectionDistance = 10f;

    private State currentState;

    void Awake ()
    {
        SetState(State.Spawn);
    }

    protected virtual void Update ()
    {

    }

    void OnCollisionEnter2D (Collision2D collision)
    {
    }

    private void SetState (State newState)
    {
        State prevState = currentState;
        currentState = newState;

        if (newState == State.Death)
        {
            Debug.Log(name + " was killed.");
            Destroy(gameObject);
        }
    }

    private void Hit (int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            SetState(State.Death);
        }
    }
}
