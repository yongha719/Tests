using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public PooledObjectData BulletPoolObjectData;

    private ObjectPool<Bullet> bulletPool;
    public IObjectPool<Bullet> BulletPool
    {
        get
        {
            if(bulletPool == null)
            {
                Transform bulletPoolParent = new GameObject("Bullet Pool").transform;
                bulletPoolParent.SetParent(transform);
                BulletPoolObjectData.Parent = bulletPoolParent;

                //bulletPool = new ObjectPool<Bullet>(bulletPoolParent, )
            }

            return null;
        }
    }

    private void Start()
    {
        
    }
}
