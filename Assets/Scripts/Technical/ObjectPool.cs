using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    struct Bullet
    {
        public Transform transform;
        public float speed;
    }

    [SerializeField]
    private GameObject playerBulletPrefab;
    [SerializeField]
    private GameObject enemyBulletPrefab;

    private static GameObject staticPlayerBulletPrefab;
    private static List<Bullet> playerBulletsInUse;
    private static List<Bullet> playerBulletsAvailable;

    private static GameObject staticEnemyBulletPrefab;
    private static List<Bullet> enemyBulletsInUse;
    private static List<Bullet> enemyBulletsAvailable;

    void Awake ()
    {
        staticPlayerBulletPrefab = playerBulletPrefab;
        playerBulletsInUse = new List<Bullet>();
        playerBulletsAvailable = new List<Bullet>();

        staticEnemyBulletPrefab = enemyBulletPrefab;
        enemyBulletsInUse = new List<Bullet>();
        enemyBulletsAvailable = new List<Bullet>();
    }

    void Update()
    {
        foreach (Bullet bullet in playerBulletsInUse)
        {
            bullet.transform.position += bullet.transform.forward * bullet.speed;
        }

        foreach (Bullet bullet in enemyBulletsInUse)
        {
            bullet.transform.position += bullet.transform.right * bullet.speed;
        }
    }

    public static GameObject CreatePlayerBullet (Vector3 position, Quaternion rotation, float speed)
    {
        //  Check whether there are no bullets available.
        if (playerBulletsAvailable.Count == 0)
        {
            GameObject bulletObject = Instantiate(staticPlayerBulletPrefab, position, rotation) as GameObject;
            Bullet bullet;

            bullet.transform = bulletObject.transform;
            bullet.speed = speed;

            playerBulletsInUse.Add(bullet);

            return bulletObject;
        }
        else
        {
            Bullet bullet = playerBulletsAvailable[0];

            playerBulletsAvailable.Remove(bullet);
            playerBulletsInUse.Add(bullet);

            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.transform.gameObject.SetActive(true);

            return bullet.transform.gameObject;
        }
    }

    public static void RemovePlayerBullet (Transform transform)
    {
        foreach (Bullet bullet in playerBulletsInUse)
        {
            if (bullet.transform.Equals(transform))
            {
                bullet.transform.gameObject.SetActive(false);

                playerBulletsInUse.Remove(bullet);
                playerBulletsAvailable.Add(bullet);

                return;
            }
        }
    }

    public static GameObject CreateEnemyBullet(Vector3 position, Quaternion rotation, float speed)
    {
        //  Check whether there are no bullets available.
        if (enemyBulletsAvailable.Count == 0)
        {
            GameObject bulletObject = Instantiate(staticEnemyBulletPrefab, position, rotation) as GameObject;
            Bullet bullet;

            bullet.transform = bulletObject.transform;
            bullet.speed = speed;

            enemyBulletsInUse.Add(bullet);

            return bulletObject;
        }
        else
        {
            Bullet bullet = enemyBulletsAvailable[0];

            enemyBulletsAvailable.Remove(bullet);
            enemyBulletsInUse.Add(bullet);

            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.transform.gameObject.SetActive(true);

            return bullet.transform.gameObject;
        }
    }

    public static void RemoveEnemyBullet(Transform transform)
    {
        foreach (Bullet bullet in enemyBulletsInUse)
        {
            if (bullet.transform.Equals(transform))
            {
                bullet.transform.gameObject.SetActive(false);

                enemyBulletsInUse.Remove(bullet);
                enemyBulletsAvailable.Add(bullet);

                return;
            }
        }
    }
}
