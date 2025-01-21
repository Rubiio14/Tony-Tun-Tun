using UnityEngine;

public class skullbunnycesta : MonoBehaviour
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
    }
}
