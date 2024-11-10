using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;
using FMODUnity;
//using FMOD;

public class ArenaMovement : MonoBehaviour
{
    private ActionControls controls;

    [Header("OverHeated Buff Info")]
    private bool _isOverHeated;
    [SerializeField, Tooltip("Is gun OverHeated")]
    public bool infiniJumpFish = false;
    private bool _canInfiniJump = false;


    //X-Movement
    [Header("Movment Variables")]
    [SerializeField, Tooltip("Speed of Character Movement")]
    public float MaxSpeed = 7f;
    //Read Input for x-movement
    private float _horizontalMovemenet;
    private float _movementTimer = 0.1f;
    private float _movementMaxTime = 0.1f;

    //Jumping
    [Header("Jump Variables")]
    [SerializeField, Tooltip("Amount of jumps before player is grounded")]
    public int maxNumberOfJumps = 1;
    //Amount of Jumps starting from 0 to 1-maxNumberOfJumps
    public int _jumpCounterIndex = 0;  
    [SerializeField, Tooltip("Cyote Time: Time Player Has to jump when Leaving the edge of a platform")]
    private float _coyoteMaxTime = 0.3f;
    private float _coyoteTimer;
    [SerializeField, Tooltip("Jump Buffer: The time window for players to jump again right before they land on the ground")]
    private float _jumpBufferMaxTime = 0.5f;
    private float _jumpBufferTimer;
    [SerializeField, Tooltip("How long the player is in the air for, Longer the Durration the higher charater goes")]
    public float jumpDuration = .2f;
    private float _jumpDurrationTimer;
    [SerializeField, Tooltip("How much force upward to apply to the character upon jump press.\n\n More force makes the character jump faster")]
    public float jumpImpulseUP = 10f;
    [SerializeField, Tooltip("Amount of force exerted downward upon the release of the Jump Button or Full Jump Duration\n\n More force makes the character fall faster")]
    public float jummpImpulseDown = 0.5f;


    // Grounded
    [Header("Grounded Variables")]
    [SerializeField, Tooltip("Max number distance of box cast grounded check, distance before character is considered grounded")]
    private float _maxGroundedDistance = 0.1f;
    [SerializeField, Tooltip("Ground collision layer.")]
    private LayerMask _groundLayer;
    [SerializeField, Tooltip("Whether the player is contacting the ground; should not be changed in inspector.")]
    private bool _isGrounded;

    //Dash Vars
    //If a player CAN Dash
    private bool canDash = true;
    //If player is currently Dashing
    private bool isDashing;
    //How fast the player is dashing
    [Header("Dash Variables")]
    [SerializeField, Tooltip("How Fast the player Dashes")]
    private float _dashSpeed = 24f;
    [SerializeField, Tooltip("How Far the player Dashes")]
    public float DashDuration = 0.2f;
    //Flag for the cooldown is done so the player can press dash again
    private bool IsDashingInAir;
    [SerializeField, Tooltip("Dash With and Without Gravity can be toggled for feel")]
    private bool _toggleGravityWhenDashing = false;
    [SerializeField, Tooltip("Amount of air dashes in quick succession")]
    public int numberOfAirDashes = 1;
    private int _dashCount;
    private bool _cyoteDash = true;
    public bool canDashResetJump = false;
    [SerializeField]TrailRenderer _trailRender;

    [Header("SFX")]
    [SerializeField] EventReference footstepsSound;
    private EventInstance footsteps;
    [SerializeField] EventReference dashSound;
    [SerializeField] EventReference jumpSound;
    [SerializeField] EventReference doubleJumpSound;
    [SerializeField] ParticleSystem dust;

    //Animations
    private Animator _anim;
    private SpriteRenderer _sr;
    //Sprite Characteristics Variables
    private bool _isIdle = false;
    private int _isFacingRight = -1;
    private float _directionOfCharacterMovement;

    //Player Components
    private Rigidbody rb;
    private Collider _collider;
    private PlayerCombat _playerCombat;


    //Buff Specific Variables
    public bool dashRestricted = false; 

    //Platform Notifier
    public float GoThroughPlatforms;


    void Start()
    {
        //Get Player Character components
        rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();
        _playerCombat = GetComponent<PlayerCombat>();
        _trailRender = GetComponent<TrailRenderer>();
        _trailRender.emitting = false;

        //Set Global variables
        _jumpCounterIndex = maxNumberOfJumps;
        _jumpBufferTimer =_jumpBufferMaxTime;
        _dashCount = numberOfAirDashes;

        //Bind JumpInput      
        //controls.Player.JumpAction.Enable();
        controls.Player.JumpAction.started += JumpInput;
        controls.Player.JumpAction.performed += JumpInput;
        controls.Player.JumpAction.canceled += JumpInput;

        //Bind MoveInput
        //controls.Player.MoveArena.Enable();
        controls.Player.MoveArena.started += MoveInput;
        controls.Player.MoveArena.performed += MoveInput;
        controls.Player.MoveArena.canceled += MoveInput;

        //Bind DashInput
        //controls.Player.Dash.Enable();
        controls.Player.Dash.started += DashInput;

        //Create Footsteps EventInstance
        footsteps = SoundManager.Instance.CreateInstance(footstepsSound);
    }
    //
    private void Awake()
    {
        controls = new ActionControls();
        //Enable Player Movement Binds
        controls.Player.JumpAction.Enable();
        controls.Player.MoveArena.Enable();
        controls.Player.Dash.Enable();
    }

    //Update: used to dcrement/Increment Timers only
    void Update()
    {
        //Debug.Log(gameObject.transform.localScale.x);
        if (isDashing)
            return;
        //Check Comment Above Function Header
        _coyoteTimer -= Time.deltaTime;
        _movementTimer -= Time.deltaTime;
        //Debug.Log("Jump Durration: " + _jumpDurrationTimer);
        AnimatePlayer2D();

        //Checks If Gun is Over Heated
        Debug.Log(_coyoteTimer);
        _isOverHeated =  _playerCombat.GetWeaponInstance().GetOverHeatedState();
        if (_isOverHeated && infiniJumpFish)
            _canInfiniJump = true;
        else
            _canInfiniJump = false;
    }
    
    void FixedUpdate()
    {
        groundedCheck();
        //Alloows for no other input while dashing
        if (isDashing)
        {
            _coyoteTimer = 0;
            return;
        } 
        Jump();
        Move();
        GoThroughPlatforms = controls.Player.MoveArena.ReadValue<Vector2>().y;
    }

    /*  JumpInput: Runs when jump button is pressed
     *      - Handles Reseting jump buffer on press to allow a jump
     *      - Increments Jump counter index
     *      - Handles how high the player jumps based on release of button
     *  Input: Keybind for Jump
     */
    public void JumpInput(InputAction.CallbackContext context)
    {
        //If press/held rest jumpBufferTimer to allow for one jump
            //Increment JumpCounter to next index
        if (context.started)
        {
            _jumpBufferTimer = _jumpBufferMaxTime;
            if(!_isGrounded)
                _jumpCounterIndex++;

            _jumpDurrationTimer = jumpDuration;

        }

        //If jump is relaesed pull the charcter down so it doesn't float
        if (context.canceled && _jumpDurrationTimer > 0)
        {
            //If character is going down with more jumps pull down 
            // AND: only apply downward force burst if the player cancelled jump early
            if (rb.velocity.y > 0 && _jumpCounterIndex < maxNumberOfJumps && _jumpDurrationTimer < jumpDuration) {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector2.down *jummpImpulseDown , ForceMode.Impulse);
                _anim.Play("Idle");
                _coyoteTimer = 0f;
            }

            // ensure we start applyingprocessing falling speed
            _jumpDurrationTimer = jumpDuration;
        }
       
    }

    /*  Jump: Functionality of how player Jumps
     *      - Handles the force to add to player for upwards momentum
     *      - Handles when to jump based off of
     *              - Jump buffer time
     *              - Amount of Jumps
     *              - Durration of Jump
     *              - CyoteTime
     *      - Handles how fast the player falls after the apex of the jump
     */
    private void Jump()
    {
        //Allows for the player to jump higher with constant force applied up for a set 
        //Durration while space is held
        if (_jumpDurrationTimer < jumpDuration && _jumpDurrationTimer > 0)
        {
            //_anim.Play("Jump");
        }

        //Apply Force Down When timer finishes
        if (_jumpDurrationTimer <= 0)
        {
            rb.AddForce(Vector2.down * jummpImpulseDown, ForceMode.Impulse);
        }

        //Jump Logic
        if (_jumpBufferTimer > 0f)
        {   
            //Initial Jump
            if (_coyoteTimer > 0f || _canInfiniJump)
            {
                SoundManager.Instance.PlayOneShot(jumpSound, gameObject.transform.position);
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                rb.AddForce(Vector2.up * jumpImpulseUP, ForceMode.Impulse);
                dust.Play();
                _anim.Play("Jump", -1, 0f);
                // prevent high jump with no control
                _jumpBufferTimer = 0;
            }
            //Other Jumps after 0th jump
            else if ((!_isGrounded && _jumpCounterIndex < maxNumberOfJumps) && !_canInfiniJump)
            {
                SoundManager.Instance.PlayOneShot(doubleJumpSound, gameObject.transform.position);
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector2.up * jumpImpulseUP, ForceMode.Impulse);
                _anim.Play("Jump");
                dust.Play();
                // prevent high jump with no control
                _jumpBufferTimer = 0;
            }
            //Decrement Timer if character is in the air to allow for 
            //The time window for players to jump again right before they land on the ground
            else if (!_isGrounded && _jumpCounterIndex > 0)
            {
                _jumpBufferTimer -= Time.deltaTime;
            }
        }

        //Reset Durration
        if (rb.velocity.y < 0 || _jumpDurrationTimer > jumpDuration)
        {
            _jumpDurrationTimer = 0;
        }

        //Play Idle Anim
        if (rb.velocity.y < -0.001 || _jumpDurrationTimer > jumpDuration)
            _anim.Play("Idle");

    }

    /*  MoveInput: Runs when left or right movement is pressed
     *      - Handles if player is able to move
     *      - Reads constant velocity while button is pressed
     *  Input: Keybind for moving right and left
     */
    public void MoveInput(InputAction.CallbackContext context)
    {
        //Reads x-distence if character can move
        if (context.performed)
        {
            _horizontalMovemenet = context.ReadValue<Vector2>().x; 
            if(_isGrounded)
                dust.Play();
        }
        // Additions for fixed one way plats ----
        else _horizontalMovemenet = 0f;
        if(context.ReadValue<Vector2>().y < 0f && _isGrounded && context.started)
        {
            //Debug.Log("Im minging");
            rb.AddForce(Vector2.down * 10, ForceMode.Impulse);
        }
        // ------

        if (context.canceled)
        {
            _movementTimer = _movementMaxTime;
            _horizontalMovemenet = 0;
        }
    }
    /*  MoveInput: Moves player based on read value from button press
     *          - Moves at constant velocity
     */
    private void Move()
    {
        //Character Velocity
        rb.velocity = new Vector2(_horizontalMovemenet * MaxSpeed, rb.velocity.y);
        //Debug.Log("Grounded: " + _isGrounded);
        //Debug.Log("Dashing: " + isDashing);
        //Debug.Log("Movement Velocity: " + _horizontalMovemenet);
        //Debug.Log("Footsteps: " + footsteps.getPlaybackState(out PLAYBACK_STATE state));

        //Footsteps Sound Control
        PLAYBACK_STATE footstepsState;
        footsteps.getPlaybackState(out footstepsState);
        if (_isGrounded && !isDashing && rb.velocity.x != 0f && !footstepsState.Equals(PLAYBACK_STATE.PLAYING))
        {
            footsteps.start();
        }
        else if ((!_isGrounded || isDashing || rb.velocity.x == 0f) && footstepsState.Equals(PLAYBACK_STATE.PLAYING))
        {
            footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
    /*  DashInput: Runs when dash button is pressed
     *      - Has a cooldown so players cant spam the button
     *      - Has A Dash Count for the amount of dash successions
     *  Input: Keybind for dashing
     */
    public void DashInput(InputAction.CallbackContext context)
    {
        //Flag for the cooldown is done so the player can press dash again
        //As well as there's a dashCount for amount of dashes that can be done in succesion
        //How long it takes for the play to press dash agian (Used so the player can't spam the button)
        if (!IsDashingInAir && _dashCount > 0 && !dashRestricted)
        {
            StartCoroutine(Dash());
            //if We Have the "Dash Rests Jump Counter Fish"
            if (canDashResetJump)
            {
                //Set Jump Count index to 0;
                _jumpCounterIndex = -1;
            }
        }
    }

    /* COROUTINE: 
    *  Dash: Moves player a certian distence at a speed in the x direction
    *  for a specified time
    *          - Dashes direction Based off the Orientation of the Sprite
    *          - Has the ability to only dash dashCount amount in air in the air
    *          - Ability to toggle weather gravity should or should not be used
    */
    private IEnumerator Dash()
    {
        if (canDash)
        {
            //Dash Succession Logic For MultiDash
            if (!_isGrounded || !_cyoteDash)
                _dashCount--;

            //Set Values/Flags Before Dashing
            IsDashingInAir = true;
            isDashing = true;
            int leftOrRightOrrientation =  gameObject.transform.localScale.x > 0 ? 1 : -1;
            rb.useGravity = !_toggleGravityWhenDashing;
            _trailRender.emitting = true;

            float dashDirection = _horizontalMovemenet;
            if (_horizontalMovemenet == 0)
                dashDirection = gameObject.transform.localScale.x;  
            
            //Dash a certian distance
            //_anim.SetBool("IsMoving", true);
            SoundManager.Instance.PlayOneShot(dashSound, gameObject.transform.position);
            _anim.Play("Dash");
            rb.velocity = new Vector2(transform.localScale.x * _dashSpeed  * leftOrRightOrrientation * dashDirection, 0f);

            _playerCombat.InvincibleDash(DashDuration - 0.1f);
            yield return new WaitForSeconds(DashDuration);
            if(_isGrounded)
                dust.Play();
            _trailRender.emitting = false;
            _anim.Play("Idle");
            //Reset Values/Flags after to original state
            isDashing = false;
            rb.useGravity = true;
            //If there are no  more dashes left
            if (_dashCount <= 0 )
            {
                //Allows for no more dashes in the air after 
                    //all dashes have been used
                canDash = false;
                //Adds the cooldown
                _dashCount = numberOfAirDashes;
            }
            IsDashingInAir = false;

        }
    }

    /* GroundCheck: Checks that are made when player is on the ground and while cyote time is not 0
    *  for a specified time 
    *          - handles checks when the player is grounded
    *          - hanadles checks when the player is Not grounded
    *          - handles exceptions with jump whil the player still has time remaing to jump durring cyote time
    */
    private void groundedCheck()
    {

        // Update grounded state //
        Vector3 castOrigin = transform.position + new Vector3(0, _maxGroundedDistance / 2f, 0);
        _isGrounded = Physics.BoxCast(castOrigin, _collider.bounds.extents, Vector3.down, transform.rotation, _maxGroundedDistance * 1.5f, _groundLayer);

        //Rest some values when player touches the ground
        if (_isGrounded)
        {
            _coyoteTimer = _coyoteMaxTime;
            canDash = true;
            _jumpDurrationTimer = jumpDuration;

        }

        //Reset Some values when cyote time is still Active
        if (_coyoteTimer > 0)
        {
            _cyoteDash = true;
            _jumpCounterIndex = 0;
        }else
            _cyoteDash = false;

        //Update Values while in the air
        if (!_isGrounded)
        {
            _jumpBufferTimer -= Time.deltaTime;
            _jumpDurrationTimer -= Time.deltaTime;
        }

    }

    
    /* Player Animation Functions if ever Needed Can Be Commented out after Pull request
            - I'm only keeping it in my code right now because dash Coroutine needs the orientation of
                of the Sprite to know which way to dash, since the combat script handles that then I can 
                take out the animation after its pushed so I can still test my code
    */
    private void AnimatePlayer2D()
    {
        MovementAnimator();
    }

    public void MovementAnimator()
    {

        int leftOrRightOrrientation = gameObject.transform.localScale.x > 0 ? 1 : -1;
        if (leftOrRightOrrientation < 0 && _isGrounded)
        {
            if (_horizontalMovemenet < 0)
            {
                _anim.SetBool("isMovingForward", true);
                _anim.SetBool("isMoving", true);

            }
            else if (_horizontalMovemenet > 0)
            {
                _anim.SetBool("isMovingForward", false);
                _anim.SetBool("isMoving", true);
            }
            else
            {
                if (_movementTimer < 0)
                    _anim.SetBool("isMoving", false);
            }
        }
        else if (leftOrRightOrrientation > 0 && _isGrounded) 
        {
            if (_horizontalMovemenet > 0)
            {
                _anim.SetBool("isMovingForward", true);
                _anim.SetBool("isMoving", true);

            }
            else if (_horizontalMovemenet < 0)
            {
                _anim.SetBool("isMovingForward", false);
                _anim.SetBool("isMoving", true);
            }
            else
            {
                if (_movementTimer > 0)
                    _anim.SetBool("isMoving", false);
            }
        }else
            _anim.SetBool("isMoving", false);

    }


    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.MoveArena.Disable();
            controls.Player.JumpAction.Disable();
            controls.Player.Dash.Disable();
        }
    }

}

