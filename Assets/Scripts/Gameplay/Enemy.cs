using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class Enemy : MonoBehaviour, GameActor
{
    protected enum EnemyState
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

    protected EnemyState currentState;
    protected float stateTimer = 0f;

    //////////////////////////
    //  Built-in Functions  //
    //////////////////////////

    void OnEnable ()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;
    }

    void Start ()
    {
        SetState(EnemyState.Spawn);
    }

    protected virtual void Update ()
    {
        if (State.Current == State.GlobalState.Game)
        {
            if (currentState == EnemyState.Idle && Vector3.Distance(Dragon.dragon.transform.position, transform.position) <= detectionDistance)
            {
                SetState(EnemyState.Alarmed);
            }

            stateTimer += Time.deltaTime;
        }
    }

    void OnDisable ()
    {
        State.OnGlobalStateChanged -= State_OnGlobalStateChanged;
    }

    //////////////////////////
    //  Delegate Functions  //
    //////////////////////////

    protected virtual void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
    }

    ////////////////////////
    //  Public Functions  //
    ////////////////////////

    public void Hit(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            SetState(EnemyState.Death);
        }
    }

    /////////////////////////
    //  Private Functions  //
    /////////////////////////

    protected virtual void SetState (EnemyState newState)
    {
        stateTimer = 0;

        EnemyState prevState = currentState;
        currentState = newState;

        if (newState == EnemyState.Spawn)
        {
            SetState(EnemyState.Idle);
        }
        else if (newState == EnemyState.Alarmed)
        {
            Debug.Log("Wablief");
            transform.DOScale(1.1f, 0.1f).OnComplete( () =>
            {
                transform.DOScale(1f, 0.1f);
                SetState(EnemyState.Attack);
            } );
        }
        else if (newState == EnemyState.Death)
        {
            Debug.Log(name + " was killed.");
            transform.DOScale(1.2f, 0.35f).OnComplete( () =>
            {
                CameraBehaviour.ScreenShake(0.5f, 0.5f);
                Destroy(gameObject);
            } );
        }
    }
}
