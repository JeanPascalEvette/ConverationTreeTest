using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : INodeScript
{
    public void FirstFrameInState()
    {
        Debug.Log("Test3 - FirstFrameInState");
    }

    public void LastFrameInState()
    {
        Debug.Log("Test3 - LastFrameInState");
    }

    void INodeScript.Update()
    {
        Debug.Log("Test3 - Update");
    }
}
