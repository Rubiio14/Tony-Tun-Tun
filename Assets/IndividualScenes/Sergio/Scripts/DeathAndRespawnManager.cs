using UnityEngine;

public class DeathAndRespawnManager : MonoBehaviour
{
    public static DeathAndRespawnManager instance;

    [SerializeField]
    CanvasGroup deathBackground;
    public bool playerDeath = false;
    public float fadeOutSpeed = 1f;

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
            Death();
        }
        else if (playerDeath == false)
        {
            Respawn();
        }
    }

    public void Death()
    {
        deathBackground.alpha += 1f * Time.deltaTime * fadeOutSpeed;
    }
    public void Respawn()
    {
        deathBackground.alpha -= 1f * Time.deltaTime * fadeOutSpeed;
    }
}
