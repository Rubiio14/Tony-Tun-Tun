using UnityEngine;

[CreateAssetMenu(fileName = "New Shoe Collectable", menuName = "Collectable/Shoe")]
public class CollectableShoeSO : CollectableSOBase
{
    public override void Collect(GameObject objectThatCollected, int index)
    {
        CollectablesManager.instance.IncrementShoe();
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.zapatoCollectedSound);
    }
}