using UnityEngine;
using System.Collections;

public class Drone : Enemy
{
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != EnemyState.Death)
        {
            Debug.Log("Collision");

            if (collision.gameObject.layer == 8)
            {
                Dragon.dragon.Hit(explosionDamage);

                SetState(EnemyState.Death);
            }
        }
    }
}
