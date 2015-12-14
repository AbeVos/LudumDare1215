using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField]
    private float targetSize = 5f;

    private bool exploding = false;
    private float explodingTime = 0f;

    private Plane[] cameraPlanes;

    void Awake ()
    {
        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    }

    void Update ()
    {
        if (State.Current == State.GlobalState.Game)
        {
            if (exploding)
            {
                Debug.Log("Exploding");

                transform.localScale = explodingTime * Vector3.one;

                if (explodingTime >= targetSize)
                {
                    transform.DOScale(0, 0.5f).OnComplete(() => { Destroy(gameObject); });
                }

                explodingTime += Time.deltaTime;
            }

            if (!GeometryUtility.TestPlanesAABB(cameraPlanes, GetComponent<Collider2D>().bounds))
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        Debug.Log("Collision1!");

        if (State.Current == State.GlobalState.Game)
        {
            Debug.Log("Collision2!" + collision.gameObject.layer);

            if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10)
            {
                exploding = true;

                Destroy(GetComponent<Rigidbody2D>());
            }
        }
    }
}
