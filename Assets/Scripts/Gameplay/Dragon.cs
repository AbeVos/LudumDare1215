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
    [SerializeField, Range(10, 30)]
    private float chargeSpeed = 10;
    [SerializeField, Range(1, 30)]
    private float heatUpSpeed = 10;
    [SerializeField, Range(1, 30)]
    private float coolDownSpeed = 10;


    /// <summary>Heat is at 100%, primary can only start again at 0% heat</summary>
    private bool primaryOverheat;
    private WeaponState currentState;
    private Vector3 lastWorldPosition;
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
    }

    void Update()
    {
        if (State.Current == State.GlobalState.Game)
        {
            //  Move the dragon in screen space
            transform.position = Vector3.Lerp(lastWorldPosition, Camera.main.ScreenToWorldPoint(InputManager.MousePosition), Time.deltaTime * responseSpeed);
            lastWorldPosition = transform.position;
        }

        

        switch (currentState)
        {
            case WeaponState.Idle:
                Heat -= Time.deltaTime * coolDownSpeed;
                Charge = 0;
                break;
            case WeaponState.PrimaryActive:
                FirePrimary();
                Heat += Time.deltaTime * heatUpSpeed;
                Charge = 0;
                break;
            case WeaponState.SecondaryFired:
                Charge = 0;
                SetState( WeaponState.Idle);
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
        Debug.Log("Primary");
    }

    private void FireSecondary()
    {
        Debug.Log("Secondary");
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
        SetState(WeaponState.Idle);
    }

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

        if ( Charge > 95f)
        {
            FireSecondary();
            SetState(WeaponState.SecondaryFired);
        }
    }
    #endregion
}