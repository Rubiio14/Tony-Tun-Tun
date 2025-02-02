using UnityEngine;
using System;
using System.Collections;



[RequireComponent (typeof(BoxCollider2D))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSOBase _collectable;
    public int Index;

    //Para desactivar malla
    [SerializeField] 
    GameObject _CollectableRenderer;
    public float _destroyCollectable = 2f;
    [SerializeField]
    GameObject _vfxCollect;


    //IMPORTANTE: arrastrar MeshRenderer y el VFX del prefab al script

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            _collectable.Collect(collision.gameObject, Index);

            GetComponent<BoxCollider2D>().enabled = false;
            _CollectableRenderer.SetActive(false);
            _vfxCollect.SetActive(true);
            Destroy();
        }
    }

    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_destroyCollectable);
        _vfxCollect.SetActive(false);
        Destroy(gameObject);

    }


}
