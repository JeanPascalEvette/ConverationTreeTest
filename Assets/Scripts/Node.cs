using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node {

    public uint iID;
    public string sName;
    public Vector2 vPosStart;
    public List<Link> daOutcomes;

    public Node()
    {
        sName = "";
        vPosStart = Vector2.zero;
        daOutcomes = new List<Link>();
    }
}
