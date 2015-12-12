using UnityEngine;
using DG.Tweening;
using System.Collections;

public class InputManager : MonoBehaviour
{
    #region Variables
    public float dragonToCameraDistance = 15;
    [Range(4,10)]
    public float responseSpeed = 5f;

    private Vector3 mousePosition, lastWorldPosition;
    #endregion

    #region Delegates
    public delegate void PrimaryButtonDown();
    public delegate void PrimaryButtonUp();
    public delegate void SecondaryButtonDown();
    public delegate void SecondaryButtonUp();

    public static event PrimaryButtonDown OnPrimaryButtonDown;
    public static event PrimaryButtonDown OnPrimaryButtonUp;
    public static event SecondaryButtonDown OnSecondaryButtonDown;
    public static event SecondaryButtonDown OnSecondaryButtonUp;
    #endregion

    #region Public functions
    #endregion

    #region Built in functions
    #endregion

    #region Setup
    void Start()
    {
        mousePosition = Vector3.zero;
        lastWorldPosition = transform.position;
    }
    #endregion

    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = dragonToCameraDistance;
        transform.position = Vector3.Lerp(lastWorldPosition, Camera.main.ScreenToWorldPoint(mousePosition), Time.deltaTime * responseSpeed);
        lastWorldPosition = transform.position;
    }
}
