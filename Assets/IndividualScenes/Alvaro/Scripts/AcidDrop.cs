using UnityEngine;

public class AcidDrop : MonoBehaviour
{
    Rigidbody dropRigidbody;

    public Vector3 Direction {  get; set; }
    public float Speed { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dropRigidbody = GetComponent<Rigidbody>();
        dropRigidbody.linearVelocity = Direction * Speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        dropRigidbody.linearVelocity = Direction * 0;
        gameObject.SetActive(false);
    }
}
