using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
