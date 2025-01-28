using UnityEngine;
using System;


[RequireComponent (typeof(BoxCollider2D))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSOBase _collectable;
    public int Index;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            _collectable.Collect(collision.gameObject, Index);
            Destroy(gameObject);
        }
    }
}
