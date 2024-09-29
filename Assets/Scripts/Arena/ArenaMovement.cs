using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArenaMovement : MonoBehaviour
{
    private ActionControls controls;

    //X-Movement
    public float MovementSpeed = 5f;

    //Jumping
    public float jumpHeight = 7f;
    public int maxNumberOfJumps = 1;
    private bool canJump;
    private int remainingJumps;
    [Tooltip("Time Player Has to jump when Leaving the edge of a platform")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private bool isGrounded;
    [Tooltip("Jump Buffer: The time window for players to jump right before they land on the ground")]
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;



    //Dash Vars
    private bool canDash = true;
    private bool isDashing;
    public float dashSpeed = 24f;
    public float dashDistance = 0.2f;
    public float dashingCooldown = 0.5f;
    //Dash With and Without Gravity can be toggled for feel
    public bool ToggleGravityWhenDashing = false;
    
    //Animations
    private Animator _anim;
    private SpriteRenderer _sr;
    private bool IsIdle = false;
    private int IsFacingRight = -1;

    private float horizontal;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        remainingJumps = maxNumberOfJumps;

        controls.Player.JumpAction.Enable();
        controls.Player.JumpAction.started += Jump;
        controls.Player.JumpAction.performed += Jump;
        controls.Player.JumpAction.canceled += Jump;

        controls.Player.MoveArena.Enable();
        controls.Player.MoveArena.started += Move;
        controls.Player.MoveArena.canceled += Move;


        controls.Player.Dash.Enable();
        controls.Player.Dash.started += Dash;
        //controls.Player.Dash.canceled += Move;
    }

    private void Awake()
    {
        controls = new ActionControls();
    }

    //Update Will be Removed after combat scrip is finished (Only Here right for debugging Purposes)
    void Update()
    {
        if (isDashing)
            return;
        //Check Comment Above Function Header
        AnimatePlayer2D();
    }
    
    void FixedUpdate()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            print("Time Reset " + coyoteTimeCounter);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            print("Time going down " + coyoteTimeCounter);
        }



        if (isDashing)
            return;

        //Jump Buffer Logic
        if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpBufferCounter = 0f;
        }



        if (isGrounded || coyoteTimeCounter > 0f)
        {
            remainingJumps = maxNumberOfJumps;
        }


        rb.velocity = new Vector2(horizontal * MovementSpeed, rb.velocity.y);

        jumpBufferCounter -= Time.deltaTime;
    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            jumpBufferCounter = jumpBufferTime;
            canJump = true;

            if (remainingJumps > 1  && !isGrounded )
            { 
                print("Remaining Jumps " + remainingJumps);
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight); 
                remainingJumps--;
            }

        }

        
        if (context.canceled && rb.velocity.y > 0)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
       
    }
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;

    }
          
    public void Dash(InputAction.CallbackContext context)
    {
        if (canDash && context.started)
        {
            StartCoroutine(Dasher(rb.velocity.x));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    //Dashes Based off the Orientation of the Sprite
    private IEnumerator Dasher(float direction)
    {
        canDash = false;
        isDashing = true;
        int leftOrRight = IsFacingRight > 0 ? 1 : -1;
        rb.useGravity = !ToggleGravityWhenDashing;

        _anim.SetBool("IsMoving", true);
        yield return new WaitForSeconds(dashDistance);
        _anim.SetBool("IsMoving", false);
        isDashing = false;

        rb.useGravity = true;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    /*private IEnumerator Mover()
    {
        
    }*/
    /* Player Animation Function if ever Needed Can Be Commented out after Pull request
            - I'm only keeping it in my code right now because dash Coroutine needs the orientation of
                of the Sprite to know which way to dash, since the combat script handles that then I can 
                take out the animation after its pushed so I can still test my code
    */
    private void AnimatePlayer2D()
    {
        if ((horizontal != 0 || horizontal != 0) && !IsIdle)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        if (!IsIdle)
        {
            if (horizontal > 0)
            {
                _sr.flipX = true;
                IsFacingRight = 1;
            }
            else if (horizontal < 0)
            {
                _sr.flipX = false;
                IsFacingRight = -1;
            }
        }

    }
}
