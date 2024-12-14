using UnityEngine;

public interface IActivate
{
    public bool Submitted {  get; set; }
    public void Activate(GameObject enablingParent);
}