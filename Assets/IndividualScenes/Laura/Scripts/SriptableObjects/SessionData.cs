using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SessionData", menuName = "Scriptable Objects/SessionData")]
[System.Serializable]
public class SessionData : ScriptableObject
{
    public List<SessionLevel> SessionLevels;
    public int CurrentLevelIndex;
    public float MusicVolume;
    public float SFXVolume;
    public string Locale;
    public SessionResolution SessionResolution;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string jSon)
    {
        JsonUtility.FromJsonOverwrite(jSon, this);
    }
}
