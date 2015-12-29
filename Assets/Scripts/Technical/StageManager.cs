using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [Space]
    [Header("Enemy Prefabs")]
    [SerializeField]
    public GameObject dronePrefab;
    [SerializeField]
    public GameObject turretPrefab;

    [Space]
    [Header("Stage")]
    public GameObject buildingPrefab;
    public float speedMultiplier = 0.1f;
    public float buildingSpawnInterval = 6f;

    public AnimationCurve difficultyCurve;//= new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(300, 1) });


    private static StageManager self;
    private static AnimationCurve staticDifficultyCurve;
    private static float _difficultyTime = 0;

    private float timer = 0f;
    private static float levelSpeed = 1f;
    private float buildingTimer = 0f;

    private List<GameObject> buildings;

    #region Properties
    public static StageManager Stage
    {
        get { return self; }
    }

    public static float DifficultyTimer
    {
        get { return _difficultyTime; }
        private set { _difficultyTime = value; }
    }

    public static float Speed
    {
        get { return levelSpeed; }
    }
    #endregion
    //////////////////////////
    //  Built-in Functions  //
    //////////////////////////

    #region Built-in Functions

    void Awake()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;
        self = this;
        staticDifficultyCurve = difficultyCurve;
        buildings = new List<GameObject>();
        levelSpeed = speedMultiplier * GetDifficulty();
        StartCoroutine(DifficultyClock(0));
    }

    void Update()
    {
        if (State.Current == State.GlobalState.Game)
        {
            levelSpeed = speedMultiplier * GetDifficulty();
            buildingTimer = buildingSpawnInterval * GetDifficulty();

            //  Move level to the left constantly.
            transform.position += Vector3.left * levelSpeed;

            if (timer >= buildingTimer)
            {
                SpawnBuilding((Random.Range(0f, 1f) < GetDifficulty() / 10f) ? true : false);
                timer = 0;
            }

         //   Invoke("SpawnDrone", buildingTimer / 2);

            timer += Time.deltaTime;

            if (DifficultyTimer >= 300)
            {
                State.SetState(State.GlobalState.Win);
            }
        }

        foreach (GameObject building in buildings)
        {
            if (building.transform.position.x < -40)
            {
                buildings.Remove(building);
                Destroy(building);
                break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision " + collision.gameObject.name);

        if (collision.gameObject.layer == 8)
        {
            AudioManager.PlayClip("dragonWall", true);
            Dragon.dragon.Hit(50);
        }
        else if (collision.gameObject.layer == 9 && collision.gameObject.GetComponent<Drone>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().Hit(10);
        }
        else if (collision.gameObject.layer == 12)
        {
            AudioManager.PlayClip("impactWall", true);
            ObjectPool.RemoveEnemyBullet(collision.transform);
        }
        else if (collision.gameObject.layer == 13 || collision.gameObject.GetComponent<PlayerBomb>() == null)
        {
            AudioManager.PlayClip("impactWall", true);
            ObjectPool.RemovePlayerBullet(collision.transform);
        }
    }
    #endregion

    //////////////////////////
    //  Delegate Functions  //
    //////////////////////////

    #region Delegate Functions

    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        if (newGlobalState == State.GlobalState.Game)
        {
            SpawnBuilding(false);
        }
    }

    #endregion

    /// <summary>Returns the difficulty multiplier for the current global time.</summary>
    public static float GetDifficulty()
    {
        return staticDifficultyCurve.Evaluate(_difficultyTime);
    }

    /////////////////////////
    //  Private Functions  //
    /////////////////////////

    private void SpawnBuilding(bool spawnTurret)
    {
        GameObject building = Instantiate(buildingPrefab, new Vector3(40, Random.Range(-5, -10), 0), Quaternion.identity) as GameObject;

        building.transform.parent = transform;

        if (spawnTurret)
        {
            GameObject turret = Instantiate(turretPrefab, building.transform.FindChild("TurretSpawn").position, Quaternion.identity) as GameObject;
            turret.transform.parent = building.transform;
        }

        buildings.Add(building);
    }

    private void SpawnDrone()
    {
        GameObject drone = Instantiate(dronePrefab, new Vector3(40, Random.Range(4f, 16f), 0), Quaternion.identity) as GameObject;

        drone.transform.parent = transform;
    }

    IEnumerator DifficultyClock(float time)
    {
        time++;
        DifficultyTimer = time;
        yield return new WaitForSeconds(1f);

        //   if (State.Current == State.GlobalState.Game)
        {
            StartCoroutine(DifficultyClock(time));
        }
    }
}
