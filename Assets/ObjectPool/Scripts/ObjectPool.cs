using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

[Serializable]
public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPooledObject
{
    private List<T> pool;

    private PooledObjectData pooledObjectData;
    private int Count => pool.Count;
    private int CountActive;
    private int CountInactive => Count - CountActive;

    private Transform poolParent;

    private int minPoolSize;

    private Func<T> createObject;
    private Action<T> onGet;
    private Action<T> onRelease;

    [SerializeField, Tooltip("pool 리스트가 늘어났는지 체크")]
    private bool hasListIncreased = true;

    [SerializeField]
    private bool stillDestroy = false;

    private List<T> destroyList;

    [Tooltip("오브젝트 삭제할 때 쓰는 코루틴")]
    private IEnumerator? destroyPoolCoroutine;

    private WaitForSeconds waitAutoDestroyDelay;
    private WaitForSeconds waitLerpDestroyDelay;
    [Tooltip("삭제하려고 할때 pool이 늘어날 수 있으니 그전에 두는 딜레이")]
    private WaitForSeconds waitDelayBeforePossiblePoolIncrease;

    public ObjectPool(Transform _poolParent, PooledObjectData _pooledObjectData, Func<T> _createObject, Action<T> _onGet, Action<T> _onRelease, int _defaultSize = 20)
    {
        poolParent = _poolParent;

        createObject = _createObject;
        onGet = _onGet;
        onRelease = _onRelease;

        minPoolSize = _defaultSize;

        pooledObjectData = _pooledObjectData;
        pooledObjectData.poolParent = poolParent;

        waitAutoDestroyDelay = new WaitForSeconds(pooledObjectData.DelayBeforePossiblePoolIncrease);
        waitLerpDestroyDelay = new WaitForSeconds(pooledObjectData.AfterDestroyDelay);
        waitDelayBeforePossiblePoolIncrease = new WaitForSeconds(pooledObjectData.DelayBeforePossiblePoolIncrease);

        pool = new List<T>(minPoolSize);
        destroyList = new List<T>(minPoolSize / 5);
    }

    public void InitPool()
    {
        for (int i = 0; i < minPoolSize; i++)
        {
            pool.Add(createObject());
        }

        MonoMethods.Start(AutoDestroyCoroutine());
    }

    public T Get()
    {
        T obj = pool.FirstOrDefault(_obj => _obj.CanGet);

        hasListIncreased = false;

        if (obj == null)
        {
            obj = createObject();
            pool.Add(obj);
            hasListIncreased = true;

            return obj;
        }

        obj.CanGet = false;
        CountActive++;

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
        CountActive--;

        obj.SetParent(poolParent);
        obj.CanGet = true;

        onRelease?.Invoke(obj);
    }

    [ContextMenu("Destroy")]
    public void Destroy(int size = 1)
    {
        destroyList.AddRange(pool.Where(_obj => _obj.CanGet).Take(Mathf.Min(size, CountInactive)));
        destroyList.ForEach(_obj => _obj.CanGet = false);

        DestroyList();
    }

    private void DestroyObject(T obj)
    {
        obj.OnDestroyObject();
        pool.Remove(obj);
        UnityEngine.Object.Destroy(obj);
    }

    private void DestroyList()
    {
        var destroySize = Mathf.Min(destroyList.Count, CountInactive);

        for (int i = 0; i < destroySize; i++)
        {
            DestroyObject(destroyList[i]);
        }
    }

    private IEnumerator AutoDestroyCoroutine()
    {
        while (true)
        {
            yield return waitAutoDestroyDelay;

            if (hasListIncreased && CountInactive <= minPoolSize)
            {
                if (pooledObjectData.shouldDestroyWithLerp )
                {
                    StopAndResetDestroyCoroutine();
                }

                yield break;
            }

            destroyList.AddRange(pool.Where(_obj => _obj.CanGet).Take(CountInactive / 10));
            destroyList.ForEach(_obj => _obj.CanGet = false);

            if (pooledObjectData.shouldDestroyWithLerp)
            {
                if (destroyPoolCoroutine == null && stillDestroy == false)
                {
                    stillDestroy = true;

                    destroyPoolCoroutine = MonoMethods.Start(LerpDestroyCoroutine());
                    Debug.Log("자동 지우기 코루틴 시작");
                }
            }
            else
            {
                Destroy(CountInactive / 10);
            }
        }
    }

    private IEnumerator LerpDestroyCoroutine()
    {
        yield return waitDelayBeforePossiblePoolIncrease;

        int destroySize;
        int startIndex = 0;

        while (destroyList.Count > 0)
        {
            destroySize = Mathf.Min(pooledObjectData.SizeToDestroy, destroyList.Count);

            for (int i = startIndex; i < startIndex + destroySize; i++)
            {
                DestroyObject(destroyList[i]);
            }

            destroyList.RemoveRange(startIndex, startIndex + destroySize);

            startIndex += destroySize;

            yield return waitLerpDestroyDelay;
        }

        StopAndResetDestroyCoroutine();
    }

    private void StopAndResetDestroyCoroutine()
    {
        if (destroyPoolCoroutine != null)
        {
            MonoMethods.Stop(destroyPoolCoroutine);
            destroyPoolCoroutine = null;
        }
        stillDestroy = false;
    }
}
