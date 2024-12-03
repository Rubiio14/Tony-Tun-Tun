using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Collectable Rayo", menuName = "Collectable/Rayo")]
public class CollectableRayoSO : CollectableSOBase
{
    [Header("Collectable Stats")]
    public int rayoAmount = 1;
    public override void Collect(GameObject objectThatCollected)
    {
        RayoManager.instance.IncrementRayo(rayoAmount);
    }
}
