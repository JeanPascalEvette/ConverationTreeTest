using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

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
                daListOfNodes[i].Init();

                if (daListOfNodes[i].iID == _cProfile.uNodeID)
                {
                    cCurrentNode = daListOfNodes[i];
                }

                for (int u = 0; u < daListOfNodes[i].daOutcomes.Count; u++)
                {
                    // Convert Keywords to fit Regex format
                    for (int v = 0; v < daListOfNodes[i].daOutcomes[u].daKeywords.Length; v++)
                    {
                        daListOfNodes[i].daOutcomes[u].daKeywords[v] = daListOfNodes[i].daOutcomes[u].daKeywords[v].Replace("(", "\\(");
                        daListOfNodes[i].daOutcomes[u].daKeywords[v] = daListOfNodes[i].daOutcomes[u].daKeywords[v].Replace(")", "\\)");
                        daListOfNodes[i].daOutcomes[u].daKeywords[v] = daListOfNodes[i].daOutcomes[u].daKeywords[v].Replace("[", "(");
                        daListOfNodes[i].daOutcomes[u].daKeywords[v] = daListOfNodes[i].daOutcomes[u].daKeywords[v].Replace("]", ")");
                    }

                    //Link nodes towards other nodes based on ID
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
        
        if(cCurrentNode.cScript != null)
            cCurrentNode.cScript.FirstFrameInState();
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
                Match match = Regex.Match(_sText, @cCurrentNode.daOutcomes[i].daKeywords[u], RegexOptions.IgnoreCase);
                if (match.Success)
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

        if (cCurrentNode.cScript != null)
            cCurrentNode.cScript.LastFrameInState();
        cCurrentNode = cCurrentNode.daOutcomes[_iIdx].cNode;
        if (cCurrentNode.cScript != null)
            cCurrentNode.cScript.FirstFrameInState();
    }
}
