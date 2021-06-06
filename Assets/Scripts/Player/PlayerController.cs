using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using static CommonLibrary.CommonDefinitions;
using static CommonLibrary.CommonMethods;
using static CommonLibrary.PlayerDefinitions;

public class PlayerController : MonoBehaviour
{
    private float graceTimer;
    private float moveInput;
    private bool  jumpInput;
    private float timeStamp;

    private Transform groundCheckLeft;
    private Transform groundCheckMiddle;
    private Transform groundCheckRight;
    
    private Vector3 velocityBeforePhysicsUpdate;
    private Vector3 positionBeforePhysicsUpdate;
    private Vector3 lastGroundLocation; // location to return to if player falls and doesn't die

    [HideInInspector]
    public  bool playerHanging;
    private bool hasSword;
    private bool isRunning;
    private bool isAttacking;
    private bool isInAir;
    private bool isGrounded;
    private int  throwingState;
    private bool isJumping;
    private bool isFalling;
    private bool isOnMovingPlatform;

    public SpriteRenderer spriteRenderer;

    private SpringJoint2D playerSpring;
    private Rigidbody2D rb;
    private Animator anim;
    public /*new*/ AudioSource audio;
    private PlayerHealth playerHealth;
    private GameObject mostRecentCheckpoint;
    private PlayerInput playerInput;

    private static Vector3 RESPAWN_VERTICAL_OFFSET = new Vector3(0, 0.495204f, 0);


    public NewInput input;

    private PlayerStateManager stateManager;
    private PlayerSettings     settings;
    private PlayerAudioSources audioSource;

    void Start()
    {
        stateManager = GameObject.Find("Player State").GetComponent<PlayerStateManager>();
        settings     = stateManager.Settings;
        audioSource  = GameObject.Find("Audio").GetComponent<AudioManager>().PlayerAudio;

        groundCheckLeft   = transform.Find("Ground Check [Left]");
        groundCheckMiddle = transform.Find("Ground Check [Middle]");
        groundCheckRight  = transform.Find("Ground Check [Right]");

        rb           = GetComponent<Rigidbody2D>();
        anim         = GetComponent<Animator>();
        playerSpring = GetComponent<SpringJoint2D>();
        playerHealth = GetComponent<PlayerHealth>();
        playerInput  = GetComponent<PlayerInput>();

        stateManager.RespawnLocation = transform.position + RESPAWN_VERTICAL_OFFSET;

        ResetState();
        SetupInputListeners();
    }

    private void ResetState()
    {
        graceTimer = settings.CoyoteTimeGraceTimer;
        ResetAnimationStates(); // PlayerAnimator -> public bool HasSword { get; set; }
    }

    private void ResetAnimationStates()
    {
        playerHanging      = false;
        hasSword           = true;
        isRunning          = false;
        isAttacking        = false;
        isInAir            = false;
        isJumping          = false;
        isFalling          = false;
        isOnMovingPlatform = false;
        throwingState      = THROWING_INACTIVE;
    }

    private void SetupInputListeners()
    {
        input.Player_onMove += _input => moveInput = _input;
        input.Player_onMove += _input => { if(_input != 0) OnAim(new Vector2(_input, 0)); }; // turn towards direction of movement
        input.Player_onJump += _input => jumpInput = _input;

        input.Player_onAim  += OnAim; // turn on aim

        input.Hanging_onSwing += _input => moveInput = _input;
        input.Hanging_onSwing += _input => { if (_input != 0) OnAim(new Vector2(_input, 0)); }; // turn towards direction of movement
        input.Hanging_onJump  += _input => BreakOffHangingSword( true);
        input.Hanging_onDrop  +=     () => BreakOffHangingSword(false);

        playerHealth.OnDeath += OnDeath;
    }

    private bool OnPlatformHasFinished = false;

    void FixedUpdate()
    {
        positionBeforePhysicsUpdate = transform.position; //For popping player back outside of a wall they overlap with
        velocityBeforePhysicsUpdate = rb.velocity; //Value to help calculate fall speed before collision

        isGrounded = IsOnGround();

        MoveUpdate();
        JumpUpdate();

        if(!isGrounded)
            graceTimer -= Time.fixedDeltaTime;
        else // isGrounded
        {
            graceTimer  = settings.CoyoteTimeGraceTimer;

            if(!isOnMovingPlatform || isOnMovingPlatform && OnPlatformHasFinished)
                lastGroundLocation = transform.position; // could do on jump, but doesn't account for walking off an edge
        }

        if(isAttacking && isInAir)
            isInAir = false;

        if(isInAir)
        {
            isFalling = rb.velocity.y < 0;
            isJumping = rb.velocity.y > 0;
        }
        else
            isFalling = isJumping = false;

        UpdateAnimations();
    }

    private void JumpUpdate()
    {
        if(rb.velocity.y < 0) // falling
            rb.velocity += Vector2.up * Physics2D.gravity.y * (settings.FallMultiplier    - 1) * Time.fixedDeltaTime;
        else if(rb.velocity.y > 0 && !jumpInput)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (settings.LowJumpMultiplier - 1) * Time.fixedDeltaTime;

        if(jumpInput)
        {
            if(isGrounded && graceTimer != settings.CoyoteTimeGraceTimer) // has landed
                jumpInput = false;
            else if(isGrounded || graceTimer > 0)
            {
                rb.velocity = Vector2.up * settings.JumpForce;
                graceTimer = 0; //Avoids multiple jumps during grace period
            }
        }

        isInAir = !isGrounded;
    }

    Vector2 lastPosition;

    public void MoveUpdate()
    {
        if(moveInput != 0 && lastPosition == (Vector2) transform.position)
            transform.position += new Vector3(OFFSET_PER_PIXEL * moveInput, 0, 0);

        rb.velocity = new Vector2(moveInput * settings.MovementSpeed, rb.velocity.y);

        if(Mathf.Abs(moveInput) > 0 && isGrounded)
        {
            isRunning = true;

            //audio.clip  = audioSource.Footsteps;
            audio.pitch = Random.Range(0.6f, 0.9f);
            audio.UnPause();
        }
        else
        {
            isRunning = false;
            audio.Pause();
        }

        lastPosition = transform.position;
    }
    
    public void OnAim(Vector2 position)
    {
         // takes normalized Vector2 value:
        //   [0,0] - center / playerPos
        //   each axis varies [-1, 1]:
        //     x = -1 -> all left, x = 1 -> all right
        //     y = -1 -> all down, y = 1 -> all up

        if(Input.GetKey(KeyCode.K))
            Debug.Log("aim: " + position);

        int  rotationValue = (position.x > 0) ? 0 : (position.x < 0) ? -180 : transform.rotation.y == 0 ? 0 : -180;
        transform.rotation = Quaternion.Euler(0, rotationValue, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Checkpoint") && stateManager.RespawnLocation != other.transform.position)
            stateManager.RespawnLocation = other.transform.position + RESPAWN_VERTICAL_OFFSET;
        else if(other.gameObject.layer == LayerMask.NameToLayer("Water") || other.gameObject.layer == LayerMask.NameToLayer("Respawn"))
        {
            if(stateManager.Health - 2 > 0)
            {
                ReturnToLastGroundLocation();
                playerHealth.PlayerDamage(2, Vector2.zero); // after moving -> sound effect at player location
            }
            else
                playerHealth.Respawn();
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        CheckIfHangingSword(col);
        FallDamageCheck(col);

        if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(IsOverlapping(GetComponent<Collider2D>(), col.gameObject.GetComponent<Collider2D>()))
                transform.position = positionBeforePhysicsUpdate;
        }
    }

    private bool IsOnGround()
    {
        System.Func<Transform, bool> checkTransform = (groundCheck) => {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Ground", "Enemy", "Archer", "Grates"));

            return Physics2D.Raycast(groundCheck.position, Vector2.down, filter, new List<RaycastHit2D>(), OFFSET_PER_PIXEL) > 0  &&
                 !((Physics2D.Raycast(groundCheck.position + new Vector3(0, OFFSET_PER_PIXEL * 5, 0), Vector2.down, filter, new List<RaycastHit2D>(), OFFSET_PER_PIXEL) > 0) &&
                   (Physics2D.Raycast(groundCheck.position - new Vector3(0, OFFSET_PER_PIXEL * 5, 0), Vector2.down, filter, new List<RaycastHit2D>(), OFFSET_PER_PIXEL) > 0));
        };

        return checkTransform(groundCheckLeft) || checkTransform(groundCheckMiddle) || checkTransform(groundCheckRight) || isOnMovingPlatform;//|| graceTimer > 0;
    }

    public void SetOnMovingPlatform(bool boo)
    {
        isOnMovingPlatform = boo;
    }

    public void SetOnMovingPlatform(bool boo, bool hasFinished = false)
    {
        isOnMovingPlatform = boo;
        OnPlatformHasFinished = hasFinished;
    }

    private void UpdateAnimations()
    {
        anim.SetBool("Has Sword", hasSword);
        anim.SetBool("Running", isRunning);
        anim.SetBool("Attacking", isAttacking);
        anim.SetBool("Jumping", isJumping);
        anim.SetBool("Falling", isFalling);
        anim.SetBool("Hanging", playerHanging);
        anim.SetInteger("Throwing", throwingState);

        if(isInAir && !hasSword)
            anim.ResetTrigger("Throw");
    }

    public void SetAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }

    public void SetHasSword(bool hasSword)
    {
        this.hasSword = hasSword;
    }

    public void SetThrowingState(Vector2 throwDirection)
    {
        int animationState = (throwDirection == Vector2.down || throwDirection.y < -.5f) ? THROWING_DOWN :
                             (throwDirection == Vector2.up   || throwDirection.y >  .5f) ? THROWING_UP   :
                             (throwDirection != Vector2.zero) ? THROWING_SIDE :
                             THROWING_INACTIVE;

        throwingState = animationState;

        if(throwingState != THROWING_INACTIVE)
        { 
            anim.SetTrigger("Throw");
            anim.SetInteger("Throwing", throwingState);
        }
    }

    // for toggling checkpoint fires
    public GameObject GetMostRecentCheckpoint()
    {
        return mostRecentCheckpoint;
    }

    public void SetMostRecentCheckpoint(GameObject obj)
    {
        mostRecentCheckpoint = obj;
    }

    public Vector3 GetCheckpointLocation() => stateManager.RespawnLocation - RESPAWN_VERTICAL_OFFSET;

    bool canSwitch = true;

    public void setHanging(Rigidbody2D sword)
    {
        if(canSwitch)
        {
            canSwitch = false;

            if(sword != null)
            {
                playerSpring.connectedBody = sword;
                playerSpring.enabled       = true;
                playerHanging              = true;

                playerInput.SwitchCurrentActionMap("Hanging");
            }
            else
            {
                playerSpring.enabled = false;
                playerHanging        = false;

                playerInput.SwitchCurrentActionMap("Player");
            }
        }
        else
            canSwitch = true;
    }

    private void CheckIfHangingSword(Collision2D col)
    {
        if(jumpInput && col.gameObject.CompareTag("Sword") && col.gameObject.GetComponent<SwordController>().CanHangOnSword && timeStamp <= Time.time && !isGrounded)
        {
            if(CheckHeightOfLocation(transform.position) < 2)
                return;

            setHanging(col.gameObject.GetComponent<Rigidbody2D>());

            timeStamp = Time.time + 2;
        }
    }

    private void BreakOffHangingSword(bool jumping)
    {
        if(playerHanging) //Removes player from sword hanging when jump is pressed
        {
            setHanging(null);

            if(jumping) // drop if false
            {
                int sign = moveInput > 0 ? 1 : -1;

                rb.AddForce(new Vector2(sign * 30, 8), ForceMode2D.Impulse);
            }
        }
    }

    public bool IsHanging()
    {
        return playerHanging;
    }

    private void FallDamageCheck(Collision2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Ground") && Mathf.Abs(velocityBeforePhysicsUpdate.y) > settings.FatalFallSpeed)
        {
            if(stateManager.Health - 2 > 0)
            {
                ReturnToLastGroundLocation();
                playerHealth.PlayerDamage(2, Vector2.zero); // after moving -> sound effect at player location
            }
            else
                playerHealth.Respawn();
        }
        else if(col.gameObject.layer == LayerMask.NameToLayer("Ground") && Mathf.Abs(velocityBeforePhysicsUpdate.y) > settings.DamageFallSpeed)
            playerHealth.PlayerDamage(2, Vector2.zero);
    }

    private void ReturnToLastGroundLocation()
    {
        float xChange = transform.position.x > lastGroundLocation.x ? -.5f : .5f;
        transform.position = new Vector3(Mathf.Round(lastGroundLocation.x) + xChange, Mathf.Ceil(lastGroundLocation.y), 0); // respawn on center of tile

        rb.velocity = Vector2.zero;
        GetComponent<PlayerThrow>().ResetSword();
        ResetState();
        
    }

    private void OnDeath()
    {
        transform.position = stateManager.RespawnLocation;
        rb.velocity = Vector2.zero;
        GetComponent<PlayerThrow>().ResetSword();
        ResetState();
        playerInput.SwitchCurrentActionMap("Player");

        StartCoroutine(MakingSureYouAreSafe()); //Puts you back where you should be when you spawn badly on a checkpoint
    }

    IEnumerator MakingSureYouAreSafe()
    {
        //Debug.Log("In Coroutine");
        yield return new WaitForSeconds(0.01f);

        if(transform.position != stateManager.RespawnLocation)
            transform.position = stateManager.RespawnLocation;

        yield break;
    }
}