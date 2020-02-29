using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : MonoBehaviour
{

    public Transform sprite;

    bool pressed = false;
    float leanDirection = 1f;

    private void Update()
    {
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
            if (!pressed)
            {
                leanDirection = Mathf.Sign(collision.attachedRigidbody.velocity.x);
            }

            if (Mathf.Abs(collision.attachedRigidbody.velocity.x) > 0)
            {
                float angle = Mathf.LerpAngle(sprite.transform.localRotation.eulerAngles.z, -30f * leanDirection, 10f * Time.deltaTime);
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }

            //sprite.transform.Rotate(Vector3.forward, -30f * Mathf.Sign(collision.attachedRigidbody.velocity.x));
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
