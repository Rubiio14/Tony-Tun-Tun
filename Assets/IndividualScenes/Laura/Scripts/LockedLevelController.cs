using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class LockedLevelController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _text;

    public void UpdateCarrotNumber(int numberOfCarrots)
    {
        _text.text = numberOfCarrots.ToString();
    }

    public void ShowNumberOfCarrotsToUnlock(int carrotsToUnlock, Vector3 position)
    {
        //Canvas show carrotsToUnlock on top of current level.
        _canvas.gameObject.SetActive(true);
        _canvas.transform.position = position;
        UpdateCarrotNumber(carrotsToUnlock);
    }
    
    public void HideNumberOfCarrotsToUnlock()
    {
        //Canvas hide carrotsToUnlock.
        //Could be animated
        _canvas.gameObject.SetActive(false);
    }

}
