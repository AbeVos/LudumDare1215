using UnityEngine;
using System.Collections;

public class Drone : Enemy
{
    [Space]
    [SerializeField]
    int explosionDamage = 5;

    protected override void Update ()
    {
        base.Update();

        if (State.Current == State.GlobalState.Game)
        {
            if (currentState == EnemyState.Attack)
            {
                transform.position = Vector3.Lerp(transform.position, Dragon.dragon.transform.position, Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log(collider);

        if (currentState != EnemyState.Death)
        {
            if (collider.gameObject.layer == 8)
            {
                Dragon.dragon.Hit(explosionDamage);

                SetState(EnemyState.Death);

                //Debug.Break();
            }
            else
            {
                Debug.Log("Ouch");
                SetState(EnemyState.Death);
            }
        }
    }

    protected override void SetState(EnemyState newState)
    {
        base.SetState(newState);

        if (newState == EnemyState.Attack)
        {
            transform.parent = null;
        }
    }
}
