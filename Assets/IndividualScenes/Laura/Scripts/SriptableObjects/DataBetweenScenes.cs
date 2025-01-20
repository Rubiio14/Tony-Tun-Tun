using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBetweenScenes", menuName = "Scriptable Objects/DataBetweenScenes")]
[System.Serializable]

public class DataBetweenScenes : ScriptableObject, ISaveable
{
    [SerializedDictionary("LevelIndex", "PLayableLevel")]
    public SerializedDictionary<int, PlayableLevel> LevelInfo;
    public int CurrentLevelIndex;
    public int TotalNumberOfCarrots;

    public int CarrotsUnlockedInLevel()
    {
        return LevelInfo[CurrentLevelIndex].CarrotsUnlocked.Count;
    }

    public void SaveCarrotInLevel(int carrot)
    {
        LevelInfo[CurrentLevelIndex].CarrotsUnlocked[carrot] = new Carrot(carrot, true);
    }

    public void LoadDataFromSavedata(SaveData dataInFile)
    {
        //Fills Data Between Scenes information from a file in System.
    }

    public void LoadDataFromHUBState()
    {

    }

    public void Initialize()
    {
        LevelInfo = new SerializedDictionary<int, PlayableLevel>();
        CurrentLevelIndex = 0;
        TotalNumberOfCarrots = 0;
    }

    public void PopulateSaveData(SaveData saveData)
    {
        saveData.Levels = new List<SaveData.LevelData>();
        saveData.LastLevelVisited = CurrentLevelIndex;

        for (int i = 0; i < LevelInfo.Count; i++)
        {
            SaveData.LevelData levelData = new SaveData.LevelData();
            levelData.Index = LevelInfo[i].LevelIndex;
            levelData.IsLocked = LevelInfo[i].IsLocked;
            levelData.CarrotList = new List<SaveData.LevelData.Carrot>();

            for (int j = 0; j < LevelInfo[i].CarrotsUnlocked.Count; j++)
            {
                SaveData.LevelData.Carrot carrotsData = new SaveData.LevelData.Carrot();
                carrotsData.Index = LevelInfo[i].CarrotsUnlocked[j].Index;
                carrotsData.IsPicked = LevelInfo[i].CarrotsUnlocked[j].IsPicked;
                levelData.CarrotList.Add(carrotsData);
            }

            saveData.Levels.Add(levelData);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        //Levels were saved
        if(saveData.Levels.Count > 0)
        {
            LevelInfo = new SerializedDictionary<int, PlayableLevel>();
            CurrentLevelIndex = saveData.LastLevelVisited;
            int numberOfCarrots = 0;

            PlayableLevel[] playableLevelsInScene = GameObject.FindObjectsByType<PlayableLevel>(FindObjectsSortMode.None);
            //Iterate over the levels in the scene unordered
            for (int i = 0; i < playableLevelsInScene.Length; i++)
            {
                //If index of saveddata matches index of playable level, load data from save data.
                SaveData.LevelData levelData = saveData.Levels[playableLevelsInScene[i].LevelIndex];
                playableLevelsInScene[i].IsLocked = levelData.IsLocked;
                playableLevelsInScene[i].CarrotsUnlocked = new List<Carrot>();
                if (levelData.CarrotList != null)
                {
                    for (int j = 0; j < levelData.CarrotList.Count; j++)
                    {
                        playableLevelsInScene[i].CarrotsUnlocked.Add(new Carrot(levelData.CarrotList[j].Index, levelData.CarrotList[j].IsPicked));
                        if (levelData.CarrotList[j].IsPicked)
                        {
                            numberOfCarrots += 1;
                            //TODO: Revisit
                        }

                    }
                }
                LevelInfo[playableLevelsInScene[i].LevelIndex] = playableLevelsInScene[i];
            }
            TotalNumberOfCarrots = numberOfCarrots;
        }
    }
}
