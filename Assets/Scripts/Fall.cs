using UnityEngine;

public class Fall : MonoBehaviour
{
    // bool isFalling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (GetComponent<Rigidbody2D>() == null)
        // {
        //     isFalling = false;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // only add if it doesn't already exist
            if (GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;
                // isFalling = true;
            }
        }
    }
}
