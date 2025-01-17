using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : ISaveable
{
    public int _lastLevelVisited;

    [System.Serializable]
    public struct LevelData
    {
        //Carrot identifier
        public int Index;
        public bool IsLocked;
        
        public struct Carrot {
            public int Index;
            public bool IsPicked;
        }

        public List<Carrot> CarrotList;
            
    }

    public List<LevelData> Levels = new List<LevelData>();

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string jSon)
    {
        JsonUtility.FromJsonOverwrite(jSon, this);
    }

    public void PopulateSaveData(SaveData saveData)
    {
        
        
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        //
    }
}