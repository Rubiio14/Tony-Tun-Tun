using UnityEngine;
using FMODUnity;
public class FMODAudioManager : MonoBehaviour
{
    [SerializeField] private EventReference m_musica;
    [SerializeField] private GameObject player;
    public static FMODAudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Más de 1 AudioManager en la escena");
        }
        instance = this;
    }
    public void Start()
    {
        PlayOneShot(m_musica, player.transform.position);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
