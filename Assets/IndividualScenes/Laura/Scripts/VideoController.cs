using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoController : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    [SerializeField] private string _sceneToLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.SetDirectAudioVolume(0, FMODAudioManager.instance.musicVolume);
        _videoPlayer.loopPointReached += OnMovieFinished;
    }

    //the action on finish
    void OnMovieFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    private void OnDisable()
    {
        _videoPlayer.loopPointReached -= OnMovieFinished;
    }
}
