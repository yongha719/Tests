using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour, IManager
{
    public void Hello()
    {
        print("Hello");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
public interface IManager
{
    void Hello();
    

}
