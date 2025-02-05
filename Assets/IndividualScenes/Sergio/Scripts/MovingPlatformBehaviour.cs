using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
    //lugares hacia los que se mueve y velocidad
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _platformMovSpeed = 2.5f;
    private int _currentWaypoint;

    //VFX partículas de movimiento
    [SerializeField]
    GameObject _vfxPlatformDust;

    private void Start()
    {
        _currentWaypoint = 0;
        if (_vfxPlatformDust != null )
        {
            _vfxPlatformDust.SetActive(true);
        }
    }
    void FixedUpdate()
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
            playerJuice.instance.myAnimator.SetBool("IsOnPlatform", true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || DeathAndRespawnManager.instance.prosesingDeath == true)
        {
            collision.transform.SetParent(null);
            playerJuice.instance.myAnimator.SetBool("IsOnPlatform", false);
        }
    }
}
