using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 1f;
    [SerializeField] float sprintMult = 1.5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float dashSpeed = 3f;
    [SerializeField] float doubleJumpCooldownLength = 1f;

    Animator anim;

    float direction = 0;
    bool isGrounded = false;

    bool isDashing = false;
    bool isSprinting = false;

    public float currSpeed;

    public int jumpCount = 0;

    public float doubleJumpCooldownTimer = 0f;

    bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDashing)
        {
            Move(direction);
        }
        if (doubleJumpCooldownTimer > 0)
        {
            doubleJumpCooldownTimer -= Time.deltaTime;
        }

        if((isFacingRight && direction == -1) || (!isFacingRight && direction == 1))
        {
            Flip();
        }
    }

    void OnMove(InputValue value)
    {
        // Vector2 v = value.Get<Vector2>();
        float v = value.Get<float>();
        direction = v;
        // Debug.Log(v);
    }

    void Move(float dir)
    {
        if(isSprinting)
        {
            currSpeed = speed * sprintMult;
        }
        else{
            currSpeed = speed;
        }
        rb.linearVelocity = new Vector2(dir * currSpeed, rb.linearVelocity.y);
        anim.SetBool("isRunning", dir != 0);
    }

    void OnJump()
    {
        if(isGrounded)
        {
            Jump();
            jumpCount++;
            doubleJumpCooldownTimer = doubleJumpCooldownLength;
            // Debug.Log("boing");
        }
        else if(jumpCount < 2 && doubleJumpCooldownTimer <= 0)
        {
            Jump();
            jumpCount++;
            // resetting it does not matter since the timer sets on the first jump anyways
            // doubleJumpCooldownTimer = 0;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
    }

    void OnDash(InputValue val)
    {
        // prevent hitting dash multiple times in a row and conflicts between left and right
        if(!isDashing)
        {
            Dash(val);
        }
    }

    void Dash(InputValue val)
    {
        float dashDir = val.Get<float>();
        // if(Math.Abs(dir) != 1)
        // {
        //     dir = 0;
        // }
        rb.linearVelocity = new Vector2(dashDir * dashSpeed, rb.linearVelocity.y/2);
        isDashing = true;
    }

    void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 newLocalScale = transform.localScale;
        newLocalScale.x *= -1f;
        transform.localScale = newLocalScale;
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            for(int i = 0; i < collision.contactCount; i++)
            {
                if(Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 45f)
                {
                    jumpCount = 0;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            for(int i = 0; i < collision.contactCount; i++)
            {
                if(Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 45f)
                {
                    isGrounded = true;
                    if(rb.linearVelocity.y <= 0)
                    {
                        jumpCount = 0;
                    }
                }
            }
        }
        if(isDashing && !collision.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = new Vector2(0, 0);
            isDashing = false;
        }
        // if the player is on the ground and is downward, set the jump count to 0 to prevent on enter having an issue if you hit the side of something instead
        // if (isGrounded && rb.linearVelocity.y <= 0)
        // {
        //     jumpCount = 0;
        // }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Collectible"))
        {
            Debug.Log("Collectible Collected!");
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Flag"))
        {
            Debug.Log("End Reached!");
            Destroy(collision.gameObject);
            OnRestart();
        }
    }
}
