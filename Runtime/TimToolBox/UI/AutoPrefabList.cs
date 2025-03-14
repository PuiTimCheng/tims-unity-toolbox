using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class AutoPrefabList<T> : IEnumerable<T> where T : Component
{
    public T prefab;
    public Transform parentTransform;
    public List<T> pooledObjects;

    public int Count => pooledObjects.Count;
    public AutoPrefabList(Transform parentTransform, int count)
    {
        this.parentTransform = parentTransform;
        InitializePool();
        SetListSize(count);
    }

    private void InitializePool()
    {
        // Ensure there’s at least one child to use as the prefab
        if (parentTransform.childCount == 0)
        {
            Debug.LogError("StructurePool requires at least one child to use as a prefab.");
            return;
        }

        pooledObjects = new List<T>();
        foreach (Transform child in parentTransform)
        {
            var instance = child.gameObject.GetComponent<T>();
            if(child == parentTransform.GetChild(0)) prefab = instance;
            pooledObjects.Add(instance);
        }
        prefab.gameObject.SetActive(true); // Ensure it starts active (you can change this based on your needs)
    }

    public void SetListSize(int count)
    {
        //disable any object over the count
        for (int i = count; i < pooledObjects.Count; i++)
        {
            if (i >= count) pooledObjects[i].gameObject.SetActive(false);
        }
        //create new object if needed
        while (pooledObjects.Count < count)
        {
            T newObj = Object.Instantiate(prefab, parentTransform);
            newObj.gameObject.SetActive(true); // 默认情况下将其设置为非活动状态
            pooledObjects.Add(newObj);
        }
    }

    public T this[int index] => Get(index);
    public T Get(int index)
    {
        if (index >= pooledObjects.Count)
        {
            Debug.LogError("Index out of range.");
            return null;
        }
        return pooledObjects[index];
    }

    public void Disable(int index)
    {
        if (index >= pooledObjects.Count)
        {
            Debug.LogError("Index out of range.");
        }
        pooledObjects[index].gameObject.SetActive(false);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return pooledObjects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

