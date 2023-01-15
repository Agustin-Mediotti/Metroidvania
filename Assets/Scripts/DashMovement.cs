using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMovement : MonoBehaviour
{
    // DASH VARS
    Rigidbody2D rb;
    bool isDashing;
    bool canDash = true;
    float direction = 1;
    float horizontal;
    public Animator animator;

    // DASH COROUTINE
    IEnumerator dashCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // DASH DIRECTION
        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            direction = horizontal;
        }
        // DASH INPUT
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = Dash(.2f, 1);
            StartCoroutine(dashCoroutine);
        }
    }

    private void FixedUpdate()
    {
        // DASH
        if (isDashing)
        {
            animator.SetTrigger("Dash");
            rb.AddForce(new Vector2(direction * 10, 0), ForceMode2D.Impulse);
        }
    }

    // DASH RUNTIME (DURATION & COOLDOWN)
    IEnumerator Dash(float dashDuration, float dashCooldown)
    {
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
