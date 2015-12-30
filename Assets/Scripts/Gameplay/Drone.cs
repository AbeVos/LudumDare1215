using UnityEngine;
using System.Collections;

public class Drone : Enemy
{
    [SerializeField]
    private int explosionDamage = 5;

    private Transform body;
    private float prevX = 0;

    private Transform[] rotors;

    protected override void Start ()
    {
        base.Start();

        body = transform.GetChild(0);
        rotors = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            rotors[i] = transform.GetChild(0).GetChild(0).GetChild(i);
        }
    }

    protected override void Update ()
    {
        base.Update();

        if (State.Current == State.GlobalState.Game)
        {
            if (currentState == EnemyState.Attack)
            {
                transform.position = Vector3.Lerp(transform.position, Dragon.dragon.transform.position, Time.deltaTime * 2f);

                //body.transform.eulerAngles = Vector3.Lerp(body.transform.eulerAngles, ((prevX - transform.position.x < 0) ? 25 : 335) * Vector3.forward + 180 * Vector3.up, 0.1f);
                body.transform.eulerAngles = Vector3.Lerp(body.transform.eulerAngles, new Vector3(0, ((prevX - transform.position.x < 0) ? -1 : 1) * 60 + 180f, 0), 0.1f);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            rotors[i].eulerAngles += Vector3.up * 600 *Time.deltaTime;
        }

        prevX = transform.position.x;
    }

    override protected void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (currentState != EnemyState.Death)
        {
            if (collider.gameObject.layer == 8)
            {
                Dragon.dragon.Hit(explosionDamage);
                
                SetState(EnemyState.Death);

                //Debug.Break();
            }
            //else
            //{
            //    Debug.Log("Ouch");
            //    SetState(EnemyState.Death);
            //}
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
