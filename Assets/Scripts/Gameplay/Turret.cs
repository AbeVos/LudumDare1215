using UnityEngine;
using System.Collections;

public class Turret : Enemy
{
    [Space]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField, Range(1, 10)]
    private int shotsPerRound = 3;

    private Transform cannon;
    private Transform bulletSpawn;

    private bool firing = false;
    private float firingTimer = 0f;

    override protected void Start ()
    {
        base.Start();

        cannon = transform.FindChild("Cannon");
        bulletSpawn = cannon.FindChild("BulletSpawn");
    }

    override protected void Update ()
    {
        base.Update();

        if (currentState == EnemyState.Attack)
        {
            Vector3 directionVector = Dragon.dragon.transform.position - transform.position;
            cannon.transform.rotation = Quaternion.Slerp(cannon.transform.rotation, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(directionVector.y, directionVector.x)), 0.1f);

            if (!firing && firingTimer >= 1f)
            {
                StartCoroutine(FiringCoroutine());
            }

            firingTimer += Time.deltaTime;
        }
    }

    private IEnumerator FiringCoroutine ()
    {
        firing = true;

        for (int i = 0; i < shotsPerRound; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.forward * 100f, ForceMode2D.Force);

            yield return new WaitForSeconds(0.3f);
        }

        firing = false;
        firingTimer = 0f;

        yield return null;
    }
}
