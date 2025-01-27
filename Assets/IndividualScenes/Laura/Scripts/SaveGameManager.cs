using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }
    public static bool IsSessionStarted;

    public SessionData SessionData;
    public SessionData DefaultData;

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

    public int GetTotalPickedCarrots()
    {
        int totalNumberOfCarrots = 0;
        foreach (SessionLevel level in SessionData.SessionLevels)
        {
            foreach (SessionCarrot carrot in level.SessionCarrots)
            {
                if (carrot.IsPicked)
                    totalNumberOfCarrots++;
            }
        }
        return totalNumberOfCarrots;
    }

    public bool IsDataSavedInFile()
    {
        return FileManager.DoesSaveFileExists();
    }

    public void DeleteSaveData()
    {
        if (FileManager.DeleteSavefile())
        {
            IsSessionStarted = false;
        }
    }

    public void SaveSessionDataToFile()
    {
        FileManager.WriteToSaveFile(SessionData.ToJson());
    }

    public void LoadSessionDataFromFile()
    {
        if (FileManager.LoadFromSaveFile(out string json))
        {
            SessionData.LoadFromJson(json);
        }
    }

    public void ResetSessionData()
    {
        SessionData.CurrentLevelIndex = DefaultData.CurrentLevelIndex;
        for(int i = 0; i < SessionData.SessionLevels.Count; i++)
        {
            SessionData.SessionLevels[i].IsLocked = DefaultData.SessionLevels[i].IsLocked;
            for(int j = 0; j < SessionData.SessionLevels[i].SessionCarrots.Count; j++)
            {
                SessionData.SessionLevels[i].SessionCarrots[j].IsPicked = DefaultData.SessionLevels[i].SessionCarrots[j].IsPicked;
            }
        }
        
    }
}
