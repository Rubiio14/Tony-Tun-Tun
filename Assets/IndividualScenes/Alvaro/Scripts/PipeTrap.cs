using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PipeTrap : MonoBehaviour
{
    [SerializeField] private float cooldownDrop, cooldownGas;

    public float speed;
    public bool inRange, isGas, isDrop, isEnable;
    public Transform acidPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void PipeDropInstance()
    {
        if (inRange && isDrop)
        {
            GameObject Drop = ObjectPool.SharedInstance.GetDropPooledObject();
            AcidDrop InstancedAcidDrop = null;
            if (Drop && Drop.TryGetComponent<AcidDrop>(out InstancedAcidDrop))
            {
                InstancedAcidDrop.transform.position = acidPosition.transform.position;
                InstancedAcidDrop.transform.rotation = acidPosition.transform.rotation;
                InstancedAcidDrop.Speed = speed;
                InstancedAcidDrop.Direction = acidPosition.forward;
                InstancedAcidDrop.gameObject.SetActive(true);
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.AcidDrop, this.gameObject.transform.position);
            }
            StartCoroutine(DropCooldown(InstancedAcidDrop.gameObject, cooldownDrop));
        }
    }

    private void PipeGasInstance()
    {
        if (inRange && isGas && !isEnable)
        {
            GameObject Gas = ObjectPool.SharedInstance.GetGasPooledObject();
            if (Gas)
            {
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.AcidGas, this.gameObject.transform.position);
                Gas.transform.position = acidPosition.transform.position;
                Gas.transform.rotation = acidPosition.transform.rotation;
                Gas.gameObject.SetActive(true);
                isEnable = true;
            }
            StartCoroutine(GasCooldown(Gas, cooldownGas));
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            inRange = true;
            PipeDropInstance();
            PipeGasInstance();
        }
        
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            if (isDrop)
            {
                StopAllCoroutines();
            }
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
        isEnable = false;
        PipeGasInstance();
    }
}
