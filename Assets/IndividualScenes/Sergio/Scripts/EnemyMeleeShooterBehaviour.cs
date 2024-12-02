using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeShooterBehaviour : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField, Range(1f, 20f)][Tooltip("Longitud máxima de detección")] public float raycastLength = 5f;
     public float startRaycast = 0f;

    public Animator meleeShooterAnimator;

    private void Start()
    {
        meleeShooterAnimator = this.GetComponent<Animator>();
    }
    void FixedUpdate()  
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x * startRaycast, 1), Vector2.right, raycastLength);

        Vector2 forward = transform.TransformDirection(Vector2.right) * raycastLength;
        Debug.DrawRay(new Vector2(transform.position.x * startRaycast, 1), forward, Color.green);

        if (hit.collider.gameObject.CompareTag("Player"))
        {
            meleeShooterAnimator.SetBool("playerDetected", true);
            Debug.Log(hit.collider.gameObject.tag);
        }
        else
        {

            meleeShooterAnimator.SetBool("playerDetected", false);
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
