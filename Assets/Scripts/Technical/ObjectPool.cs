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

    struct Pickup
    {
        public Transform transform;
        public int value;
    }

    [SerializeField]
    private GameObject playerBulletPrefab;
    [SerializeField]
    private GameObject enemyBulletPrefab;

    [SerializeField]
    private GameObject xpPickupPrefab;

    private static GameObject staticPlayerBulletPrefab;
    private static List<Bullet> playerBulletsInUse;
    private static List<Bullet> playerBulletsAvailable;

    private static GameObject staticEnemyBulletPrefab;
    private static List<Bullet> enemyBulletsInUse;
    private static List<Bullet> enemyBulletsAvailable;

    private static GameObject staticXpPickupPrefab;
    private static List<Pickup> xpPickupsInUse;
    private static List<Pickup> xpPickupsAvailable;

    private List<Bullet> bulletsToRemove;
    private List<Pickup> pickupsToRemove;

    private Plane[] cameraPlanes;

    void Awake ()
    {
        staticPlayerBulletPrefab = playerBulletPrefab;
        playerBulletsInUse = new List<Bullet>();
        playerBulletsAvailable = new List<Bullet>();

        staticEnemyBulletPrefab = enemyBulletPrefab;
        enemyBulletsInUse = new List<Bullet>();
        enemyBulletsAvailable = new List<Bullet>();

        staticXpPickupPrefab = xpPickupPrefab;
        xpPickupsInUse = new List<Pickup>();
        xpPickupsAvailable = new List<Pickup>();

        bulletsToRemove = new List<Bullet>();
        pickupsToRemove = new List<Pickup>();

        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    }

    void OnEnable ()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;
    }

    void Update()
    {
        if (State.Current == State.GlobalState.Game)
        {
            #region Player Bullets
            foreach (Bullet bullet in playerBulletsInUse)
            {
                bullet.transform.position += bullet.transform.right * bullet.speed;

                if (!GeometryUtility.TestPlanesAABB(cameraPlanes, bullet.transform.GetComponent<Collider2D>().bounds))
                {
                    bulletsToRemove.Add(bullet);
                    //RemoveEnemyBullet(bullet);
                }
            }

            if (bulletsToRemove.Count > 0)
            {
                foreach (Bullet bullet in bulletsToRemove)
                {
                    RemovePlayerBullet(bullet);
                }

                bulletsToRemove.Clear();
            }
            #endregion

            #region Enemy Bullets
            foreach (Bullet bullet in enemyBulletsInUse)
            {
                //bullet.transform.position += bullet.transform.right * bullet.speed;

                if (!GeometryUtility.TestPlanesAABB(cameraPlanes, bullet.transform.GetComponent<Collider2D>().bounds))
                {
                    bulletsToRemove.Add(bullet);
                    //RemoveEnemyBullet(bullet);
                }
            }

            if (bulletsToRemove.Count > 0)
            {
                foreach (Bullet bullet in bulletsToRemove)
                {
                    RemoveEnemyBullet(bullet);
                }

                bulletsToRemove.Clear();
            }
            #endregion

            #region XP Pickups

            foreach (Pickup pickup in xpPickupsInUse)
            {
                //bullet.transform.position += bullet.transform.right * bullet.speed;

                if (!GeometryUtility.TestPlanesAABB(cameraPlanes, pickup.transform.GetComponent<Collider2D>().bounds))
                {
                    pickupsToRemove.Add(pickup);
                    //RemoveEnemyBullet(bullet);
                }
            }

            if (pickupsToRemove.Count > 0)
            {
                foreach (Pickup pickup in pickupsToRemove)
                {
                    RemoveXpPickup(pickup);
                }

                pickupsToRemove.Clear();
            }

            #endregion
        }
    }

    void OnDisable ()
    {
        State.OnGlobalStateChanged -= State_OnGlobalStateChanged;
    }

    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        //  Remove all bullets when the game is paused, because games are hard :(
        if (newGlobalState == State.GlobalState.Pause)
        {
            foreach (Bullet bullet in playerBulletsInUse)
            {
                bulletsToRemove.Add(bullet);
            }

            foreach (Bullet bullet in bulletsToRemove)
            {
                RemovePlayerBullet(bullet);
            }

            bulletsToRemove.Clear();

            foreach (Bullet bullet in enemyBulletsInUse)
            {
                bulletsToRemove.Add(bullet);
            }

            foreach (Bullet bullet in bulletsToRemove)
            {
                RemoveEnemyBullet(bullet);
            }

            bulletsToRemove.Clear();
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

    private static void RemovePlayerBullet(Bullet bullet)
    {
        bullet.transform.gameObject.SetActive(false);

        playerBulletsInUse.Remove(bullet);
        playerBulletsAvailable.Add(bullet);
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

            bulletObject.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right * speed);

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

            bullet.transform.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right * speed);

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

    private static void RemoveEnemyBullet(Bullet bullet)
    {
        bullet.transform.gameObject.SetActive(false);

        enemyBulletsInUse.Remove(bullet);
        enemyBulletsAvailable.Add(bullet);
    }

    public static GameObject CreateXpPickup (Vector3 position, int value)
    {
        //  Check whether there are no bullets available.
        if (xpPickupsAvailable.Count == 0)
        {
            GameObject pickupObject = Instantiate(staticPlayerBulletPrefab, position, Quaternion.identity) as GameObject;
            pickupObject.transform.parent = StageManager.Stage.transform;

            Pickup pickup;
            pickup.transform = pickupObject.transform;
            pickup.value = value;

            xpPickupsInUse.Add(pickup);

            return pickupObject;
        }
        else
        {
            Pickup pickup = xpPickupsAvailable[0];

            xpPickupsAvailable.Remove(pickup);
            xpPickupsInUse.Add(pickup);

            pickup.transform.position = position;
            pickup.transform.rotation = Quaternion.identity;
            pickup.transform.gameObject.SetActive(true);

            return pickup.transform.gameObject;
        }
    }

    public static void RemoveXpPickup (Transform transform)
    {
        foreach (Pickup pickup in xpPickupsInUse)
        {
            if (pickup.transform.Equals(transform))
            {
                pickup.transform.gameObject.SetActive(false);

                xpPickupsInUse.Remove(pickup);
                xpPickupsAvailable.Add(pickup);

                return;
            }
        }
    }

    private static void RemoveXpPickup (Pickup pickup)
    {
        pickup.transform.gameObject.SetActive(false);

        xpPickupsInUse.Remove(pickup);
        xpPickupsAvailable.Add(pickup);
    }
}
