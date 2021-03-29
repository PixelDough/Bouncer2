using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenEvents
{
    
    public static void TweenPopupSpiral(Transform target, bool entering = true, bool instant = false)
    {
        LeanTween.cancel(target.gameObject);

        if (entering)
        {
            target.localScale = Vector3.zero;
            target.localRotation = Quaternion.Euler(0f, 0f, 15f);

            LeanTween.scale(target.gameObject, Vector3.one, 1.5f).setEase(LeanTweenType.easeOutElastic);
            LeanTween.rotateLocal(target.gameObject, Vector3.zero, 2f).setEase(LeanTweenType.easeOutElastic);
        }
        else
        {
            LeanTween.scale(target.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInCubic);
        }


    }

}
