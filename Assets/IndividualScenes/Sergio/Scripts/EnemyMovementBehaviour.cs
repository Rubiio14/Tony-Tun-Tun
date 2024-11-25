using UnityEngine;

public class EnemyMovementBehaviour : MonoBehaviour
{
    public Transform[] waypoints;
    public float movSpeed = 2.5f;
    int currentWaypoint;
    public bool needRotation;

    private void Start()
    {
        currentWaypoint = 0;
    }
    void Update()
    {
        if (transform.position != waypoints[currentWaypoint].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, movSpeed * Time.deltaTime);
        }
        else
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            if(needRotation == true)
            {
                transform.Rotate(0, 180, 0);
            }
        } 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //ejecutar muerte jugador, llama a DeathAndRespawn 
            DeathAndRespawnManager.instance.playerDeath = true;
        }
    }

}
