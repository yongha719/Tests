using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PooledObjectData", menuName = "MySciptable/ObjectPool", order = int.MaxValue)]
public class PooledObjectData : ScriptableObject
{
    public int size;

    public GameObject pooledObjectPrefab;

    public Transform poolParent;

    public bool shouldDestroyWithLerp;

    [Tooltip("Lerp로 지울 때 한번에 지울 사이즈")]
    public int SizeToDestroy;

    [Tooltip("자동으로 삭제할지 체크할 때 딜레이")]
    public float AutoDestroyPoolDelay = 1;

    [Tooltip("삭제하려고 할때 pool이 늘어날 수 있으니 그전에 두는 딜레이")]
    public float DelayBeforePossiblePoolIncrease = 10;

    [Tooltip("Lerp로 삭제할 때의 반복 딜레이")]
    public float AfterDestroyDelay = 0.5f;
}
