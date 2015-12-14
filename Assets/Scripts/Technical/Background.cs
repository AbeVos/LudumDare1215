using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    float moveSpeed;

    Transform[] buildings;
    Material floor;

    void Start ()
    {
        moveSpeed = 0.4f * StageManager.GetDifficulty();

        buildings = new Transform[transform.childCount - 1];
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            buildings[i] = transform.GetChild(i);
        }

        floor = transform.GetChild(transform.childCount - 1).GetComponent<Renderer>().material;
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

            floor.mainTextureOffset += 0.004f * moveSpeed * Vector2.right;
        }
    }
}
