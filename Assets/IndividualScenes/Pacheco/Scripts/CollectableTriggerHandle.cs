using UnityEngine;

public class CollectableTriggerHandle : MonoBehaviour
{
    private CollectableManager _collectable;

    public void Awake()
    {
        _collectable = GetComponent<CollectableManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _collectable.Collect(other.gameObject);

            Destroy(gameObject);
        }
    }
