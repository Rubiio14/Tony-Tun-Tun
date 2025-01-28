using System.Collections;
using UnityEngine;

public class DeathAndRespawnManager : MonoBehaviour
{
    public static DeathAndRespawnManager instance;

    [SerializeField] public Checkpoint_Manager checkpoint_Manager;

    [SerializeField] public CanvasGroup deathBackground;
    [SerializeField] public bool prosesingDeath = false;
    
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject tonyGhost;

    [SerializeField] private float fadeInOutSpeed = 1f;
    [SerializeField] private float deathAnimationDuration = .1f;
    [SerializeField] private float secondsToRespawn = 2f;

    [SerializeField] float waitForFade = 2f;
    [SerializeField] float whereToMoveGhost = 20f;


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

    public void OnPlayerDeath()
    {
        Debug.Log("Jugador ha muerto, iniciando proceso de muerte.");
        prosesingDeath = true;
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        Debug.Log("Comienza la animación de muerte.");

        Vector2 playerPosition = player.transform.position;
        float clampedY = Mathf.Max(playerPosition.y, 10f);
        tonyGhost.transform.position = new Vector2(playerPosition.x, clampedY);

        player.SetActive(false);
        tonyGhost.SetActive(true);

        Vector2 targetPosition = new Vector2(playerPosition.x, playerPosition.y + whereToMoveGhost);
        float elapsedTime = 0f;

        while (elapsedTime < deathAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            tonyGhost.transform.position = Vector2.Lerp(playerPosition, targetPosition, elapsedTime / deathAnimationDuration);
            yield return null;
        }

        Debug.Log("Animación de muerte completada, comenzando fade-in.");

        yield return StartCoroutine(FadeCanvasGroup(deathBackground, 1f, fadeInOutSpeed));

        yield return new WaitForSeconds(secondsToRespawn);

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Debug.Log("Responiendo al jugador en el último checkpoint.");
        checkpoint_Manager.ReSpawn();
        FadeOut();

        Debug.Log("Jugador respawneado y listo para continuar.");
    }

    private void FadeOut()
    {
        Debug.Log("Comenzando fade-out.");
        StartCoroutine(FadeCanvasGroup(deathBackground, 0f, fadeInOutSpeed));

        prosesingDeath = false;

        player.SetActive(true);
        tonyGhost.SetActive(false);

        Debug.Log("FadeOut completado, fondo restablecido.");
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float endAlpha, float duration)
    {
        yield return new WaitForSeconds(waitForFade);
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += .1f;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime);
            yield return new WaitForSeconds(.05f);
        }

        canvasGroup.alpha = endAlpha;
    }
}