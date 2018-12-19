using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLink
{
    public List<string> daKeywords;
    public ConversationNode node;

    public NodeLink(List<string> _daKeywords, ConversationNode _node)
    { 
        node = _node;
        if (_daKeywords == null)
            daKeywords = new List<string> { };
        else
            daKeywords = _daKeywords;
    }

}
