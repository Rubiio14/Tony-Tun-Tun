using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Manager : MonoBehaviour
{
    public static Checkpoint_Manager instance;
    public DeathAndRespawnManager deathAndRespawnManager;
    public hudManager hudManager;

    public List<Transform> checkpoints_List;
    public int currentIndexCheckpoint = 0;

    public Transform spawnPoint;
    public GameObject tony;
    public float stValueUpdate = 0.1f;

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
            hudManager.staminaRecharge = hudManager.staminaRecharge + stValueUpdate;
            Debug.Log($"Checkpoint actualizado a �ndice {currentIndexCheckpoint}, posici�n del spawn: {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("Checkpoint no encontrado o ya est� activado.");
        }
    }

    public void ReSpawn()
    {
        if (deathAndRespawnManager.prosesingDeath)
        {
            tony.transform.position = spawnPoint.position;
            Debug.Log("Jugador respawneado en: " + spawnPoint.position);
        }
    }
}