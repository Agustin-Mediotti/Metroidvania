using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alucard : MonoBehaviour
{
    // public fields
    public float speed = 500;

    // private fields
    [SerializeField] Collider2D standingCollider;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int totalJumps;
    [SerializeField] float jumpPower = 500;
    [SerializeField] bool crouchPressed = false;
    [SerializeField] bool isRunning = true;

    Rigidbody2D rb;
    Animator animator;

    float horizontalValue;
    float runSpeedModifier = 2f;
    float crouchSpeedModifier = 0.5f;
    bool facingRight = true;
    bool isGrounded = false;
    int avaiblejumps;
    bool multipleJump;
    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;
    bool coyoteJump;


    void Awake()
    {
        avaiblejumps = totalJumps;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (CanMoveOrInteract() == false)
            return;

        // set the yVelocity in the animator
        animator.SetFloat("yVelocity", rb.velocity.y);
        // store horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");
        // if left alt is clicked enabled isRunning
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            isRunning = true;
        // if left alt is realese disable isRunning
        if (Input.GetKeyUp(KeyCode.LeftAlt))
            isRunning = false;
        // if we press jump button enabled jump
        if (Input.GetButtonDown("Jump"))
            Jump();
        // if we press Crouch button enabled jump
        if (Input.GetButtonDown("Crouch"))
            crouchPressed = true;
        // otherwise disable it
        else if (Input.GetButtonUp("Crouch"))
            crouchPressed = false;

    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, crouchPressed);
    }



    bool CanMoveOrInteract()
    {
        bool can = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
            can = false;
        if (FindObjectOfType<InventorySystem>().isOpen)
            can = false;

        return can;
    }


    bool canMove()
    {
        bool can = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
            can = false;
        if (FindObjectOfType<InventorySystem>().isOpen)
            can = false;

        return can;
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        // check if the GroundCheck obj is collinding with other colliders of Ground layer. If yes (isGrounded is true) else (isGrounded is false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                avaiblejumps = totalJumps;
                multipleJump = false;
                //sfx for grounded action
                AudioManager.instance.PlaySFX("landing");
            }
            else
            {
                if(wasGrounded)
                    StartCoroutine(coyoteJumpDelay());
            }
        }
        // As soon as we are grounded the "jump" bool in the animator is disable
        animator.SetBool("Jump", !isGrounded);
    }

    IEnumerator coyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump=false;
    }

    void Jump()
    {
        if (isGrounded)
        {
            multipleJump = true;
            avaiblejumps--;
            //sfx for jump action
            AudioManager.instance.PlaySFX("jump");

            rb.velocity = Vector2.up * jumpPower;
            animator.SetBool("Jump", true);
        }
        else
        {
            if (coyoteJump)
            {
                multipleJump = true;
                avaiblejumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }

            if (multipleJump && avaiblejumps > 0)
            {
                avaiblejumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }
        }
    }

    void Move(float dir, bool crouchFlag)
    {
        #region Crouch

        // if we are crouching and disabled crouching check overhead for collision with Ground items
        if (!crouchFlag)
        {
            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
                crouchFlag = true;
        }
        animator.SetBool("Crouch", crouchFlag);
        standingCollider.enabled = !crouchFlag;
        #endregion
        #region Move & Run
        // set the value of x uxing dir and speed 
        float xVal = dir * speed * Time.fixedDeltaTime;
        // if we are running multiply with the running modifier
        if (isRunning)
            xVal *= runSpeedModifier;
        // if we are running multiply with the running modifier
        if (crouchFlag)
            xVal *= crouchSpeedModifier;
        // create vec2 for the velocity
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        // set the player's velocity
        rb.velocity = targetVelocity;

        // if looking right and clicked left (flip to the left)
        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        // if looking left and clicked right (flip to the right)
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
        // idle 0, 10 walking, 20 running
        //Set the float to the velocity according to the x value of the RigidBody2D velocity
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(overheadCheckCollider.position, overheadCheckRadius);
    }

}

