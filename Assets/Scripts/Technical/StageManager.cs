using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [Space]
    [Header("Enemy Prefabs")]
    [SerializeField]
    private GameObject dronePrefab;
    [SerializeField]
    private GameObject turretPrefab;

    [Space]
    [Header("Stage")]
    [SerializeField]
    private GameObject buildingPrefab;
    [SerializeField]
    private float levelSpeed = 0.1f;
    [SerializeField]
    private float buildingSpawnInterval = 10f;

    private float timer = 0f;

    private List<GameObject> buildings;

    //////////////////////////
    //  Built-in Functions  //
    //////////////////////////

    #region Built-in Functions

    void Awake ()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;

        buildings = new List<GameObject>();
    }

    void Update ()
    {
        if (State.Current == State.GlobalState.Game)
        {
            //  Move level to the left constantly.
            transform.position -= Vector3.right * levelSpeed;

            if (timer >= buildingSpawnInterval)
            {
                SpawnBuilding((Random.Range(0f, 1f) > 0.5f) ? true : false);
                Invoke("SpawnDrone", buildingSpawnInterval / 2);

                timer = 0;
            }

            timer += Time.deltaTime;
        }

        foreach (GameObject building in buildings)
        {
            if (building.transform.position.x < -20)
            {
                buildings.Remove(building);
                Destroy(building);
                break;
            }
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        Debug.Log("Collision " + collision.gameObject.name);

        if (collision.gameObject.layer == 12)
        {
            ObjectPool.RemoveEnemyBullet(collision.transform);
        }
        else if (collision.gameObject.layer == 13)
        {
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

    /////////////////////////
    //  Private Functions  //
    /////////////////////////

    private void SpawnBuilding (bool spawnTurret)
    {
        GameObject building = Instantiate(buildingPrefab, new Vector3(20, Random.Range(-5, -15), 0), Quaternion.identity) as GameObject;

        building.transform.parent = transform;

        if (spawnTurret)
        {
            GameObject turret = Instantiate(turretPrefab, building.transform.FindChild("TurretSpawn").position, Quaternion.identity) as GameObject;
            turret.transform.parent = building.transform;
        }

        buildings.Add(building);
    }

    private void SpawnDrone ()
    {
        GameObject drone = Instantiate(dronePrefab, new Vector3(20, Random.Range(4f, 16f), 0), Quaternion.identity) as GameObject;

        drone.transform.parent = transform;
    }
}
