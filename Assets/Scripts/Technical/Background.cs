using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 0.2f;

    Transform[] buildings;

    void Start ()
    {
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
