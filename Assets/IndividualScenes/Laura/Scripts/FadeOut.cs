using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private float _elapsedTime;
    [SerializeField] private float _fadeDuration;
    private CanvasGroup _loadingScreen;

    private void Awake()
    {
        _loadingScreen = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        while (_elapsedTime < _fadeDuration)
        {
            _elapsedTime += Time.deltaTime;
            _loadingScreen.alpha = 1 - Mathf.Clamp01(_elapsedTime / _fadeDuration);
        }
    }

}
