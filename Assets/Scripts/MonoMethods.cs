using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MonoMethods : MonoBehaviour
{
    private static MonoMethods monoMethods;

    private static ConditionalWeakTable<IEnumerator, Coroutine> coroutineTable = new ConditionalWeakTable<IEnumerator, Coroutine>();

    private void Start()
    {
        monoMethods = this;
    }

    public static IEnumerator? Start(IEnumerator? coroutine)
    {
        if (coroutine != null)
        {
            coroutineTable.Add(coroutine, monoMethods.StartCoroutine(coroutine));
        }

        return coroutine;
    }

    public static void Stop(IEnumerator? coroutine)
    {
        if (coroutine != null && coroutineTable.TryGetValue(coroutine, out var routine))
        {
            monoMethods.StopCoroutine(routine);
        }
    }
}
