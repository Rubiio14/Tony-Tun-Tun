using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateText(int currentRayo)
    {
        _text.text = currentRayo.ToString();
    }
}
