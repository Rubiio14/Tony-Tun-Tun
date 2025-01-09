using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SaveData SavedData { get; private set; }


    internal SaveData LoadData()
    {
        return null;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

}
