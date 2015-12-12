using UnityEngine;
using System.Collections;

public class Dragon : MonoBehaviour, GameActor
{
    [SerializeField, Range(4, 10)]
    private float responseSpeed = 5f;

    private Vector3 lastWorldPosition;

    /// <summary>Static reference to self.</summary>
    private static Dragon self;
    private static int _health, _charge, _heat, _exp;

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
    public static int Epx
    {
        get { return _exp; }
        set { _exp = value; }
    }
    public static int Charge
    {
        get { return _charge; }
        private set
        {
            if (value <= 100 && value <= 0) { _charge = value; }
        }

    }
    public static int Heat
    {
        get { return _health; }
        private set
        {
            if (value <= 100 && value <= 0) { _health = value; }
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
    }

    void OnDisable()
    {
        State.OnGlobalStateChanged -= State_OnGlobalStateChanged;
    }

    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
    }

    private void InputManager_OnPrimaryButtonDown()
    {
        Debug.Log("Prime Down");
    }

    private void InputManager_OnPrimaryButtonUp()
    {
        Debug.Log("Prime UP");
    }

    private void InputManager_OnSecondaryButtonDown()
    {
        Debug.Log("Second DOWN");
    }

    private void InputManager_OnSecondaryButtonUp()
    {
        Debug.Log("Second UP");
    }

    public void Hit(int damage)
    {
    }
}