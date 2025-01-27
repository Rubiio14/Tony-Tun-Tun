using System.Collections;
using UnityEngine;

public class DeathAndRespawnManager : MonoBehaviour
{
    public static DeathAndRespawnManager instance;

    [SerializeField] public Checkpoint_Manager checkpoint_Manager;

    [SerializeField] public CanvasGroup deathBackground;
    [SerializeField] public bool playerDeath = false;
    [SerializeField] public bool readyToRespawn = false;

    [SerializeField] public GameObject player;
    [SerializeField] public GameObject tonyGhost;

    [SerializeField] private float fadeInOutSpeed = 1f;
    [SerializeField] private float deathAnimationDuration = 3f;
    [SerializeField] private float secondsToRespawn = 2f;

    private float time;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (playerDeath)
        {
            tonyGhost.SetActive(true);
            tonyGhost.transform.position = Vector2.MoveTowards
            (
                tonyGhost.transform.position,
                new Vector2(tonyGhost.transform.position.x, tonyGhost.transform.position.y + 3),
                deathAnimationDuration * Time.deltaTime
            );

            StartCoroutine(Death());
            time += Time.deltaTime;

            if (time >= secondsToRespawn)
            {
                readyToRespawn = true;

                if (readyToRespawn)
                {
                    RespawnPlayer();
                }
            }
        }
        else
        {
            deathBackground.alpha = 0f;
        }
    }

    IEnumerator Death()
    {
        player.SetActive(false);

        yield return new WaitForSeconds(deathAnimationDuration);

        while (deathBackground.alpha < 1f)
        {
            deathBackground.alpha += Time.deltaTime * fadeInOutSpeed;

            if (!playerDeath)
            {
                yield break;
            }

            yield return null;
        }
    }

    private void RespawnPlayer()
    {
        checkpoint_Manager.ReSpawn();

        FadeOut();

        playerDeath = false;
        readyToRespawn = false;
        time = 0f;

        Debug.Log("Jugador respawneado y listo para continuar.");
    }

    public void FadeOut()
    {
        deathBackground.alpha = 0f;

        player.SetActive(true);
        tonyGhost.SetActive(false);

        Debug.Log("FadeOut completado, fondo restablecido.");
    }
}