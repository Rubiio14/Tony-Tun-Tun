using UnityEngine;

[CreateAssetMenu(fileName = "New Shoe Collectable", menuName = "Collectable/Shoe")]
public class CollectableShoeSO : CollectableSOBase
{
    public override void Collect(GameObject objectThatCollected)
    {
        CollectablesManager.instance.IncrementShoe();

    }
}