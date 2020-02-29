using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    public bool isVisible = true;

    public Renderer renderer;


    private void Update()
    {
        float alpha = renderer.material.color.a;
        alpha = Mathf.Lerp(alpha, isVisible ? 1 : 0, 10 * Time.deltaTime);

        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);

    }


    public void SetVisibility(bool _isVisible)
    {
        isVisible = _isVisible;
    }
}
