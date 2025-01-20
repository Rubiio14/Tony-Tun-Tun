using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<LevelData> Levels;
    public int LastLevelVisited;

    [System.Serializable]
    public class LevelData
    {
        //Carrot identifier
        public int Index;
        public bool IsLocked;
        
        public class Carrot {
            public int Index;
            public bool IsPicked;
        }

        public List<Carrot> CarrotList;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string jSon)
    {
        JsonUtility.FromJsonOverwrite(jSon, this);
    }
}