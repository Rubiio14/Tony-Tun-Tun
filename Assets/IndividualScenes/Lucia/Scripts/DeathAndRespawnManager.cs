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
        prosesingDeath = true;
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        float positionInZ = 3.25f;
        float positionInY = 3f;

        Vector3 playerPosition = player.transform.position;
        Vector3 tonyGhostPosition = new Vector3 (playerPosition.x, playerPosition.y + positionInY, playerPosition.z - positionInZ);
        float clampedY = Mathf.Max(playerPosition.y, 10f);
        //tonyGhost.transform.position = new Vector3(playerPosition.x, clampedY, positionInZ);

        player.SetActive(false);
        tonyGhost.SetActive(true);

        Vector3 targetPosition = new Vector3 (tonyGhostPosition.x, tonyGhostPosition.y + whereToMoveGhost, tonyGhostPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < deathAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            tonyGhost.transform.position = Vector3.Lerp (tonyGhostPosition, targetPosition, elapsedTime / deathAnimationDuration);
            yield return null;
        }


        yield return StartCoroutine(FadeCanvasGroup(deathBackground, 1f, fadeInOutSpeed));

        yield return new WaitForSeconds(secondsToRespawn);

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        checkpoint_Manager.ReSpawn();
        FadeOut();

    }

    private void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(deathBackground, 0f, fadeInOutSpeed));

        prosesingDeath = false;

        player.SetActive(true);
        tonyGhost.SetActive(false);

    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float endAlpha, float duration)
    {
        yield return new WaitForSeconds(waitForFade);
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += 0.1f;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime);
            yield return new WaitForSeconds(0.05f);
        }

        canvasGroup.alpha = endAlpha;
    }
}