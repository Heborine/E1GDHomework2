using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float x = 6.6f;
    [SerializeField] float topY = 13f;
    [SerializeField] float botY = -9f;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
