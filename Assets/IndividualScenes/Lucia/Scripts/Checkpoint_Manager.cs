using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Manager : MonoBehaviour
{
    public static Checkpoint_Manager instance;
    public hudManager hudManager;

    public List<Transform> checkpoints_List;
    public int currentIndexCheckpoint = 0;

    public Transform spawnPoint;
    public GameObject tony;
    public float ch1updateJump = 0.1f;
    public float ch2updateJump = 0.02f;
    public float ch3updateJump = 0.03f;

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

        currentIndexCheckpoint = newCheckPointIndex;
        spawnPoint.position = checkpoints_List[currentIndexCheckpoint].position;

        if (currentIndexCheckpoint == 1)
        {
            hudManager.staminaRecharge += ch1updateJump;
        }
        else if (currentIndexCheckpoint == 2)
        {
            hudManager.staminaRecharge += ch2updateJump;
        }
        else if (currentIndexCheckpoint == 3)
        {
            hudManager.staminaRecharge += ch3updateJump;
        }
    }

    public void ReSpawn()
    {
        if (DeathAndRespawnManager.instance.prosesingDeath)
        {
            tony.transform.position = spawnPoint.position;
        }
    }
}