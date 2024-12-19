using UnityEngine;

public class EnemyMeleeShooterBehaviour : MonoBehaviour
{
    //Para ajustar la distancia del raycast desde el inspector
    [Header("Detection")]
    [SerializeField, Range(1f, 50f)] [Tooltip("Longitud maxima de deteccion")] public float raycastLength = 5f;

    //Para que el raycast empiece desde el enemigo pero no impacte con su propio collider. No es modificable en el inspectos para ahorrarnos errores.
    private float _startRaycast = 0.9f;

    //Animaciones del shooter
    private Animator _meleeShooterAnimator;
    //Para detectar solo el collider del jugador
    [SerializeField] private LayerMask _playerLayer;

    private void Start()
    {
        _meleeShooterAnimator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        //Rayo Debug en la escena
        Vector2 forward = transform.TransformDirection(Vector2.right) * raycastLength;
        Debug.DrawRay(new Vector2(transform.position.x * _startRaycast, transform.position.y), forward, Color.green);

        //Crear raycast e impactar con el layer del jugador 
        if (Physics2D.Raycast(new Vector2(transform.position.x * _startRaycast, transform.position.y), Vector2.right, raycastLength, _playerLayer))
        {
            //activar animacion de disparo
            _meleeShooterAnimator.SetBool("playerDetected", true);
        }
        else
        {
            //Desactivar animacion de disparo
            _meleeShooterAnimator.SetBool("playerDetected", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //ejecutar muerte jugador, llama al script DeathAndRespawn 
            DeathAndRespawnManager.instance.playerDeath = true;
            DeathAndRespawnManager.instance.tonyGhost.transform.position = new Vector2(DeathAndRespawnManager.instance.player.transform.position.x, DeathAndRespawnManager.instance.player.transform.position.y + 2);

        }
    }
}