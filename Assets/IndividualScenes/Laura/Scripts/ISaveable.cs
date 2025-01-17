using UnityEngine;

public interface ISaveable
{
    public void PopulateSaveData(SaveData saveData);
    public void LoadFromSaveData(SaveData saveData);
}
