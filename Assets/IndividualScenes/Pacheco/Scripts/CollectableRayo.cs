using UnityEngine;

public class CollectableRayo : MonoBehaviour, ICollectable
{


    public int totalRayo = 0;


    public void Collect()
    {
        totalRayo++;
        Destroy(gameObject);
    }

    public void collectingRayo()
    {       

    }
}
