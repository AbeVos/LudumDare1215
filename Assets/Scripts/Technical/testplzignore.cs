using UnityEngine;
using System.Collections;

public class testplzignore : MonoBehaviour
{
    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, 0.3f * Mathf.Sin(Time.realtimeSinceStartup * 2.5f), transform.localPosition.z);
    }
}
