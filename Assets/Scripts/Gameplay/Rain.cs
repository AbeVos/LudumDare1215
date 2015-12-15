using UnityEngine;
using System.Collections;

public class Rain : MonoBehaviour
{
    private Material mat;
    private float t;

    void Awake ()
    {
        mat = GetComponent<Renderer>().material;
        t = 0;
    }

    void Update ()
    {
        mat.mainTextureOffset = new Vector2(Time.time * 2.5f, Time.time * 2.5f);
    }
}
