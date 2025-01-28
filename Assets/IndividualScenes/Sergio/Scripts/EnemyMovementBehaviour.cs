using UnityEngine;

public class EnemyMovementBehaviour : MonoBehaviour
{
    //lugares hacia los que se mueve y velocidad
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _movSpeed = 2.5f;
    private int currentWaypoint;

    //Si es volador no activara la rotacion. Si no lo es, rotará al lado contrario
    [SerializeField] private bool enemyFlying;
    private bool needRotation;

    private void Start()
    {
        currentWaypoint = 0;
    }
    void Update()
    {
        if (transform.position != _waypoints[currentWaypoint].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[currentWaypoint].position, _movSpeed * Time.deltaTime);
        }
        else
        {
            if (enemyFlying == false)
            {
                needRotation = true;
            }
            else
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y * -1, transform.rotation.z, transform.rotation.w);
            }
            currentWaypoint = (currentWaypoint + 1) % _waypoints.Length;
            if (needRotation == true)
            {
                transform.Rotate(0, 180, 0);
                needRotation = false;
            }
        }
    }
    //Matar al jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DeathAndRespawnManager.instance.OnPlayerDeath();

            Debug.Log("Instanced position of Tony Ghost is " + DeathAndRespawnManager.instance.tonyGhost.transform.position);
        }
    }
}