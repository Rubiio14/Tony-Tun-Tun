using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
    public Transform[] waypoints;
    public float platformMovSpeed = 2.5f;
    int currentWaypoint;

    private void Start()
    {
        currentWaypoint = 0;
    }
    void Update()
    {
        if (transform.position != waypoints[currentWaypoint].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, platformMovSpeed * Time.deltaTime);
        }
        else
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
