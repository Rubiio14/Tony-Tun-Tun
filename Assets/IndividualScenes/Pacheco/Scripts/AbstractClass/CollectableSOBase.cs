using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CollectableSOBase", menuName = "Scriptable Objects/CollectableSOBase")]
public abstract class CollectableSOBase : ScriptableObject
{
    public abstract void Collect(GameObject objectThatCollected);
}
