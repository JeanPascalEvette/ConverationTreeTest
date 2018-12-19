using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Chapter {

    private int iChapter;
    private Node[] daListOfNodes;
    private Node cCurrentNode;

    public Chapter(Profile _cProfile)
    {
        iChapter = _cProfile.iChapter;

        InitChapter(_cProfile);
    }

    private void InitChapter(Profile _cProfile)
    {
        string sChapterData = "Assets/Export/TreeNodes.json";
        using (StreamReader streamReader = File.OpenText(sChapterData))
        {
            string jsonString = streamReader.ReadToEnd();
            daListOfNodes = JsonHelper.FromJson<Node>(jsonString);
        }

        if(daListOfNodes.Length > 0)
        {
            for(int i = 0; i < daListOfNodes.Length; i++)
            {
                if(daListOfNodes[i].iID == _cProfile.uNodeID)
                {
                    cCurrentNode = daListOfNodes[i];
                    break;
                }
            }

            if (cCurrentNode == null)
                cCurrentNode = daListOfNodes[0];
        }
    }

    public Node GetCurrentNode()
    {
        return cCurrentNode;
    }
}
