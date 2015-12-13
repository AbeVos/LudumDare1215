using UnityEngine;
using System.Collections;

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

    [Header("Weapon Speeds")]
    [SerializeField, Range(10, 40)]
    private float chargeSpeed = 10;
    [SerializeField, Range(1, 40)]
    private float heatUpSpeed = 10;
    [SerializeField, Range(1, 40)]
    private float coolDownSpeed = 10;
    [SerializeField, Range(0.05f, 1f)]
    private float FireRatePrimary = 0.2f;
    [SerializeField, Range(0.05f, 1f)]
    private float BulletSpeed = 0.2f;

    private bool coroutineRunning = false;
    private bool burstRunning = false;
    /// <summary>Heat is at 100%, primary can only start again at 0% heat</summary>
    private static bool primaryOverheat;
    private WeaponState currentState;
    private Vector3 lastWorldPosition;
    private AudioSource source;
    #endregion

    #region statics
    /// <summary>Static reference to self.</summary>
    private static Dragon self;
    private static int _health, _exp = 0;
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
    #endregion

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
        lastWorldPosition = transform.position;
        source = GetComponent<AudioSource>();
        Health = 100;
    }

    void Update()
    {
        if (State.Current == State.GlobalState.Game)
        {
            //  Move the dragon in screen space
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
            worldPos.z = 0;
            transform.position = Vector3.Lerp(lastWorldPosition, worldPos, Time.deltaTime * responseSpeed);
            lastWorldPosition = transform.position;
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
                if (Heat >= 99)
                {
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

    }

    void OnDisable()
    {
        State.OnGlobalStateChanged -= State_OnGlobalStateChanged;
    }

    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
    }

    private void SetState(WeaponState newState)
    {
        WeaponState prevState = currentState;
        currentState = newState;
        Debug.Log(newState);
    }


    public void Hit(int damage)
    {
        Health -= damage;
    }

    private void FirePrimary()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(primaryCoroutine());
        }
    }


    private void FireSecondary()
    {
        StartCoroutine(secondaryCoroutine(0.05f, 2f));
    }

    IEnumerator primaryCoroutine()
    {
        coroutineRunning = true;
        source.PlayOneShot(source.clip);
        ObjectPool.CreatePlayerBullet(
            transform.position, Quaternion.identity, BulletSpeed)
            .transform.GetChild(0).rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        CameraBehaviour.ScreenShake(FireRatePrimary / 2f, Random.Range(0.3f, 0.45f), true);
        yield return new WaitForSeconds(FireRatePrimary);
        coroutineRunning = false;
    }

    IEnumerator secondaryCoroutine(float speed, float time)
    {
        burstRunning = true;
        yield return new WaitForSeconds(speed);
        burstRunning = false;
    }


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