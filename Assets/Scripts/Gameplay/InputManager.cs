using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    #region Variables

    //private Vector3 mousePosition, lastWorldPosition;
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

    public static Vector3 MousePosition
    {
        get
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            return mousePosition;
        }
    }

    public static KeyCode PrimaryButton { get; set; }
    public static KeyCode SecondaryButton { get; set; }
    public static bool keysRemaped { get; set; }

    void Update()
    {
        if (keysRemaped)
        {
            if (Input.GetKeyUp(PrimaryButton) && OnPrimaryButtonUp != null)
            {
                OnPrimaryButtonUp();
            }
            else if (Input.GetKeyDown(PrimaryButton) && OnPrimaryButtonDown != null)
            {
                OnPrimaryButtonDown();
            }

            if (Input.GetKeyUp(SecondaryButton) && OnSecondaryButtonUp != null)
            {
                OnSecondaryButtonUp();
            }
            else if (Input.GetKeyDown(SecondaryButton) && OnSecondaryButtonDown != null)
            {
                OnSecondaryButtonDown();
            }
        }

        else
        {
            if (Input.GetMouseButtonUp(0) && OnPrimaryButtonUp != null)
            {
                OnPrimaryButtonUp();
            }
            else if (Input.GetMouseButtonDown(0) && OnPrimaryButtonDown != null)
            {
                OnPrimaryButtonDown();
            }

            if (Input.GetMouseButtonUp(1) && OnSecondaryButtonUp != null)
            {
                OnSecondaryButtonUp();
            }
            else if (Input.GetMouseButtonDown(1) && OnSecondaryButtonDown != null)
            {
                OnSecondaryButtonDown();
            }
        }
    }
}
