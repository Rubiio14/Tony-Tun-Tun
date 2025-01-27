using UnityEngine;
using System;


[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent(typeof(CollectableTriggerHandler))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSOBase _collectable;
    public int Index;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void Collect(GameObject objectThatCollected)
    {
        _collectable.Collect(objectThatCollected, Index);
    }
}
