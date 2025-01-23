using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "New Carrot Collectable", menuName = "Collectable/Carrot")]
public class CollectableCarrotSO : CollectableSOBase
{

    public override void Collect(GameObject objectThatCollected)
    {
        CollectablesManager.instance.IncrementCarrot();
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.zanahoriaCollectedSound, objectThatCollected.transform.position);
    }
}
