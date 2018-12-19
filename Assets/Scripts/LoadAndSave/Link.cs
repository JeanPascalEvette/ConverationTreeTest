using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Link {

    [SerializeField] public string[] daKeywords;
    [SerializeField] public uint iID;
    private Node cNode; // Not used in Json
    
    public Link(string[] _daKeywords, uint _iID)
    {
        iID = _iID;
        cNode = null;
        daKeywords = _daKeywords;
    }

    public void SetNode(Node _cNode)
    {
        cNode = _cNode;
        iID = cNode.iID;
    }
}
