using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedLevelController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    [SerializeField] private Image[] _carrots;
    [SerializeField] private Sprite _pickedCarrotImage;
    [SerializeField] private Sprite _notPickedCarrotImage;

    public void UpdateCarrotsForLevel(int carrotsUnlocked)
    {
        for(int i = 0; i < _carrots.Length; i++)
        {
            if(i < carrotsUnlocked)
            {
                _carrots[i].sprite = _pickedCarrotImage;
            }
            else
            {
                _carrots[i].sprite = _notPickedCarrotImage;
            }
        }
    }

    public void ShowCurrentCarrotsInLevel(int carrotsUnlocked, Vector3 position)
    {
        //Canvas show current carrots in level on top of current level, range from 0 to 2
        //Could be animated
        _canvas.gameObject.transform.position = position;
        UpdateCarrotsForLevel(carrotsUnlocked);
        _canvas.gameObject.SetActive(true);
    }

    public void HideCurrentCarrotsInLevel()
    {
        //Canvas hide current carrots in level.
        //Could be animated
        _canvas.gameObject.SetActive(false);
    }
    
}
