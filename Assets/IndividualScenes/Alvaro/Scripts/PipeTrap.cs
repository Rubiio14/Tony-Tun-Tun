using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PipeTrap : MonoBehaviour
{
    [SerializeField] private float cooldownTimer;

    public float speed;
    public Transform acidPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void PipeDropInstance()
    {
        GameObject Drop = ObjectPool.SharedInstance.GetDropPooledObject();
        AcidDrop InstancedAcidDrop = null;
        if(Drop && Drop.TryGetComponent<AcidDrop>(out InstancedAcidDrop))
        {
            InstancedAcidDrop.transform.position = acidPosition.transform.position;
            InstancedAcidDrop.transform.rotation = acidPosition.transform.rotation;
            InstancedAcidDrop.Speed = speed;
            InstancedAcidDrop.Direction = acidPosition.forward;
            InstancedAcidDrop.gameObject.SetActive(true);
        }
        StartCoroutine(DropCooldown(InstancedAcidDrop.gameObject, cooldownTimer));
    }

    private void PipeGasInstance()
    {
        GameObject Gas = ObjectPool.SharedInstance.GetGasPooledObject();
        if (Gas)
        {
            Gas.transform.position = acidPosition.transform.position;
            Gas.transform.rotation = acidPosition.transform.rotation;
            Gas.gameObject.SetActive(true);
        }
        StartCoroutine(GasCooldown(Gas,cooldownTimer));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            PipeDropInstance();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PipeGasInstance();
        }
    }

    public IEnumerator DropCooldown(GameObject drop, float timer)
    {
        yield return new WaitForSeconds(timer);
        PipeDropInstance();
    }

    

    public IEnumerator GasCooldown(GameObject gas, float timer)
    {
        yield return new WaitForSeconds(timer);
        gas.SetActive(false);
        yield return new WaitForSeconds(timer);
        PipeGasInstance();
    }
}
