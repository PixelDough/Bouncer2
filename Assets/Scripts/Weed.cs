using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : MonoBehaviour
{

    public Transform sprite;
    public ParticleSystem hitParticle;
    public Transform flowerHead;

    bool pressed = false;
    float leanDirection = 1f;

    private void Update()
    {
        // If the player is no longer colliding, lean back to 0.
        if (!pressed && sprite.transform.localRotation.eulerAngles.z != 0f)
        {
            float angle = Mathf.LerpAngle(sprite.transform.localRotation.eulerAngles.z, 0f, 5f * Time.deltaTime);
            sprite.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            // Only set the lean direction based on the way the player first collided with it. If they change directions while it's pressed, don't change the lean direction.
            if (!pressed)
            {
                leanDirection = Mathf.Sign(collision.attachedRigidbody.velocity.x);
                if (hitParticle != null && flowerHead != null) hitParticle.Play();
                if (flowerHead != null) Destroy(flowerHead.gameObject);
            }

            // Lean while the player is still colliding with it.
            if (Mathf.Abs(collision.attachedRigidbody.velocity.x) > 0)
            {
                float angle = Mathf.LerpAngle(sprite.transform.localRotation.eulerAngles.z, -30f * leanDirection, 10f * Time.deltaTime);
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }

            pressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            pressed = false;
        }
    }

}
