using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        //SaveGameManager.CurrentSessionData.CurrentLevelIndex = 0;
        //Debug.Log($"On Level: {SaveGameManager.Instance.SessionData.LevelInfo[0].IsLocked}");

        SaveGameManager.Instance.SessionData.SessionLevels[1].SessionCarrots[0].IsPicked = true;

        /*string json = JsonUtility.ToJson(SaveGameManager.Instance.SessionData);
        Debug.Log(json);*/

        SceneManager.LoadScene("HUB");
        /*if (SaveGameManager.Instance.SessionData.ChangeScene)
        {
            Debug.Log("Changing scene");
            SaveGameManager.Instance.SessionData.ChangeScene = false;
            SceneManager.LoadScene("HUB");
        }*/

    }

}
