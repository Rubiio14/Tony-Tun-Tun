using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "New Rayo Collectable", menuName = "Collectable/Rayo")]
public class CollectableRayoSO : CollectableSOBase
{
    public float staminaAmount = 0.3f;
    

    public override void Collect(GameObject objectThatCollected, int index)
    {
        CollectablesManager.instance.IncrementRayo(staminaAmount);
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.rayoCollectedSound, objectThatCollected.transform.position);
    }
}
