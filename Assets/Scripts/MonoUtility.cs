using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoUtility
{
    public static void SetActive(this MonoBehaviour mono, bool value)
    {
        mono.gameObject.SetActive(value);
    }

    public static void SetParent(this MonoBehaviour mono, Transform parent)
    {
        mono.transform.SetParent(parent);
    }
}
