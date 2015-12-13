using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraBehaviour : MonoBehaviour
{
    private static CameraBehaviour self;

    void Awake()
    {
        self = this;
    }

    public static void ScreenShake(float duration, float magnitude, bool limitZ)
    {
        if (limitZ)
        {
            self.transform.DOShakePosition(duration, new Vector3(magnitude, magnitude, 0));
        }
        else
        {
            self.transform.DOShakePosition(duration, magnitude);
        }
    }
}
