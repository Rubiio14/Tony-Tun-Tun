using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct LevelData
    {
        //Carrot identifier
        public string _uuid;
        public bool _locked;
        public List<string> _carrotsUuid;
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

    public interface ISaveable
    {
        void PopulateSaveData(SaveData saveData);
        void LoadFromSaveData(SaveData saveData);

    }

}