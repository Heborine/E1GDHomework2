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

    float direction = 0;
    bool isGrounded = false;

    bool isDashing = false;
    bool isSprinting = false;

    public float currSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // // only reset dash if you stay still
        // if(rb.linearVelocity.x == 0 && rb.linearVelocity.y == 0)
        // {
        //     isDashing = false;
        // }
        if(!isDashing)
        {
            Move(direction);
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
    }

    void OnJump()
    {
        if(isGrounded)
        {
            Jump();
            // Debug.Log("boing");
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

    void OnCollisionEnter2D(Collision2D collision) 
    {
        
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
                }
            }
        }
        if(isDashing)
        {
            rb.linearVelocity = new Vector2(0, 0);
            isDashing = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
