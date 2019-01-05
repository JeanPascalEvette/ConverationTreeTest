using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : INodeScript
{
    public void FirstFrameInState()
    {
        Debug.Log("Test2 - FirstFrameInState");
    }

    public void LastFrameInState()
    {
        Debug.Log("Test2 - LastFrameInState");
    }

    void INodeScript.Update()
    {
        Debug.Log("Test2 - Update");
    }
}
