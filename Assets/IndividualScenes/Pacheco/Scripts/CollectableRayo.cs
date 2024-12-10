using UnityEngine;

public class CollectableRayo : ScriptableObject, ICollectable
{


    public float staminaCounter = 0;


    public void Collect()
    {
    }

    public void collectingRayo()
    {
        hudManager.instance.updateRayo(staminaCounter);
    }
}
