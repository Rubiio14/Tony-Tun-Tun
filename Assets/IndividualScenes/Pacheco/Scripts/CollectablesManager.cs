using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public interface ICollectable
    {
        void collect();
    }
}
