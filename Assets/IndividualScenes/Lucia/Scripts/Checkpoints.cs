using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Checkpoints : MonoBehaviour
{
    public Checkpoint_Manager checkpoint_Manager;

    public GameObject checkpointPainted;
    public GameObject checkpointNotPainted;

    private void Start()
    {
        checkpointPainted.SetActive(false);
        checkpointNotPainted.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkpoint_Manager.ChangeCheckpointIndex(transform);

            checkpointPainted.SetActive(true);
            checkpointNotPainted.SetActive(false);

            Debug.Log("Checkpoint activado.");
            //SFX
        }
    }
}