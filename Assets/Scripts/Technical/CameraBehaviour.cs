using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject Stage;
    new private static Camera camera;
    private static Vector3 startPosition;
    private static Transform gameTransform;

    public static Vector3 StartPosition
    {
        get { return startPosition; }
    }

    void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        startPosition = transform.Find("StartPosition").position;
        gameTransform = transform.Find("GameTransform");
    }

    void OnEnable()
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
            camera.transform.DOMove(gameTransform.position, GameManager.introTime);
            camera.transform.DORotate(gameTransform.eulerAngles, GameManager.introTime);
        }
    }

    public static void ScreenShake(float duration, float magnitude, bool limitZ)
    {
        if (limitZ)
        {
            camera.transform.DOKill();
            camera.transform.DOShakePosition(duration, new Vector3(magnitude, magnitude, 0)).OnComplete(() => { camera.transform.position = gameTransform.position; });
        }
        else
        {
            camera.transform.DOKill();
            camera.transform.DOShakePosition(duration, magnitude).OnComplete(() => { camera.transform.position = gameTransform.position; });
        }
    }

    public static void WeaponShake(float duration, float magnitude)
    {
        camera.transform.DOKill();
        camera.transform.DOShakePosition(duration, new Vector3(magnitude, magnitude*1.5f, 0)).OnComplete(() => { camera.transform.position = gameTransform.position; });
    }
}
