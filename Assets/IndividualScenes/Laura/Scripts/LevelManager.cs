using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        //SaveGameManager.CurrentSessionData.CurrentLevelIndex = 0;
        //Debug.Log($"On Level: {SaveGameManager.Instance.SessionData.LevelInfo[0].IsLocked}");
        SaveGameManager.Instance.SessionData.LevelInfo[0].CarrotList = new List<SaveData.LevelData.Carrot>();
        SaveData.LevelData.Carrot carrot = new SaveData.LevelData.Carrot();
        carrot.IsPicked = true;
        SaveGameManager.Instance.SessionData.LevelInfo[0].CarrotList.Add(carrot);

        SaveGameManager.Instance.SessionData.LevelInfo.Add(new SaveData.LevelData());

        string json = JsonUtility.ToJson(SaveGameManager.Instance.SessionData);
        Debug.Log(json);

        if (SaveGameManager.Instance.SessionData.ChangeScene)
        {
            Debug.Log("Changing scene");
            SaveGameManager.Instance.SessionData.ChangeScene = false;
            SceneManager.LoadScene("HUB");
        }
        
    }

}
