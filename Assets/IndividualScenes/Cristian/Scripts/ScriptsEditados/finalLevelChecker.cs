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


    public void OnTriggerEnter2D(Collider2D collision)
    {

        ShowNumberOfCarrotsToUnlock(transform.position);
        if (collision.CompareTag("Player"))
        {
            if (hudManager.instance.carrotsCounter == 3)
            {
                Debug.Log("Final");
            }           
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {

        HideNumberOfCarrotsToUnlock();
        //Animacion se cierra
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
                    DesactivateDoor();
                    //Animación se abre
                }
            }
            
        }
        
    }

    public void DesactivateDoor()
    {
        _Bloqueo.SetActive(false);
        _Unlocked = false;
    }
    public void ShowNumberOfCarrotsToUnlock(Vector3 position)
    {
        //Canvas show carrotsToUnlock on top of current level.
        _canvas.gameObject.SetActive(true);
        _canvas.transform.position = position;
        UpdateCarrotNumber();
    }

    public void HideNumberOfCarrotsToUnlock()
    {
        //Canvas hide carrotsToUnlock.
        //Could be animated
        _canvas.gameObject.SetActive(false);
    }
}
