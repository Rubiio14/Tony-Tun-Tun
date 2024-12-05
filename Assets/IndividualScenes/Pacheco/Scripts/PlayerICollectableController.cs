using UnityEngine;

public class PlayerICollectableController : MonoBehaviour
{
    private ICollectable collectableInstance;

    private void TryToCollect(Collider2D collision)
    {
        if (collectableInstance != null)
        {
            if (collision.CompareTag("Player"))
            {
                collectableInstance.Collect();
            }
        }
    }

    public void SetIInstance(ICollectable manager)
    {
        collectableInstance = manager;
    }

    public void ClearIInstance()
    {
        collectableInstance = null;
    }
}
