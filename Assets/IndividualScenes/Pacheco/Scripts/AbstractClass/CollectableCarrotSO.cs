using UnityEngine;

[CreateAssetMenu(fileName = "New Carrot Collectable", menuName = "Collectable/Carrot")]
public class CollectableCarrotSO : CollectableSOBase
{

    public override void Collect(GameObject objectThatCollected)
    {
        CollectablesManager.instance.IncrementCarrot();
    }
}
