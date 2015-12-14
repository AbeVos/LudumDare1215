using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraBehaviour : MonoBehaviour
{
    private static CameraBehaviour self;
    
    new private static Camera camera;

    void Awake ()
    {
        self = this;
        camera = GetComponentInChildren<Camera>();
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
            camera.transform.DOMove(introTransform.position, 2f);
            camera.transform.DORotate(introTransform.eulerAngles, 2f);
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
