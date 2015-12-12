using UnityEngine;
using System.Collections;

public class Drone : Enemy
{
    protected override void Update ()
    {
        base.Update();

        Debug.Log("Updated Drone");
    }
}
