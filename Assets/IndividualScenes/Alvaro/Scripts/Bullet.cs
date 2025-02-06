using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRigidbody;

    [field: SerializeField] public Vector3 Direction { get; set; }
    [field:SerializeField]public float Speed { get; set; }


    private void OnEnable()
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
