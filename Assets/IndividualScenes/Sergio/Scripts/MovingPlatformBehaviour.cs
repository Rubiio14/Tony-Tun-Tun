using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
    //lugares hacia los que se mueve y velocidad
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _platformMovSpeed = 2.5f;
    private int _currentWaypoint;

    private void Start()
    {
        _currentWaypoint = 0;
    }
    void Update()
    {
        if (transform.position != _waypoints[_currentWaypoint].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _platformMovSpeed * Time.deltaTime);
        }
        else
        {
            _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
        }
    }

    //Que el jugador se haga hijo de la plataforma para que siga el movimiento (o que deje de serlo)
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
