using UnityEngine;

public class AcidDrop : MonoBehaviour
{
    Rigidbody2D dropRigidbody;

    public Vector3 Direction {  get; set; }
    public float Speed { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dropRigidbody = GetComponent<Rigidbody2D>();
        dropRigidbody.linearVelocity = Direction * Speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        dropRigidbody.linearVelocity = Direction * 0;
        gameObject.SetActive(false);
    }
}
