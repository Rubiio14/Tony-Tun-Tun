using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableTriggerHandler : MonoBehaviour
{

    private Collectable _collectable;

    private void Awake()
    {
        _collectable = GetComponent<Collectable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            _collectable.Collect(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
