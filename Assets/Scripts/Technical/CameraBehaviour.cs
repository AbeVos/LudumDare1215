using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraBehaviour : MonoBehaviour
{
    new private static Camera camera;
    private static Vector3 startPosition;

    public static Vector3 StartPosition
    {
        get { return startPosition; }
    }

    void Awake ()
    {
        camera = GetComponentInChildren<Camera>();
        startPosition = transform.Find("StartPosition").position;
    }

    void OnEnable ()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;
    }

    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        if (newGlobalState == State.GlobalState.Start)
        {
            Transform introTransform = transform.Find("IntroCamera");
            camera.transform.position = introTransform.position;
            camera.transform.rotation = introTransform.rotation;
        }
        else if (newGlobalState == State.GlobalState.Initialize)
        {
            Transform introTransform = transform.Find("GameTransform");
            camera.transform.DOMove(introTransform.position, 4f);
            camera.transform.DORotate(introTransform.eulerAngles, 4f);
        }
    }

    public static void ScreenShake(float duration, float magnitude, bool limitZ)
    {
        if (limitZ)
        {
            camera.transform.DOShakePosition(duration, new Vector3(magnitude, magnitude, 0));
        }
        else
        {
            camera.transform.DOKill();
            camera.transform.DOShakePosition(duration, magnitude);
        }
    }
}
