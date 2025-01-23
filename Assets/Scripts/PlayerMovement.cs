using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpHeight = 3f;

    float direction = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(direction);
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
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
    }

    void OnJump()
    {
        Jump();
        // Debug.Log("boing");
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
    }
}
