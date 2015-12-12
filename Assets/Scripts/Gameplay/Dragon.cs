using UnityEngine;
using System.Collections;

public class Dragon : MonoBehaviour, GameActor
{
    [SerializeField, Range(4, 10)]
    private float responseSpeed = 5f;

    private Vector3 lastWorldPosition;

    /// <summary>Static reference to self.</summary>
    private static Dragon self;

    public static Dragon dragon
    {
        get { return self; }
    }

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
    }

    private void InputManager_OnPrimaryButtonUp()
    {
    }

    private void InputManager_OnSecondaryButtonDown()
    {
    }

    private void InputManager_OnSecondaryButtonUp()
    {
    }

    public void Hit(int damage)
    {
    }
}