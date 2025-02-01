using UnityEngine;

public class Ball : MonoBehaviour
{
    float x;
    float topY;
    float botY;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        topY = transform.position.y;
        botY = transform.position.y - 22f;
        x = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < botY)
        {
            transform.position = new Vector2(x, topY);
            rb.linearVelocity = Vector2.zero;
        }
    }
}
