using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CollectableTriggerHandle))]

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private CollectableSOBase _collectable;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

    }

    public void Collect(GameObject objectThatCollected)
    {
        _collectable.Collect(objectThatCollected);
    }
}
