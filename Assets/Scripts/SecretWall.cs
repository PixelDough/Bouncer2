using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    public bool isVisible = true;
    public bool destroyOnComplete = false;

    public Renderer renderer;


    public void SetVisibility(bool _isVisible)
    {
        isVisible = _isVisible;
        LeanTween.alpha(gameObject, isVisible ? 1 : 0, 0.25f).destroyOnComplete = (!isVisible && destroyOnComplete);
    }
}
