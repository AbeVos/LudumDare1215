using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Dragon : MonoBehaviour, GameActor
{
    private enum WeaponState
    {
        /// <summary>Idle state, Initial state.</summary>
        Idle,
        /// <summary>Attack active, heat rising</summary>
        PrimaryActive,
        /// <summary>Charge is building. Weapon is off</summary>
        SecondaryActive,
        /// <summary>Charge is empty, attack is active </summary>
        SecondaryFired
    }

    #region Variables
    [SerializeField, Range(4, 10)]
    private float responseSpeed = 7.5f;

    [Space]
    [SerializeField]
    private GameObject bombPrefab;

    [Space]
    [Header("Weapon Speeds")]
    [SerializeField, Range(10, 40)]
    private static float chargeSpeed = 10;
    [SerializeField, Range(1, 40)]
    private static float heatUpSpeed = 10;
    [SerializeField, Range(1, 40)]
    private static float coolDownSpeed = 10;
    [SerializeField, Range(0.05f, 1f)]
    private static float FireRatePrimary = 0.2f;
    [SerializeField, Range(0.05f, 1f)]
    private static float BulletSpeed = 0.2f;

    private bool coroutineRunning = false;
    private bool burstRunning = false;
    /// <summary>Heat is at 100%, primary can only start again at 0% heat</summary>
    private static bool primaryOverheat;
    private WeaponState currentState;
    private AudioSource source;
    private Animator anim;

    private Transform primaryBarrel;
    private Transform secondaryBarrel;
    private Vector3 lastWorldPos;
    private float lastVelocity;

    private bool alarm = false;
    #endregion

    #region statics
    /// <summary>Static reference to self.</summary>
    private static Dragon self;
    private static int _health, _exp, _primaryDamage, _secondaryDamage, _upgradeRank = 0;
    private static float _charge, _heat = 0f;
    #endregion

    #region Properties
    public static Dragon dragon
    {
        get { return self; }
    }
    public static int Health
    {
        get { return _health; }
        set { _health = value; }
    }
    public static int Exp
    {
        get { return _exp; }
        set { _exp = value; }
    }
    public static float Charge
    {
        get { return _charge; }
        private set
        {
            if (value <= 100 && value >= 0) { _charge = value; }
        }
    }
    public static float Heat
    {
        get { return _heat; }
        private set
        {
            if (value <= 100 && value >= 0) { _heat = value; }
        }
    }
    public static bool Overheat
    {
        get { return primaryOverheat; }
        private set { primaryOverheat = value; }
    }
    public static int PrimaryDamage
    {
        get { return _primaryDamage; }
        set { if (value > 0) { _primaryDamage = value; } }
    }
    public static int SecondaryDamage
    {
        get { return _secondaryDamage; }
        set { if (value > 0) { _secondaryDamage = value; } }
    }
    public static int Rank
    {
        get { return _upgradeRank; }
        set { if (value > 0) { _upgradeRank = value; } }
    }
    #endregion

    #region Built-in
    void OnEnable()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;

        InputManager.OnPrimaryButtonDown += InputManager_OnPrimaryButtonDown;
        InputManager.OnPrimaryButtonUp += InputManager_OnPrimaryButtonUp;
        InputManager.OnSecondaryButtonDown += InputManager_OnSecondaryButtonDown;
        InputManager.OnSecondaryButtonUp += InputManager_OnSecondaryButtonUp;

        self = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        Health = 100;
        Rank = 1;

        PrimaryDamage = 1;
        SecondaryDamage = 5;

        primaryBarrel = transform.Find("PrimaryBarrel");
        secondaryBarrel = transform.Find("SecondaryBarrel");
        lastWorldPos = transform.position;
        lastVelocity = 0;
    }

    void Update()
    {
        if (State.Current == State.GlobalState.Game)
        {
            //  Move the dragon in screen space
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
            worldPos.z = 0;
            transform.position = Vector3.Lerp(transform.position, worldPos, Time.deltaTime * responseSpeed);

            //     Debug.LogError();
            if ((transform.position.x - lastWorldPos.x) < 0.1f)
            {
                float speed = ((transform.position.x - lastWorldPos.x));
                lastVelocity = speed;
                anim.SetFloat("Speed", speed);
                //  anim.SetFloat("Speed", Mathf.Lerp(lastVelocity, speed, Time.deltaTime) );
                //  Debug.LogError(Mathf.Lerp(lastVelocity, speed, Time.deltaTime).ToString("#.0"));
            }
            else
            {
                anim.SetFloat("Speed", 1f);
            }



            switch (currentState)
            {
                case WeaponState.Idle:
                    Heat -= Time.deltaTime * coolDownSpeed;
                    if (Heat < 5)
                    {
                        Overheat = false;
                    }
                    Charge = 0;
                    break;

                case WeaponState.PrimaryActive:
                    FirePrimary();
                    Heat += Time.deltaTime * heatUpSpeed;

                    if (!alarm && Heat >= 80)
                    {
                        alarm = true;
                        AudioManager.PlayClip("prealarm", true);
                    }

                    if (alarm && Heat < 80)
                    {
                        alarm = false;
                    }

                    if (Heat >= 99)
                    {
                        AudioManager.PlayClip("alarm", true);
                        SetState(WeaponState.Idle);
                        Overheat = true;
                    }
                    Charge = 0;
                    break;

                case WeaponState.SecondaryFired:

                    Charge = 0;
                    SetState(WeaponState.Idle);
                    break;

                case WeaponState.SecondaryActive:
                    Heat -= Time.deltaTime * coolDownSpeed;
                    Charge += Time.deltaTime * chargeSpeed;
                    break;
            }

            lastWorldPos = transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (State.Current == State.GlobalState.Game)
        {
            Debug.Log("Draak");
            if (collision.gameObject.layer == 11)
            {
                AudioManager.PlayClip("xp", true);
                //  Remove XpPickup object and add experience to dragon.
                ObjectPool.RemoveXpPickup(collision.transform);
            }
            else if (collision.gameObject.layer == 12)
            {
                ObjectPool.RemoveEnemyBullet(collision.transform);
                Hit((int)Mathf.Max(1, StageManager.GetDifficulty()));
            }
        }
    }

    void OnDisable()
    {
        State.OnGlobalStateChanged -= State_OnGlobalStateChanged;
    }
    #endregion

    #region Abe interface
    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        if (newGlobalState == State.GlobalState.Initialize)
        {
           transform.DOMove(CameraBehaviour.StartPosition, 1f).OnComplete(() =>
           {
               transform.position = CameraBehaviour.StartPosition;
               AudioManager.PlayClip("dragonLong", true);
           });

        }
        else if (newGlobalState == State.GlobalState.Pause)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else if (newGlobalState == State.GlobalState.Lose)
        {
            AudioManager.PlayClip("dragonShort", true);
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }

    private void SetState(WeaponState newState)
    {
        WeaponState prevState = currentState;
        currentState = newState;
    }

    public void Hit(int damage)
    {
        Health -= damage;

        GameManager.PlayerHit(damage);
    }
    #endregion

    #region Attacks
    private void FirePrimary()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(primaryCoroutine());
        }
    }

    private void FireSecondary()
    {
        Instantiate(bombPrefab, secondaryBarrel.position, Quaternion.identity);
        StartCoroutine(secondaryCoroutine(0.05f, 2f));
    }

    IEnumerator primaryCoroutine()
    {
        coroutineRunning = true;
        anim.SetBool("PrimaryFire", true);
        source.PlayOneShot(source.clip);

        ObjectPool.CreatePlayerBullet(
            transform.position, Quaternion.identity, BulletSpeed);
            //.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);

        CameraBehaviour.ScreenShake(FireRatePrimary / 2f, Random.Range(0.3f, 0.45f), true);
        yield return new WaitForSeconds(FireRatePrimary);
        anim.SetBool("PrimaryFire", false);
        coroutineRunning = false;
    }

    IEnumerator secondaryCoroutine(float speed, float time)
    {
        burstRunning = true;
        yield return new WaitForSeconds(speed);
        burstRunning = false;
    }
    #endregion

    #region Upgrades
    public static void UpgradeWeapon(UpgradeManger.Upgrade upgrade)
    {
        if (upgrade.WeaponType == 2)
        {
            SecondaryDamage = upgrade.SecodaryDamage;
            Charge = upgrade.SecondaryChargeSpeed;
        }
        else
        {
            PrimaryDamage = upgrade.PrimaryDamage;
            heatUpSpeed = upgrade.PrimaryHeatUp;
            coolDownSpeed = upgrade.PrimaryCooldown;
            FireRatePrimary = upgrade.PrimaryFireRate;
        }
        Debug.Log("> Did upgrade");
    }
    #endregion

    #region Buttons

    private void InputManager_OnPrimaryButtonDown()
    {
        if (!primaryOverheat && currentState == WeaponState.Idle)
        {
            SetState(WeaponState.PrimaryActive);
        }
    }

    private void InputManager_OnPrimaryButtonUp()
    {
        if (currentState != WeaponState.SecondaryActive)
        {
            SetState(WeaponState.Idle);
        }
    }


    // secondary

    private void InputManager_OnSecondaryButtonDown()
    {
        if (Charge <= 0)
        {
            SetState(WeaponState.SecondaryActive);
        }
    }

    private void InputManager_OnSecondaryButtonUp()
    {
        SetState(WeaponState.Idle);

        if (Charge > 95f)
        {
            FireSecondary();
            SetState(WeaponState.SecondaryFired);
        }
    }
    #endregion
}