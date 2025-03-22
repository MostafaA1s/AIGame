using UnityEngine;

public class projectile : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.left * speed, ForceMode2D.Impulse);

    }

}
