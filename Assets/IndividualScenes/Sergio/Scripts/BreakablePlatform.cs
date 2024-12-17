using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Temblar();
        }
    }
    void Temblar()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 1);


    }
}
