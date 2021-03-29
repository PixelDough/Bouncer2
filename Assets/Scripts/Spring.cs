using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    public Transform springMain;
    public float bounceHeight = 16f;
    [FMODUnity.EventRef]
    public string springEvent;

    bool sprung = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sprung) { return; }

        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(LaunchSpring(collision.attachedRigidbody));
            
        }
    }


    private IEnumerator LaunchSpring(Rigidbody2D rb)
    {
        sprung = true;

        LeanTween.moveLocalY(springMain.gameObject, -0.25f, 0.1f).setEaseOutCubic();
        yield return new WaitForSeconds(0.1f);

        FMODUnity.RuntimeManager.PlayOneShot(springEvent);

        rb.velocity = new Vector2(0f, Mathf.Sqrt(-2 * bounceHeight * Physics2D.gravity.y));
        //rb.AddForce(transform.up * (), ForceMode2D.Impulse);
        LeanTween.moveLocalY(springMain.gameObject, 1f, 0.1f).setEaseOutElastic();
        yield return new WaitForSeconds(0.1f);

        LeanTween.moveLocalY(springMain.gameObject, 0f, 0.25f).setEaseInOutCubic();
        yield return new WaitForSeconds(1f);

        sprung = false;

        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * bounceHeight));
    }

}
