using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    [Header("Move Variables")]
    [Range(0f, 1f)]
    public float accelTime = 0.058f;
    [Range(0f, 1f)]
    public float decelTime = 0.039f;
    [Range(0f, 1f)]
    public float turnTime = 0.049f;
    [Range(0f, 20f)]
    public float topSpeed = 10f;
    [Range(0f, 1f)]
    public float accelTimeAir = 0.073f;
    [Range(0f, 1f)]
    public float decelTimeAir = 0.047f;
    [Range(0f, 1f)]
    public float turnTimeAir = 0.062f;
    [Range(0f, 20f)]
    public float topSpeedAir = 10f;
    float move;

    [Header("Jump Variables")]
    public bool canDoubleJump = true;
    [Range(0f, 10f)]
    public float minJumpHeight = 2.35f;
    [Range(0f, 10f)]
    public float maxJumpHeight = 3.945f;
    [Range(0f, 1f)]
    public float jumpTimeToApex = 0.31f;
    [Range(0f, 10f)]
    public float fallGravityMultiplier = 1.55f;
    [Range(0f, 100f)]
    public float terminalVelocity = 55f;
    [Range(0f, 0.25f)]
    public float coyoteTime = 0.067f;
    [Range(0f, 0.25f)]
    public float inputBufferTime = 0.085f;
    float gravityScale;
    float jumpCutFactor;
    float jumpVelocity;
    public LayerMask groundedCheckLayers;
    float jumpInputTimer;
    float groundedTimer;
    bool jumpReleased;
    bool hasDoubleJump;
    Vector2 groundedCheckSize;
    Vector2 groundedCheckPos;

    [Header("Fall Through Variables")]
    public float onewayCornerCorrectionAmount = 0.33f;
    public float onewayDisabledTime = 0.11f;
    public LayerMask onewayCheckLayers;
    public int onewayLayerIndex;
    bool fallThroughHeld;

    [Header("Jump Corner Correction Variables")]
    public Vector2 jumpCornerCorrectSize = new Vector2(0.3f, 0.46f);

    [Header("Effects Jump Squash/Stretch Variables")]
    public Vector2 jumpSquashAmount = new Vector2(0.65f, 1.3f);
    [Range(0f, 0.4f)]
    public float jumpSquashDuration = 0.09f;
    public Ease jumpSquashEaseFunction = Ease.OutQuint;

    [Range(0f, 0.4f)]
    public float fallSquashDuration = 0.12f;
    public Ease fallSquashEaseFunction = Ease.InCubic;

    public Vector2 landSquashAmount = new Vector2(0.075f, -0.09f);
    [Range(0f, 0.4f)]
    public float landSquashDuration = 0.17f;
    [Range(0f, 10f)]
    public float landSquashVelocityFactor = 5f;

    [Header("Effects Fall Through Squash/Stretch Variables")]
    public Vector2 readyFallThroughSquashAmount = new Vector2(1.2f, 0.8f);
    [Range(0f, 0.4f)]
    public float readyFallThroughSquashDuration = 0.09f;
    public Ease readyFallThroughSquashEaseFunction = Ease.OutQuint;

    [Range(0f, 0.4f)]
    public float unreadyFallThroughSquashDuration = 0.09f;
    public Ease unreadyFallThroughSquashEaseFunction = Ease.InCubic;
    bool fallThroughSquash;

    [Header("Effects Run Lean Variables")]
    public float leanAmount = 9.5f;
    public float leanDuration = 0.24f;
    public Ease leanEaseFunction = Ease.OutQuart;
    float prevLeanDir;

    bool fallSquashed;
    bool wasGrounded;
    float prevYVel;
    float prevPrevYVel;

    [Header("Effects Eye Movement")]
    public float maxEyeOffset = 0.24f;
    public float eyeMoveSpeed = 5.5f;
    public Transform eyes;

    [Header("Effects")]
    public ParticleSystem jumpParticles;
    public ParticleSystem landParticles;
    public ParticleSystem groundParticles;
    public ParticleSystem fallThroughParticles;
    public ParticleSystem deathParticlesPrefab;
    public GameObject doubleJumpIndicatorPrefab;
    public float doubleJumpIndicatorOffset = -0.4f;
    GameObject doubleJumpIndicator;

    [Header("References")]
    public GameObject spriteObject;
    public Transform spriteContainer;
    public Transform bone;

    Rigidbody2D rigi;
    BoxCollider2D col;
    InputMain controls;

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        groundedCheckPos = new Vector2(0, -col.size.y / 2f);
        groundedCheckSize = new Vector2(col.size.x - 0.02f, 0.05f);

        gravityScale = ((-2f * maxJumpHeight) / (jumpTimeToApex * jumpTimeToApex)) / Physics2D.gravity.y;
        jumpVelocity = (2f * maxJumpHeight) / jumpTimeToApex;
        jumpCutFactor = minJumpHeight / maxJumpHeight;

        rigi.gravityScale = gravityScale;
        fallSquashed = true;
    }

    void Update()
    {

        // Decrement timers
        jumpInputTimer -= Time.deltaTime;
        groundedTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector2 vel = rigi.velocity;

        // Apply Terminal Velocity
        vel.y = Mathf.Clamp(vel.y, -terminalVelocity, Mathf.Infinity);

        // Check grounded
        if (Physics2D.OverlapBox((Vector2)transform.position + groundedCheckPos, groundedCheckSize, 0, groundedCheckLayers) && Mathf.Abs(vel.y) < 0.2f)
        {
            groundedTimer = coyoteTime;
            hasDoubleJump = true;
        }


        // Move
        float accel = (groundedTimer > 0) ? accelTime : accelTimeAir;
        float decel = (groundedTimer > 0) ? decelTime : decelTimeAir;
        float turn = (groundedTimer > 0) ? turnTime : turnTimeAir;
        float speed = (groundedTimer > 0) ? topSpeed : topSpeedAir;

        float targetXVel = move * speed;
        float reachSpeedTime;
        if (move != 0)
        {
            if (Mathf.Sign(move) != Mathf.Sign(vel.x))
                reachSpeedTime = turn;
            else
                reachSpeedTime = accel;
        }
        else
        {
            reachSpeedTime = decel;
        }
        vel.x = Mathf.MoveTowards(vel.x, targetXVel, speed / (60 * reachSpeedTime) * Time.fixedDeltaTime * 50f);


        // Jump Input
        if (jumpInputTimer > 0)
        {
            // Fall through platform
            bool onOneWay = false;
            if (fallThroughHeld && groundedTimer > 0f)
            {
                // Handle Fall through Corner Correction
                bool onewayOuterRight = Physics2D.Raycast(transform.position + new Vector3(col.size.x / 2f, -col.size.y / 2f), Vector2.down, 0.05f, onewayCheckLayers);
                bool onewayOuterLeft = Physics2D.Raycast(transform.position + new Vector3(-col.size.x / 2f, -col.size.y / 2f), Vector2.down, 0.05f, onewayCheckLayers);

                bool groundInnerRight = Physics2D.Raycast(transform.position + new Vector3(col.size.x / 2f - onewayCornerCorrectionAmount, -col.size.y / 2f), Vector2.down, 0.05f, groundedCheckLayers - onewayCheckLayers);
                bool groundInnerLeft = Physics2D.Raycast(transform.position + new Vector3(-col.size.x / 2f + onewayCornerCorrectionAmount, -col.size.y / 2f), Vector2.down, 0.05f, groundedCheckLayers - onewayCheckLayers);
                if ((onewayOuterLeft || onewayOuterRight) && (!groundInnerLeft && !groundInnerRight))
                {
                    onOneWay = true;
                    RaycastHit2D groundSideRightHit = Physics2D.Raycast(transform.position + new Vector3(col.size.x / 2f - onewayCornerCorrectionAmount, -col.size.y / 2f - 0.05f), Vector2.right, onewayCornerCorrectionAmount, groundedCheckLayers - onewayCheckLayers);
                    RaycastHit2D groundSideLeftHit = Physics2D.Raycast(transform.position + new Vector3(-col.size.x / 2f + onewayCornerCorrectionAmount, -col.size.y / 2f - 0.05f), Vector2.left, onewayCornerCorrectionAmount, groundedCheckLayers - onewayCheckLayers);
                    
                    if (groundSideRightHit.collider != null)
                    {
                        rigi.MovePosition(rigi.position + new Vector2(-(onewayCornerCorrectionAmount - groundSideRightHit.distance) - 0.03f, 0));
                    }
                    else if (groundSideLeftHit.collider != null)
                    {
                        rigi.MovePosition(rigi.position + new Vector2(onewayCornerCorrectionAmount - groundSideLeftHit.distance + 0.03f, 0));
                    }
                }
            }
            if (fallThroughHeld && groundedTimer > 0f && onOneWay)
            {
                jumpInputTimer = 0;
                vel.y = 0;
                fallThroughParticles.Play();
                StartCoroutine(FallThrough());
            }


            // Jump
            else if (groundedTimer > 0)
            {
                fallSquashed = false;
                jumpInputTimer = 0;
                groundedTimer = 0;
                vel.y = jumpVelocity;

                // Effects
                // Squash/stretch
                spriteObject.transform.DOKill();
                spriteObject.transform.DOScale(jumpSquashAmount, jumpSquashDuration).SetEase(jumpSquashEaseFunction);
                // Particles
                jumpParticles.Play();
                // Spawn double jump indicator
                if (doubleJumpIndicator == null && canDoubleJump)
                {
                    doubleJumpIndicator = Instantiate(doubleJumpIndicatorPrefab, transform.position, Quaternion.identity);
                    doubleJumpIndicator.transform.parent = transform;
                    doubleJumpIndicator.transform.localPosition = new Vector3(0, doubleJumpIndicatorOffset);
                }

            }


            // Double Jump
            else if (hasDoubleJump && canDoubleJump)
            {
                fallSquashed = false;
                jumpInputTimer = 0;
                vel.y = jumpVelocity;
                hasDoubleJump = false;

                // Effects
                // Squash/stretch
                spriteObject.transform.DOKill();
                spriteObject.transform.DOScale(jumpSquashAmount, jumpSquashDuration).SetEase(jumpSquashEaseFunction);
                // Particles
                jumpParticles.Play();
                // Double jump indicator
                if (doubleJumpIndicator != null)
                {
                    doubleJumpIndicator.GetComponent<Animator>().SetTrigger("Launch");
                    doubleJumpIndicator.transform.parent = null;
                    Destroy(doubleJumpIndicator, 0.5f);
                    doubleJumpIndicator = null;
                }

            }
        }


        // Release jump
        if (jumpReleased)
        {
            jumpReleased = false;
            if (vel.y > 1f)
            {
                fallSquashed = true;
                // Effects
                // Squash/stretch
                spriteObject.transform.DOKill();
                spriteObject.transform.DOScale(1f, fallSquashDuration).SetEase(fallSquashEaseFunction);
                vel.y *= jumpCutFactor;
                // Particles stop
                jumpParticles.Stop();

            }
        }


        // Jump Corner Corrections
        if (vel.y > 1f)
        {
            Vector3 pos = transform.position;
            Vector2 corner = col.size / 2f;
            bool outerRight = Physics2D.Raycast(pos + (Vector3)corner,
                Vector2.up, jumpCornerCorrectSize.y, groundedCheckLayers);
            bool outerLeft = Physics2D.Raycast(pos + new Vector3(-corner.x, corner.y),
                Vector2.up, jumpCornerCorrectSize.y, groundedCheckLayers);
            bool innerRight = Physics2D.Raycast(pos + new Vector3(corner.x - jumpCornerCorrectSize.x, corner.y),
                Vector2.up, jumpCornerCorrectSize.y, groundedCheckLayers);
            bool innerLeft = Physics2D.Raycast(pos + new Vector3(-corner.x + jumpCornerCorrectSize.x, corner.y),
                Vector2.up, jumpCornerCorrectSize.y, groundedCheckLayers);
            if ((outerRight || outerLeft) && (!innerLeft && !innerRight))
            {
                RaycastHit2D sideRightHit = Physics2D.Raycast(pos + new Vector3(corner.x - jumpCornerCorrectSize.x, corner.y + jumpCornerCorrectSize.y),
                    Vector2.right, jumpCornerCorrectSize.x, groundedCheckLayers);
                RaycastHit2D sideLeftHit = Physics2D.Raycast(pos + new Vector3(-corner.x + jumpCornerCorrectSize.x, corner.y + jumpCornerCorrectSize.y),
                    Vector2.left, jumpCornerCorrectSize.x, groundedCheckLayers);
                if (sideRightHit.collider != null && move != 1)
                {
                    rigi.MovePosition(rigi.position + new Vector2(-(jumpCornerCorrectSize.x - sideRightHit.distance) - 0.03f, 0));
                }
                else if (sideLeftHit.collider != null && move != -1)
                {
                    rigi.MovePosition(rigi.position + new Vector2(jumpCornerCorrectSize.x - sideLeftHit.distance + 0.03f, 0));
                }
            }
        }


        // Unsquash if falling (only when release jump was not called)
        if (vel.y < 1f && !fallSquashed)
        {
            fallSquashed = true;
            // Effects
            // Squash/stretch
            spriteObject.transform.DOKill();
            spriteObject.transform.DOScale(1f, fallSquashDuration).SetEase(fallSquashEaseFunction);
            // Particles stop
            jumpParticles.Stop();
        }


        // Spawn double jump indicator when leaving the ground
        if (wasGrounded && groundedTimer < 0)
        {
            // Spawn double jump indicator
            if (doubleJumpIndicator == null && canDoubleJump)
            {
                doubleJumpIndicator = Instantiate(doubleJumpIndicatorPrefab, transform.position, Quaternion.identity);
                doubleJumpIndicator.transform.parent = transform;
                doubleJumpIndicator.transform.localPosition = new Vector3(0, doubleJumpIndicatorOffset);
            }
        }


        // Squash when hitting the ground
        if (!wasGrounded && groundedTimer > 0f)
        {
            // Effects
            float impactFactor = Mathf.Lerp(0, 1, -prevPrevYVel / terminalVelocity);
            spriteObject.transform.DOKill();
            spriteObject.transform.localScale = Vector3.one;
            spriteObject.transform.DOPunchScale((impactFactor * landSquashVelocityFactor + 1f) * landSquashAmount, landSquashDuration).SetRelative();

            var em = landParticles.emission;
            em.SetBurst(0, new ParticleSystem.Burst(0f, (int)(impactFactor * 12) + 5f));
            landParticles.Play();

            // Delete double jump indicator
            Destroy(doubleJumpIndicator);
        }


        // Squash when holding down
        if (fallThroughHeld && !fallThroughSquash && groundedTimer > 0)
        {
            fallThroughSquash = true;
            spriteContainer.DOScale(readyFallThroughSquashAmount, readyFallThroughSquashDuration).SetEase(readyFallThroughSquashEaseFunction);
        }
        else if (fallThroughSquash)
        {
            spriteContainer.DOScale(new Vector3(1f, 1f), unreadyFallThroughSquashDuration).SetEase(unreadyFallThroughSquashEaseFunction);
            fallThroughSquash = false;
        }


        // Ground particles
        if (move != 0 && groundedTimer > 0f)
        {
            if (!groundParticles.isPlaying)
                groundParticles.Play();
        }
        else
        {
            groundParticles.Stop();
        }


        // Set Run Lean
        int leanDir = (int)Mathf.Sign(move) * ((move == 0) ? 0 : 1);
        if (leanDir != prevLeanDir)
        {
            bone.DORotate(new Vector3(0, 0, leanAmount * leanDir + 90f), leanDuration).SetEase(leanEaseFunction);
        }
        prevLeanDir = leanDir;


        // Move Eyes
        eyes.localPosition = new Vector3(Mathf.MoveTowards(eyes.localPosition.x, maxEyeOffset * leanDir, eyeMoveSpeed * Time.fixedDeltaTime), eyes.localPosition.y);


        // Set gravity (Fast Fall)
        if (vel.y < -0.5f)
            rigi.gravityScale = gravityScale * fallGravityMultiplier;
        else
            rigi.gravityScale = gravityScale;


        // Set was grounded
        prevPrevYVel = prevYVel;
        prevYVel = vel.y;
        if (groundedTimer > 0)
            wasGrounded = true;
        else
            wasGrounded = false;


        // Set rigidbody velocity
        rigi.velocity = vel;
    }

    IEnumerator FallThrough()
    {
        Physics2D.IgnoreLayerCollision(onewayLayerIndex, gameObject.layer, true);
        yield return new WaitForSeconds(onewayDisabledTime);
        Physics2D.IgnoreLayerCollision(onewayLayerIndex, gameObject.layer, false);
    }

    /*
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube((Vector2)transform.position + groundedCheckPos, new Vector3(groundedCheckSize.x, groundedCheckSize.y, 1));
    }
    */

    //-----------------------------Input Actions Setup----------------------
    void Awake()
    {
        //----------------------Setup input events---------------------------
        controls = new InputMain();
        // Jump Press
        controls.Gameplay.Jump.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
                jumpInputTimer = coyoteTime;
            else if (!ctx.ReadValueAsButton())
                jumpReleased = true;

        };
        // Move Horizontal
        controls.Gameplay.Move.performed += ctx => move = Mathf.Sign(ctx.ReadValue<float>());
        controls.Gameplay.Move.canceled += ctx => move = 0f;
        // Fall through platform
        controls.Gameplay.FallThrough.performed += ctx => fallThroughHeld = ctx.ReadValueAsButton();

    }
    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
