using UnityEngine;

public class endLevelLoadScene : MonoBehaviour
{
    public bool _firstTime = true;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _firstTime)
        {
            FMODAudioManager.instance.StopMusic();
            _firstTime = false;
            StartCoroutine(UIManager.Instance.LoadScene("HUB"));
        }
    }
}
