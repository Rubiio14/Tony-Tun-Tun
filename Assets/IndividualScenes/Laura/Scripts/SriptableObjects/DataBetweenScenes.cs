using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBetweenScenes", menuName = "Scriptable Objects/DataBetweenScenes")]
[System.Serializable]

public class DataBetweenScenes : ScriptableObject, ISaveable
{
    public List<Level> LevelInfo;
    public int CurrentLevelIndex;
    public int TotalNumberOfCarrots;

    /*public int CarrotsUnlockedInLevel()
    {
        return LevelInfo[CurrentLevelIndex].CarrotsUnlocked.Count;
    }

    public void SaveCarrotInLevel(int carrot)
    {
        LevelInfo[CurrentLevelIndex].CarrotsUnlocked[carrot] = new Carrot(carrot, true);
    }*/

    public void LoadDataFromSavedata(SaveData dataInFile)
    {
        //Fills Data Between Scenes information from a save file in System.
        
    }

    public void LoadDataFromHUBState()
    {
        //Find levels in scene fill DataBetweenScenes
    }

    public void Initialize()
    {
        LevelInfo = new List<Level>();
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
            PlayableLevel playableLevel = LevelInfo[i] as PlayableLevel;
            if (playableLevel)
            {
                levelData.IsLocked = playableLevel.IsLocked;
                levelData.CarrotList = new List<SaveData.LevelData.Carrot>();

                for (int j = 0; j < playableLevel.CarrotsUnlocked.Count; j++)
                {
                    SaveData.LevelData.Carrot carrotsData = new SaveData.LevelData.Carrot();
                    carrotsData.Index = playableLevel.CarrotsUnlocked[j].Index;
                    carrotsData.IsPicked = playableLevel.CarrotsUnlocked[j].IsPicked;
                    levelData.CarrotList.Add(carrotsData);
                }
            }
            saveData.Levels.Add(levelData);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        //Levels were saved
        if(saveData.Levels.Count > 0)
        {
            LevelInfo = new List<Level>();
            CurrentLevelIndex = saveData.LastLevelVisited;
            int numberOfCarrots = 0;

            List<Level> levelInScene = GameObject.FindObjectsByType<Level>(FindObjectsSortMode.None).OrderBy(level => level.LevelIndex).ToList();
            //Iterate over the levels in the scene unordered
            for (int i = 0; i < levelInScene.Count; i++)
            {
                //If index of saveddata matches index of playable level, load data from save data.
                SaveData.LevelData levelData = saveData.Levels[i];
                PlayableLevel level = levelInScene[i] as PlayableLevel;
                if (level)
                {
                    level.IsLocked = levelData.IsLocked;
                    level.CarrotsUnlocked = new List<Carrot>();
                    if (levelData.CarrotList != null)
                    {
                        for (int j = 0; j < levelData.CarrotList.Count; j++)
                        {
                            level.CarrotsUnlocked.Add(new Carrot(levelData.CarrotList[j].Index, levelData.CarrotList[j].IsPicked));
                            if (levelData.CarrotList[j].IsPicked)
                            {
                                numberOfCarrots += 1;
                                //TODO: Revisit
                            }

                        }
                    }
                }
            }
            LevelInfo = levelInScene;
            TotalNumberOfCarrots = numberOfCarrots;
        }
    }
}
