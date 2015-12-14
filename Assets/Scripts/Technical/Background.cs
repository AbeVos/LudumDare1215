using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    float moveSpeed;

    Transform[] buildings;

    void Start ()
    {
        moveSpeed = 0.4f * StageManager.GetDifficulty();

        buildings = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            buildings[i] = transform.GetChild(i);
        }
    }

    void Update ()
    {
        if (State.Current != State.GlobalState.Pause)
        {
            for (int i = 0; i < buildings.Length; i++)
            {
                buildings[i].position += moveSpeed * Vector3.left;

                if (buildings[i].position.x <= -200f)
                {
                    buildings[i].position += 400f * Vector3.right;
                }
            }
        }
    }
}
