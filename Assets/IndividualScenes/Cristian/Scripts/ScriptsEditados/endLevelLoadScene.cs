using UnityEngine;

public class endLevelLoadScene : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(UIManager.Instance.LoadScene("HUB"));
            
        }
    }
}
