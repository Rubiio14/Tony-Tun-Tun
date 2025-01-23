using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Manager : MonoBehaviour
{
    public static Checkpoint_Manager instance;
    public DeathAndRespawnManager deathAndRespawnManager;

    public List<Transform> checkpoints;
    public int currentIndexCheckpoint;

    Transform spawnPoint;
    public GameObject tony;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ReSpawn()
    {
        if (deathAndRespawnManager.playerDeath == true)
        {
            tony.transform.position = spawnPoint.position;
        }
    }

    public void ChangeCheckpointIndex(Transform checkPoint)
    {
        int newCheckPointIndex = checkpoints.IndexOf(checkPoint);

        if (newCheckPointIndex >= currentIndexCheckpoint)
        {
            currentIndexCheckpoint = newCheckPointIndex;
            spawnPoint = checkpoints[currentIndexCheckpoint];
            //print("esto se esta llamando");
        }
    }

     /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeCheckpointIndex(spawnPoint);
        }
    }
     */
}
