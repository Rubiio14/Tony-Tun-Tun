using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void Start()
    {
        FMODAudioManager.instance.InitializeMusic(FMODEvents.instance.levelMusic);
        FMODAudioManager.instance.SetMusicParameter("Zona", 0);
    }
}
