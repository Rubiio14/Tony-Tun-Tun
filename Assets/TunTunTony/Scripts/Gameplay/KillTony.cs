using UnityEngine;

public class KillTony : MonoBehaviour
{
    //Matar al jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DeathAndRespawnManager.instance.OnPlayerDeath();
            FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.playerDeath, this.gameObject.transform.position);
            Debug.Log("Instanced position of Tony Ghost is " + DeathAndRespawnManager.instance.tonyGhost.transform.position);
        }
    }
}
