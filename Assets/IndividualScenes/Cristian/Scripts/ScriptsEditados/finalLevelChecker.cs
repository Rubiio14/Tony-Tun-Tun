using UnityEngine;
using TMPro;
public class finalLevelChecker : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _CarrotUnlocked1;
    [SerializeField] private GameObject _CarrotUnlocked2;
    [SerializeField] private GameObject _CarrotUnlocked3;
    [SerializeField] private GameObject _Bloqueo;
    [SerializeField] private bool _Unlocked = false;

    [SerializeField] Animator doorController;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        ShowNumberOfCarrots(transform.position);

        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Game_End"))
        {
            doorController.SetBool("OpenDoor", true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        HideNumberOfCarrots();
        doorController.SetBool("OpenDoor", false);
    }

    public void UpdateCarrotNumber()
    {
        if (hudManager.instance.carrotsCounter >= 1)
        {
            _CarrotUnlocked1.SetActive(true);
            if (hudManager.instance.carrotsCounter >= 2)
            {
                _CarrotUnlocked2.SetActive(true);
                if (hudManager.instance.carrotsCounter == 3)
                {
                    _CarrotUnlocked3.SetActive(true);                   
                }
            }
        }
    }

    public void ShowNumberOfCarrots(Vector3 position)
    {
        //Canvas show carrotsToUnlock on top of current level.
        _canvas.gameObject.SetActive(true);
        _canvas.transform.position = position;
        UpdateCarrotNumber();
    }

    public void HideNumberOfCarrots()
    {
        //Canvas hide carrotsToUnlock.
        //Could be animated
        _canvas.gameObject.SetActive(false);
    }
}
