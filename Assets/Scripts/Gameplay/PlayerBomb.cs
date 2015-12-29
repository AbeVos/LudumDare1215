using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerBomb : MonoBehaviour
{
    private readonly float ExplotionSpeed = 40f;
    private readonly float targetSize = 60f;

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
                transform.localScale = Vector3.one * (explodingTime + 1);
                CameraBehaviour.ScreenShake(0.5f, 1.5f, true);

                if (explodingTime >= targetSize)
                {
                    Destroy(gameObject);
                }

                explodingTime += Time.deltaTime* ExplotionSpeed;
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
        AudioManager.PlayClip("ChargeExplotion", true);
        transform.parent = StageManager.Stage.transform;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
