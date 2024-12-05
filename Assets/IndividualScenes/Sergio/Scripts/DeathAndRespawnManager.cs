using System.Collections;
using UnityEngine;

public class DeathAndRespawnManager : MonoBehaviour
{
    public static DeathAndRespawnManager instance;

    //Canvas y variable para oscurecer pantalla en la muerte del jugador
    [SerializeField] public CanvasGroup deathBackground;
    [SerializeField] public bool playerDeath = false;

    //jugador y fantasma del jugador
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject tonyGhost;

    //Duracion y velocidad de la pantalla de muerte
    [SerializeField] private float fadeOutSpeed = 1f;
    [SerializeField] private float deathAnimationDuration = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
           Destroy(this);
        }
    }
    private void Update()
    {
        if (playerDeath == true)
        {
            tonyGhost.SetActive(true);          
            tonyGhost.transform.position = Vector2.MoveTowards(tonyGhost.transform.position, new Vector2(tonyGhost.transform.position.x, tonyGhost.transform.position.y + 3), deathAnimationDuration * Time.deltaTime);
            StartCoroutine("Death");
        }
        else if (playerDeath == false)
        {
            Respawn();
        }

    }

    IEnumerator Death()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(deathAnimationDuration);
        deathBackground.alpha += 1f * Time.deltaTime * fadeOutSpeed;
        yield return null;
    }

    public void Respawn()
    {
        deathBackground.alpha -= 1f * Time.deltaTime * fadeOutSpeed;
    }
}
