using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraBehaviour : MonoBehaviour
{
    private static CameraBehaviour self;

    void Awake ()
    {
        self = this;
    }
     
    public static void ScreenShake (float duration, float magnitude)
    {
        self.transform.DOShakePosition(duration, magnitude);
    }
}
