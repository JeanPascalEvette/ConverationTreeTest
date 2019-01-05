using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeScript{

    void FirstFrameInState();
    void Update();
    void LastFrameInState();
}
