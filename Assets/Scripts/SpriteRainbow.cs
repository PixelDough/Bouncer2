using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRainbow : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(ChangeColors());
    }

    private void Update()
    {
        
    }

    private IEnumerator ChangeColors()
    {
        while (true)
        {
            float a = spriteRenderer.color.a;
            spriteRenderer.color = Random.ColorHSV();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, a);

            yield return new WaitForSeconds(0.1f);
        }
    }

}
