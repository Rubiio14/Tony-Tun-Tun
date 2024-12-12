using UnityEngine;

[CreateAssetMenu(fileName = "New Rayo Collectable", menuName = "Collectable/Rayo")]
public class CollectableRayoSO : CollectableSOBase
{
    public float staminaAmount = 0.3f;

    public override void Collect(GameObject objectThatCollected)
    {
        RayoManager.instance.IncrementRayo(staminaAmount);
    }
}
