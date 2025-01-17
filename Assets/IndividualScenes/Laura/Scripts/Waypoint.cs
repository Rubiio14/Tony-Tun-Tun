using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waypoint : MonoBehaviour
{
    public float _yOffsetTop;
    public float _yOffsetMid;
    public float _yOffsetFloor;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HubManager.Instance.MarkAsPlatformArrival();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HubManager.Instance.MarkAsPlatformDeparture();
        }
    }
}
