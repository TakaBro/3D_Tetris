using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public GameObject prefab;
    public int amount;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public List<PoolItem> items;
    public List<GameObject> pooledItems;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject Get(string tag)
    {
        // Search in Pooled items
        for (int i = 0; i < pooledItems.Count; i++)
        {
            // If not active and equals tag
            if (!pooledItems[i].activeInHierarchy && pooledItems[i].tag == tag)
            {
                return pooledItems[i];
            }
        }

        // If not in the Pool, instantiate if tag exists in items 
        foreach (PoolItem item in items)
        {
            if (item.prefab.tag == tag)
            {
                GameObject newObj = Instantiate(item.prefab);
                newObj.SetActive(false);
                pooledItems.Add(newObj);
                return newObj;
            }
        }
        return null;
    }

    void Start()
    {
        pooledItems = new List<GameObject>();
        foreach (PoolItem item in items)
        {
            for (int i = 0; i < item.amount; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false); // Inactive when in obj pool
                pooledItems.Add(obj);
            }
        }
    }
}
