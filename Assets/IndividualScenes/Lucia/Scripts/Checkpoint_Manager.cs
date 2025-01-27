using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Manager : MonoBehaviour
{
    public static Checkpoint_Manager instance;
    public DeathAndRespawnManager deathAndRespawnManager;

    public List<Transform> checkpoints_List;
    public int currentIndexCheckpoint = 0;

    public Transform spawnPoint;
    public GameObject tony;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeCheckpointIndex(Transform checkPoint)
    {
        int newCheckPointIndex = checkpoints_List.IndexOf(checkPoint);

        if (newCheckPointIndex != -1 && newCheckPointIndex >= currentIndexCheckpoint)
        {
            currentIndexCheckpoint = newCheckPointIndex;
            spawnPoint.position = checkpoints_List[currentIndexCheckpoint].position;
            Debug.Log($"Checkpoint actualizado a índice {currentIndexCheckpoint}, posición del spawn: {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("Checkpoint no encontrado o ya está activado.");
        }
    }

    public void ReSpawn()
    {
        if (deathAndRespawnManager.playerDeath == true)
        {
            tony.transform.position = spawnPoint.position;
            Debug.Log("Jugador respawneado en: " + spawnPoint.position);
        }
    }
}