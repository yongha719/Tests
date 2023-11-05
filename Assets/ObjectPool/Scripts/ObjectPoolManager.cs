using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public PooledObjectData bulletPoolData;

    private ObjectPool<Bullet> bulletPool = null;
    public IObjectPool<Bullet> BulletPool
    {
        get
        {
            if (bulletPool == null)
            {
                var bulletPoolParent = new GameObject("Bullet Pool====").transform;
                bulletPoolParent.SetParent(transform);
                bulletPoolData.poolParent = bulletPoolParent;

                bulletPool = new ObjectPool<Bullet>(bulletPoolParent, bulletPoolData, CreateBulletPool, OnGetBullet, OnReleaseBulletPool, bulletPoolData.size);
                bulletPool.InitPool();
            }

            return bulletPool;
        }
    }

    private Bullet CreateBulletPool()
    {
        GameObject bulletObject = Instantiate(bulletPoolData.pooledObjectPrefab, bulletPoolData.poolParent);

        var bullet = bulletObject.GetComponent<Bullet>();
        bullet.pool = bulletPool;

        bulletObject.SetActive(false);

        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.SetActive(true);
    }

    private void OnReleaseBulletPool(Bullet bullet)
    {
        bullet.SetActive(false);
    }
}
