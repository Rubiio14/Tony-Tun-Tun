using UnityEngine;

[CreateAssetMenu(fileName = "New Carrot Collectable", menuName = "Collectable/Carrot")]
public class CollectableCarrotSO : CollectableSOBase
{
    public override void Collect(GameObject objectThatCollected, int index)
    {
        CollectablesManager.instance.IncrementCarrot();
        SaveGameManager.Instance.SessionData.SessionLevels[SaveGameManager.Instance.SessionData.CurrentLevelIndex].SessionCarrots[index].IsPicked = true;
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.zanahoriaCollectedSound, objectThatCollected.transform.position);
    }
}
