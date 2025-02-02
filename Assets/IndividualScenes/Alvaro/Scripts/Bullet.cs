using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRigidbody;

    public Vector3 Direction { get; set; }
    public float Speed { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.gravityScale = 0;
        bulletRigidbody.linearVelocity = Direction * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bulletRigidbody.linearVelocity = Direction * 0;
        gameObject.SetActive(false);
    }
}
