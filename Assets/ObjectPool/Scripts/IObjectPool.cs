using UnityEngine;

public interface IObjectPool<T> where T : MonoBehaviour, IPooledObject
{
    T Get();

    T Get(Transform parent);

    void Release(T obj);
}
