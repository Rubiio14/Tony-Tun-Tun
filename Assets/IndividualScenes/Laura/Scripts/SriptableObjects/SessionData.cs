using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SessionData", menuName = "Scriptable Objects/SessionData")]
[System.Serializable]
public class SessionData : ScriptableObject
{
    public List<SaveData.LevelData> LevelInfo;
    public int CurrentLevelIndex;
    public int TotalNumberOfCarrots;

    public bool ChangeScene;
}
