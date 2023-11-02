using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PooledObjectData", menuName = "MySciptable/ObjectPool", order = int.MaxValue)]
public class PooledObjectData : ScriptableObject
{
    public int DefaultSize = 20;

    public GameObject PooledObjectPrefab;

    public Transform Parent;
}
