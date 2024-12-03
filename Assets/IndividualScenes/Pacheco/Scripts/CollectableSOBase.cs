using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "collectableSOBase", menuName = "Scriptable Objects/collectableSOBase")]
public abstract class CollectableSOBase : ScriptableObject
{
    public abstract void Collect(GameObject objectThatCollected);
}
