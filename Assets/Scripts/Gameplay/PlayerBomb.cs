using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerBomb : MonoBehaviour
{
    private float targetSize = 40f;

    private bool exploding = false;
    private float explodingTime = 0f;

    private Plane[] cameraPlanes;

    void Awake ()
    {
        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    }

    void Start ()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * 10, ForceMode2D.Impulse);
    }

    void Update ()
    {
        if (State.Current == State.GlobalState.Game)
        {
            if (exploding)
            {
                transform.localScale = (explodingTime + 1) * Vector3.one;

                if (explodingTime >= targetSize)
                {
                    //transform.DOScale(0, 0.5f).OnComplete(() => { Destroy(gameObject); });
                    Destroy(gameObject);
                }

                explodingTime += 0.45f;
            }

            if (!GeometryUtility.TestPlanesAABB(cameraPlanes, GetComponent<Collider2D>().bounds))
            {
                Explode();
            }
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (State.Current == State.GlobalState.Game)
        {
            if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10)
            {
                Explode();
            }
        }
    }

    private void Explode ()
    {
        exploding = true;

        transform.parent = StageManager.Stage.transform;

        GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
