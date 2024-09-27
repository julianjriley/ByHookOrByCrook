using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArenaMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    
    //Dash Vars
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashTime = 0.2f;
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
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDashing)
            return;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        AnimatePlayer2D();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (canDash && context.performed)
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

    private IEnumerator Dasher(float direction)
    {
        canDash = false;
        isDashing = true;
        
        int leftOrRight = IsFacingRight > 0 ? 1 : -1;
        rb.useGravity = !ToggleGravityWhenDashing;

        _anim.SetBool("IsMoving", true);
        /*if(direction == 0 && !IsFacingRight)
        {
            _sr.flipX = true;
            IsFacingRight = true;
        }*/
        rb.velocity = new Vector2(transform.localScale.x * dashingPower * leftOrRight, 0f);
        yield return new WaitForSeconds(dashTime);
        _anim.SetBool("IsMoving", false);
        isDashing = false;

        rb.useGravity = true;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

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
