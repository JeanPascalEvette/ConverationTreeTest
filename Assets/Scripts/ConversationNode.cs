using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class ConversationNode : DraggableNode
{

    List<KeyValuePair<KeyWord, ConversationNode>> daOutcomes;

    public ConversationNode()
    {
        daOutcomes = new List<KeyValuePair<KeyWord, ConversationNode>>();
    }

    public void LinkTo(KeyWord _keyword, ConversationNode _node)
    {
        daOutcomes.Add(new KeyValuePair<KeyWord, ConversationNode>(_keyword, _node));
    }

    public override void DrawBackground(Vector2 _vNodeOffset)
    {
        Handles.BeginGUI();
        foreach (KeyValuePair<KeyWord, ConversationNode> keyValuePair in daOutcomes)
        {
            Vector2 vP1 = _vNodeOffset + vPosStart + vSize * 0.5f;
            Vector2 vP2 = _vNodeOffset + keyValuePair.Value.vPosStart + vSize * 0.5f;
            if(keyValuePair.Key.sWord.Length > 0)
                Handles.color = Color.white;
            else
                Handles.color = Color.magenta;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.DrawLine(vP1, vP2);
        }
        Handles.EndGUI();

        base.DrawBackground(_vNodeOffset);
    }

    public override void Draw(Vector2 _vNodeOffset)
    {
        base.Draw(_vNodeOffset);
    }
}
