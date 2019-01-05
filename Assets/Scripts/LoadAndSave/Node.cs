using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node {

    public uint iID;
    public string sName;
    public string sText;
    public string sScriptName; 
    [System.NonSerialized] public INodeScript cScript;
    public Vector2 vPosStart;
    public List<Link> daOutcomes;

    public Node()
    {
        sName = "";
        sScriptName = "";
        vPosStart = Vector2.zero;
        cScript = null;
        daOutcomes = new List<Link>();
    }

    public void Init()
    {
        if (sScriptName.Length > 0)
            cScript = (INodeScript)System.Activator.CreateInstance(System.Type.GetType(sScriptName));
    }
}
