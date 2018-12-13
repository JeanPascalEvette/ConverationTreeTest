using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyWord {
    private static uint iIDCount = 0;

    public string sWord;
    public uint iID;
    
    public KeyWord(string _sWord)
    {
        iID = iIDCount++;
        sWord = _sWord;
    }

    public KeyWord(uint _iID, string _sWord)
    {
        iID = _iID;
        sWord = _sWord;
    }
}
