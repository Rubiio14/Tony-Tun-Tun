using UnityEngine;

public class EnemyMeleeShooterBehaviour : MonoBehaviour
{
    public Animator meleeShooterAnimator;
    [SerializeField]
    GameObject triggerEmpty;
    private void Start()
    {
        meleeShooterAnimator = this.GetComponent<Animator>();
    }
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //ejecutar muerte jugador, llama a DeathAndRespawn 
            DeathAndRespawnManager.instance.playerDeath = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            meleeShooterAnimator.SetBool("playerDetected", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            meleeShooterAnimator.SetBool("playerDetected", false);
        }
    }
}
