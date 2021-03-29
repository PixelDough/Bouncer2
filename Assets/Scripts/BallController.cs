using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
public class BallController : MonoBehaviour
{
    [Range(1, 12)]
    public int raycastVerticalCount = 6;
    public float coyoteTimeMax = 0.1f;
    public float maxHorizontalVelocity = 100f;
    public ParticleSystem particleSystemDust;
    public GameObject ballObject;
    public GameObject ballSprite;

    [Header("FMOD Sounds")]
    [FMODUnity.EventRef]
    public string JumpSound;
    FMOD.Studio.EventInstance JumpSoundEvent;
    [FMODUnity.EventRef]
    public string BounceSound;
    FMOD.Studio.EventInstance BounceSoundEvent;
    [FMODUnity.ParamRef]
    public string PlayerSpeed;

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

        moveInput += Mathf.Clamp(Input.acceleration.x * 3, -1, 1);

        if (moveInput != 0f) { boostDirection = Mathf.Sign(moveInput); }
        if (!boostPressed) { 
            boostPressed = player.GetButtonDown(RewiredConsts.Action.Jump);

            if (!boostPressed) boostPressed = Input.touchCount > 0;
        }

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

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(PlayerSpeed, rb2d.velocity.magnitude);

        Vector2 dir = rb2d.velocity.normalized;

        Vector3 velScale = new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), 0f);
        velScale = Vector3.zero;

        ballObject.transform.localScale = Vector3.Lerp(ballObject.transform.localScale, Vector3.one + velScale, 5f * Time.deltaTime);
        ballSprite.transform.rotation = transform.rotation;



        if (player.GetButtonDown(RewiredConsts.Action.Pause)) { Application.Quit(); }

        coyoteTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {

        //foreach(JumpThroughPlatform p in FindObjectsOfType<JumpThroughPlatform>())
        //{
        //    p.gameObject.GetComponent<Collider2D>().isTrigger = !(p.gameObject.GetComponent<Collider2D>().bounds.max.y < transform.position.y - GetComponent<CircleCollider2D>().radius * 0.99f);
        //}
        isGrounded = false;

        //ContactPoint2D[] points = new ContactPoint2D[10];
        //rb2d.GetContacts(points);
        //foreach (ContactPoint2D p in points)
        //{
        //    if (p.normal.y * rb2d.gravityScale > .15f && Vector2.Distance(p.point, transform.position) >= collider.radius - 0.01f && (Mathf.Sign(rb2d.velocity.y) == -Mathf.Sign(p.normal.y) || rb2d.velocity.y == 0))
        //    {
        //        isGrounded = true;
        //        coyoteTime = coyoteTimeMax;
        //        groundNormal = p.normal;
        //        break;
        //    }
        //}


        if (moveInput != 0 && Mathf.Abs(rb2d.velocity.x) < maxHorizontalVelocity)
        {
            
            rb2d.AddForce(new Vector2(moveInput * 150f, 0));
            
            
        }

        if (boostPressed && coyoteTime > 0)
        {

            Vector2 jumpVector = Vector2.up * rb2d.gravityScale;

            rb2d.velocity = new Vector2(rb2d.velocity.x, 10f * rb2d.gravityScale);

            coyoteTime = 0;

            var Shot = FMODUnity.RuntimeManager.CreateInstance(JumpSound);
            Shot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            Shot.start();

            ballObject.transform.localScale = Vector3.one * 2f;
            if (particleSystemDust)
            {
                particleSystemDust.Play();
            }
            //if (moveInput != 0)
            //    rb2d.AddForce((boostDirection * Vector2.right) * 50f, ForceMode2D.Impulse);
        }


        boostPressed = false;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        foreach (ContactPoint2D c in collision.contacts)
        {
            if (Mathf.Abs(c.normal.y) > Mathf.Abs(c.normal.x))
            {
                if (Mathf.Sign(rb2d.velocity.y) == Mathf.Sign(c.normal.y))
                {
                    if (collision.relativeVelocity.magnitude > 5 && !isGrounded)
                    {
                        Debug.Log("Bounce Y!");

                        //ballObject.transform.localScale = Vector3.one - ((Vector3)c.normal / 2);
                        if (particleSystemDust)
                        {
                            particleSystemDust.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(transform.position, c.point));
                            particleSystemDust.Play();
                        }

                        //ballObject.transform.localScale = ballObject.transform.localScale - ((Vector3)c.normal / 2);

                        var Shot = FMODUnity.RuntimeManager.CreateInstance(BounceSound);
                        Shot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                        Shot.start();
                    }
                }
            }
            else
            {
                if (Mathf.Sign(rb2d.velocity.x) == Mathf.Sign(c.normal.x))
                {
                    if (collision.relativeVelocity.magnitude > 5)
                    {
                        Debug.Log("Bounce X!");
                        var Shot = FMODUnity.RuntimeManager.CreateInstance(BounceSound);
                        Shot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                        Shot.start();

                        if (particleSystemDust)
                        {
                            particleSystemDust.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(transform.position, c.point));
                            particleSystemDust.Play();
                        }

                        //ballObject.transform.localScale = ballObject.transform.localScale - ((Vector3)c.normal / 2);
                        
                        // Wall Bouncing height
                        if (rb2d.velocity.y > 0)
                        {
                            //rb2d.velocity = new Vector2(rb2d.velocity.x, 10f * rb2d.gravityScale);
                            rb2d.AddForce(Vector2.up * 1000);
                        }
                    }
                }
            }

            CheckGrounded(c);

        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        foreach (ContactPoint2D c in collision.contacts)
        {
            //if (c.normal.y * rb2d.gravityScale > .15f && Vector2.Distance(c.point, transform.position) >= collider.radius - 0.01f && (Mathf.Sign(rb2d.velocity.y) == -Mathf.Sign(c.normal.y) || rb2d.velocity.y == 0))
            //{
            //    isGrounded = true;
            //    coyoteTime = coyoteTimeMax;
            //    groundNormal = c.normal;
            //    break;
            //}

            CheckGrounded(c);
        }
    }


    private void CheckGrounded(ContactPoint2D contactPoint)
    {
        if (contactPoint.normal.y * rb2d.gravityScale > .15f && Vector2.Distance(contactPoint.point, transform.position) >= collider.radius - 0.01f && contactPoint.relativeVelocity.y >= 0)
        {
            isGrounded = true;
            coyoteTime = coyoteTimeMax;
            groundNormal = contactPoint.normal;
        }
    }

}
