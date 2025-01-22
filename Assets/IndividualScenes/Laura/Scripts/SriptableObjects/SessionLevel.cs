using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionLevel
{
    public int Identifier;
    public bool isPlayable;
    public bool IsLocked;
    public int NumberOfCarrotsToUnlock;
    public string SceneName;
    public Vector3 Position;
    public List<SessionCarrot> SessionCarrots;
}