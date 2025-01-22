using UnityEngine;

//Save system implementation from https://www.youtube.com/watch?v=uD7y4T4PVk0
public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }

    public SessionData SessionData;

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

    public bool IsDataSaved()
    {
        return FileManager.DoesSaveFileExists();
    }

    public void DeleteSaveData()
    {
        if (FileManager.DeleteSavefile())
        {
            Debug.Log("Delete successful");
        }
    }

    public void SaveSessionDataToFile()
    {
        if (FileManager.WriteToSaveFile(SessionData.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadSessionDataFromFile()
    {
        if (FileManager.LoadFromSaveFile(out var json))
        {
            Debug.Log(json);

            Debug.Log("Load successful");
        }
    }

}
