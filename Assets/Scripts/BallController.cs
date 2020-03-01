using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Range(1, 12)]
    public int raycastVerticalCount = 6;
    public float coyoteTimeMax = 0.1f;
    public GameObject ballObject;

    private float coyoteTime = 0f;

    Rigidbody2D rb2d;
    CircleCollider2D collider;

    bool isGrounded = false;

    float moveInput = 0f;
    bool boostPressed = false;
    float boostDirection = 0f;

    DistanceJoint2D rope = null;

    Vector2 groundNormal = Vector2.up;

    // Rewired Info
    Player player;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        rope = GetComponent<DistanceJoint2D>();

        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        moveInput = player.GetAxis(RewiredConsts.Action.RollHorizontal);
        if (moveInput != 0f) { boostDirection = Mathf.Sign(moveInput); }
        if (!boostPressed) { boostPressed = player.GetButtonDown(RewiredConsts.Action.Jump); }

        // Grappling Hook
        //if (Input.GetMouseButtonDown(0))
        //{
        //    rope.enabled = true;
        //    rope.connectedAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    rope.enableCollision = true;
        //}

        //if (Input.GetMouseButtonUp(0) && rope.enabled)
        //{
        //    rope.enabled = false;
        //}

        if (player.GetButtonDown(RewiredConsts.Action.Pause)) { Application.Quit(); }

        coyoteTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {

        //foreach(JumpThroughPlatform p in FindObjectsOfType<JumpThroughPlatform>())
        //{
        //    p.gameObject.GetComponent<Collider2D>().isTrigger = !(p.gameObject.GetComponent<Collider2D>().bounds.max.y < transform.position.y - GetComponent<CircleCollider2D>().radius * 0.99f);
        //}

        ContactPoint2D[] points = new ContactPoint2D[10];
        rb2d.GetContacts(points);
        foreach (ContactPoint2D p in points)
        {
            if (p.normal.y * rb2d.gravityScale > 0)
            {
                isGrounded = true;
                coyoteTime = coyoteTimeMax;
                groundNormal = p.normal;
            }
        }
        

        if (moveInput != 0)
        {
            
            rb2d.AddForce(new Vector2(moveInput * 150f, 0));

            
        }

        if (boostPressed && coyoteTime > 0)
        {

            Vector2 jumpVector = Vector2.up * rb2d.gravityScale;

            rb2d.velocity = new Vector2(rb2d.velocity.x, 10f * rb2d.gravityScale);

            coyoteTime = 0;
            
            //if (moveInput != 0)
            //    rb2d.AddForce((boostDirection * Vector2.right) * 50f, ForceMode2D.Impulse);
        }


        boostPressed = false;

    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    isGrounded = false;

    //    foreach(ContactPoint2D point in collision.contacts)
    //    {
    //        if (point.normal.y > 0)
    //        {
    //            isGrounded = true;
    //            coyoteTime = coyoteTimeMax;
    //        }
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    isGrounded = false;

    //    foreach (ContactPoint2D point in collision.contacts)
    //    {
    //        if (point.normal.y > 0)
    //        {
    //            isGrounded = true;
    //            coyoteTime = coyoteTimeMax;
    //        }
    //    }
    //}

}
