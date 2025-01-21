using UnityEngine;

public class Level : MonoBehaviour
{
    public int LevelIndex;
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
