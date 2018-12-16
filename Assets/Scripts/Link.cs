using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Link {

    [SerializeField] public string sWord;
    [SerializeField] public uint iID;
    
    public Link(string _sWord, uint _iID)
    {
        iID = _iID;
        sWord = _sWord;
    }
}
