using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPooledObject
{
    private List<T> pool;

    private Transform poolParent;

    private Func<T> createObject;
    private Action<T> onGet;
    private Action<T> onRelease;

    private Coroutine autoDestroyCoroutine;

    private int CountActive;
    private int CountInactive;

    public ObjectPool(Transform _poolParent, Func<T> _createObject, Action<T> _onGet, Action<T> _onRelease, int defaultSize = 20)
    {
        poolParent = _poolParent;
        createObject = _createObject;
        onGet = _onGet;
        onRelease = _onRelease;

        pool = new List<T>(defaultSize);
    }

    public void InitPool()
    {
        for (int i = 0; i < pool.Capacity; i++)
        {
            pool.Add(createObject());
        }

        ObjectPoolManager.Instance.StartCoroutine(AutoDestroyCoroutine());
    }

    public T Get()
    {
        // 현재 비활성화된 오브젝트를 찾음
        T obj = pool.Find(_obj => _obj.ActiveInHierarchy() == false);

        if (obj == null)
        {
            obj = createObject();
            pool.Add(obj);
        }
        
        onGet?.Invoke(obj);

        return obj;
    }

    public T Get(Transform parent)
    {
        var obj = Get();
        obj.SetParent(parent);

        return obj;
    }

    public void Release(T obj)
    {

    }

    private IEnumerator AutoDestroyCoroutine()
    {
        yield return null;
    }
}
