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
                }

                for (int u = 0; u < daListOfNodes[i].daOutcomes.Count; u++)
                {
                    for (int i2 = 0; i2 < daListOfNodes.Length; i2++)
                    {
                        if(daListOfNodes[i2].iID == daListOfNodes[i].daOutcomes[u].iID)
                        {
                            daListOfNodes[i].daOutcomes[u].cNode = daListOfNodes[i2];
                            break;
                        }
                    }
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

    public int ValidateString(string _sText)
    {
        int keyWordsToValidate = 0;
        for (int i = 0; i < cCurrentNode.daOutcomes.Count; i++)
        {
            keyWordsToValidate = cCurrentNode.daOutcomes[i].daKeywords.Length;
            for (int u = 0; u < cCurrentNode.daOutcomes[i].daKeywords.Length; u++)
            {
                if (_sText.Contains(cCurrentNode.daOutcomes[i].daKeywords[u]))
                {
                    keyWordsToValidate--;
                }
            }
            if (keyWordsToValidate <= 0)
            {
                return i;
            }
        }
        return -1;
    }

    public void ChooseOutcome(int _iIdx)
    {
        if (_iIdx < 0 || _iIdx >= cCurrentNode.daOutcomes.Count)
            return;

        cCurrentNode = cCurrentNode.daOutcomes[_iIdx].cNode;
    }
}
