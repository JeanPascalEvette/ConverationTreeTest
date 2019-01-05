using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : INodeScript {
    public void FirstFrameInState()
    {
        Debug.Log("Test1 - FirstFrameInState");
    }

    public void LastFrameInState()
    {
        Debug.Log("Test1 - LastFrameInState");
    }
    
    void INodeScript.Update()
    {
        Debug.Log("Test1 - Update");
    }
}
