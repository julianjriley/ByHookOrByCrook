using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.XR;


public class ArenaMovement : MonoBehaviour
{
    private ActionControls controls;

    //X-Movement
    [Header("Movment Variables")]
    [SerializeField, Tooltip("Speed of Character Movement")]
    public float MaxSpeed = 7f;
    //Read Input for x-movement
    private float _horizontal;

    //Jumping
    [Header("Jump Variables")]
    [SerializeField, Tooltip("Amount of jumps before player is grounded")]
    public int maxNumberOfJumps = 1;
    //Amount of Jumps starting from 0 to 1-maxNumberOfJumps
    private int _jumpCounterIndex = 0;
    
    [SerializeField, Tooltip("Cyote Time: Time Player Has to jump when Leaving the edge of a platform")]
    private float _coyoteMaxTime = 0.3f;
    private float _coyoteTimer;
   
    [SerializeField, Tooltip("Jump Buffer: The time window for players to jump again right before they land on the ground")]
    private float _jumpBufferMaxTime = 0.5f;
    private float _jumpBufferTimer;

    [SerializeField, Tooltip("Amount of force exerted downward upon the release of the Jump Button")]
    private float _jummpCancelForce = 0.5f;

    [Header("Jump Height: A Combination of Jump Force and Durration and Fall Speed can \nbe tuned to create the player's optimal jump Height\n")]
    [SerializeField, Tooltip("How long the player is in the air for, Longer the Durration the higher charater goes at the same speed")]
    public float jumpDurrationMaxTime = .2f;
    [SerializeField, Tooltip("How much force upward to apply to the character upon jump press.\n\n More force makes the character jump faster and higher")]
    public float jumpUpForce = 10f;
    private float jumpDurrationTimer;
    [SerializeField, Tooltip("How much force is exerting downward after Jump Durration is over. \n\n The more force the faster the player will fall. \n\n If the Falling Force > JumpUp Force, the player will go down after the durration ends")]
    public float jumpFallingForce = 11f;


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
    public float dashDistance = 0.2f;
    //Flag for the cooldown is done so the player can press dash again
    private bool IsDashingInAir;
    [SerializeField, Tooltip("Dash With and Without Gravity can be toggled for feel")]
    private bool _toggleGravityWhenDashing = false;
    [SerializeField, Tooltip("Amount of air dashes in quick succession")]
    public int numberOfAirDashes = 1;
    private int _dashCount;
    
    //Animations
    private Animator _anim;
    private SpriteRenderer _sr;
    //Sprite Characteristics Variables
    private bool _isIdle = false;
    private int _isFacingRight = -1;

    //Player Components
    private Rigidbody rb;
    private Collider _collider;

    void Start()
    {
        //Get Player Character components
        rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();

        //Set Global variables
        _jumpCounterIndex = maxNumberOfJumps;
        _jumpBufferTimer =_jumpBufferMaxTime;
        _dashCount = numberOfAirDashes;

        //Bind JumpInput      
        controls.Player.JumpAction.Enable();
        controls.Player.JumpAction.started += JumpInput;
        controls.Player.JumpAction.performed += JumpInput;
        controls.Player.JumpAction.canceled += JumpInput;

        //Bind MoveInput
        controls.Player.MoveArena.Enable();
        controls.Player.MoveArena.started += MoveInput;
        controls.Player.MoveArena.performed += MoveInput;
        controls.Player.MoveArena.canceled += MoveInput;

        //Bind DashInput
        controls.Player.Dash.Enable();
        controls.Player.Dash.started += DashInput;
    }

    private void Awake()
    {
        controls = new ActionControls();
    }

    //Update: used to dcrement/Increment Timers only
    void Update()
    {
        if (isDashing)
            return;
        //Check Comment Above Function Header
        _coyoteTimer -= Time.deltaTime;
        AnimatePlayer2D();
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

            jumpDurrationTimer = jumpDurrationMaxTime;

        }

        
        //If jump is relaesed pull the charcter down so it doesn't float
        if (context.canceled )
        {
            //If character is going down with more jumps pull down
            if (rb.velocity.y > 0 && _jumpCounterIndex < maxNumberOfJumps) {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector2.down *_jummpCancelForce , ForceMode.Impulse);
                _coyoteTimer = 0f;
            }
        }
       
    }

    /*  Jump: Functionality of how player Jumps
     *      - Handles the force to add to player for upwards momentum
     *      - Handles when to jump based off of
     *              - Jump buffer time
     *              - Amount of Jumps
     *              - CyoteTime
     *      - Handles how fast the player falls after the apex of the jump
     */
    private void Jump()
    {
        //Allows for the player to jump higher with constant force applied up for a set 
        //Durration while space is held
        if (jumpDurrationTimer < jumpDurrationMaxTime && jumpDurrationTimer > 0)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
            rb.AddForce(Vector2.down * jumpFallingForce, ForceMode.Force);
        }

        //Jump Logic
        if (_jumpBufferTimer > 0f)
        {   
            //Initial Jump
            if (_coyoteTimer > 0f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector2.up * jumpUpForce, ForceMode.Impulse);

                // prevent high jump with no control
                _jumpBufferTimer = 0;
            }
            //Other Jumps after 0th jump
            else if (!_isGrounded && _jumpCounterIndex < maxNumberOfJumps)
            {

                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector2.up * jumpUpForce, ForceMode.Impulse);

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

        //Fall Speed Force downward
        if (rb.velocity.y < 0)
        {
           // rb.AddForce(Vector2.down * jumpFallingForce, ForceMode.Force);

            jumpDurrationTimer = 0;
        }

    }

    /*  MoveInput: Runs when left or right movement is pressed
     *      - Handles if player is able to move
     *      - Reads constant velocity while button is pressed
     *  Input: Keybind for moving right and left
     */
    public void MoveInput(InputAction.CallbackContext context)
    {
        //Reads x-distence if character can move
        if(context.performed) 
            _horizontal = context.ReadValue<Vector2>().x;
        else _horizontal = 0f;

    }
    /*  MoveInput: Moves player based on read value from button press
     *          - Moves at constant velocity
     */
    private void Move()
    {
        //Character Velocity
        rb.velocity = new Vector2(_horizontal * MaxSpeed, rb.velocity.y);
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
        if (!IsDashingInAir && _dashCount > 0)
            StartCoroutine(Dash());
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
            //Set Values/Flags Before Dashing
            IsDashingInAir = true;
            isDashing = true;
            int leftOrRight = _isFacingRight > 0 ? 1 : -1;
            rb.useGravity = !_toggleGravityWhenDashing;

            //Dash a certian distance
            _anim.SetBool("IsMoving", true);
            rb.velocity = new Vector2(transform.localScale.x * _dashSpeed * _isFacingRight, 0f);
            yield return new WaitForSeconds(dashDistance);
            _anim.SetBool("IsMoving", false);

            //Reset Values/Flags after to original state
            isDashing = false;
            rb.useGravity = true;

            //Dash Succession Logic For MultiDash
            _dashCount--;
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

        // Update grounded state
        Vector3 castOrigin = transform.position + new Vector3(0, _maxGroundedDistance / 2f, 0);
        _isGrounded = Physics.BoxCast(castOrigin, _collider.bounds.extents, Vector3.down, transform.rotation, _maxGroundedDistance * 1.5f, _groundLayer);

        //Rest some values when player touches the ground
        if (_isGrounded)
        {
            _coyoteTimer = _coyoteMaxTime;
            canDash = true;
            jumpDurrationTimer = jumpDurrationMaxTime;
        }

        //Reset Some values when cyote time is still Active
        if (_coyoteTimer > 0)
        {
            _jumpCounterIndex = 0;
        }

        //Update Values while in the air
        if (!_isGrounded)
        {
            _jumpBufferTimer -= Time.deltaTime;
            jumpDurrationTimer -= Time.deltaTime;
        }

    }

   //Bellow can be commented out depending on other scripts
    
    /* Player Animation Function if ever Needed Can Be Commented out after Pull request
            - I'm only keeping it in my code right now because dash Coroutine needs the orientation of
                of the Sprite to know which way to dash, since the combat script handles that then I can 
                take out the animation after its pushed so I can still test my code
    */
    private void AnimatePlayer2D()
    {
        if ((_horizontal != 0 || _horizontal != 0) && !_isIdle)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        if (!_isIdle)
        {
            if (_horizontal > 0)
            {
                _sr.flipX = true;
                _isFacingRight = 1;
            }
            else if (_horizontal < 0)
            {
                _sr.flipX = false;
                _isFacingRight = -1;
            }
        }

    }
}
