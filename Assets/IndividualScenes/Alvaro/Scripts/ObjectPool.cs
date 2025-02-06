using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    public List<GameObject> pooledDropObjects;
    public List<GameObject> pooledBulletObjects;
    public List<GameObject> pooledGasObjects;

    public GameObject acidDrop;
    public int amountDropToPool;
    public GameObject bullet;
    public int amountBulletToPool;
    public GameObject gas;
    public int amountGasToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledDropObjects = new List<GameObject>();
        GameObject acidDropInstance;
        for (int i = 0; i < amountDropToPool; i++)
        {
            acidDropInstance = Instantiate(acidDrop);
            acidDropInstance.SetActive(false);
            pooledDropObjects.Add(acidDropInstance);
        }
        pooledBulletObjects = new List<GameObject>();
        GameObject bulletInstance;
        for (int i = 0; i < amountBulletToPool; i++)
        {
            bulletInstance = Instantiate(bullet);
            bulletInstance.SetActive(false);
            pooledBulletObjects.Add(bulletInstance);
        }
        pooledGasObjects = new List<GameObject>();
        GameObject gasInstance;
        for (int i = 0; i < amountGasToPool; i++)
        {
            gasInstance = Instantiate(gas);
            gasInstance.SetActive(false);
            pooledGasObjects.Add(gasInstance);
        }
    }

    public GameObject GetDropPooledObject()
    {
        for (int i = 0; i < amountDropToPool; i++)
        {
            if (!pooledDropObjects[i].activeInHierarchy)
            {
                return pooledDropObjects[i];
            }
        }
        return null;
    }

    public GameObject GetBulletPooledObject()
    {
        for (int i = 0; i < amountBulletToPool; i++)
        {
            if (!pooledBulletObjects[i].activeInHierarchy)
            {
                return pooledBulletObjects[i];
            }
        }
        return null;
    }

    public GameObject GetGasPooledObject()
    {
        for (int i = 0; i < amountGasToPool; i++)
        {
            if (!pooledGasObjects[i].activeInHierarchy)
            {
                return pooledGasObjects[i];
            }
        }
        return null;
    }

}
