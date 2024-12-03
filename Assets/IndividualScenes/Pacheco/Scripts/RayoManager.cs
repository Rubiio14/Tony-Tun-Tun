using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayoManager : MonoBehaviour
{
    public static RayoManager instance;

    public int currentRayo { get; private set; }

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

    public void IncrementRayo(int amount)
    {
        currentRayo += amount;

        //HUDManager.instanceUpdateText(currentRayo);
    }

}
