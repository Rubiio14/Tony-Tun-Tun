using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [field: SerializeField] public bool IsTransitable { get; private set; }
    [field: SerializeField] public bool IsAccessible { get; private set; }
    [field: SerializeField] public Vector3 Location { get; private set; }

    [SerializeField] private float _delay;
    [SerializeField] private string _sceneName;
    [SerializeField] private AudioClip _selectionAudio;
    [SerializeField] private ParticleSystem _particleSystem;

    void Start()
    {
        Location = transform.position;
    }

    public void SelectLevel()
    {
        StartCoroutine(LoadLevel(_sceneName));
        //AudioManager.Instance.PlaySFX(_selectionAudio);
        //_particleSystem.gameObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        //_particleSystem.Play();
    }

    private IEnumerator LoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }

}
