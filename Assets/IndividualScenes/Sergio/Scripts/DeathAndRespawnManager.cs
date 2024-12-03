using System.Collections;
using UnityEngine;

public class DeathAndRespawnManager : MonoBehaviour
{
    public static DeathAndRespawnManager instance;

    [SerializeField]
    CanvasGroup deathBackground;
    public bool playerDeath = false;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject tonyGhost;

    public float fadeOutSpeed = 1f;
    public float deathAnimationDuration = 3;

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
            StartCoroutine("Death");
        }
        else if (playerDeath == false)
        {
            Respawn();
        }
    }

    IEnumerator Death()
    {
        tonyGhost.transform.position = player.transform.position;
        Destroy(player);
        tonyGhost.SetActive(true);
        yield return new WaitForSeconds(deathAnimationDuration);
        deathBackground.alpha += 1f * Time.deltaTime * fadeOutSpeed;
        yield return null;
    }

    public void Respawn()
    {
        deathBackground.alpha -= 1f * Time.deltaTime * fadeOutSpeed;
    }
}
