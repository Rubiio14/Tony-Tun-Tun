using UnityEngine;
using UnityEngine.EventSystems;

public class SoundSelection : MonoBehaviour, IDeselectHandler
{
    public void OnDeselect(BaseEventData eventData)
    {
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.select);
    }
}
