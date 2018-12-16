using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLink
{
    public string sWord;
    public ConversationNode node;

    public NodeLink(string _sWord, ConversationNode _node)
    {
        node = _node;
        sWord = _sWord;
    }

}
