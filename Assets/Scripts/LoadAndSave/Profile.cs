using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Profile{

    public string sName;
    public int iChapter;
    public uint uNodeID;
    // Todo : Add list of choices

    public Profile()
    {
        sName = "";
        iChapter = -1;
        uNodeID = 0;
    }
}
