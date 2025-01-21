using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int LastLevelVisited;
    public List<LevelData> Levels;

    [System.Serializable]
    public class LevelData
    {
        public int Index;
        public bool IsLocked;
        public List<Carrot> CarrotList;

        [System.Serializable]
        public class Carrot {
            public int Index;
            public bool IsPicked;
        }
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