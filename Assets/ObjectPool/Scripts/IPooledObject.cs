using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject 
{
    public bool CanGet { get; set; }

    void OnDestroyObject();
}
