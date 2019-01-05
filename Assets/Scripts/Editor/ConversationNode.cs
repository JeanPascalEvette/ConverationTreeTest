using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ConversationNode : DraggableNode
{
    private static float ARROW_BRANCH_DISTANCE_FROM_MAIN = 10.0f;
    private static float ARROW_BRANCH_DISTANCE_FROM_END = 30.0f;

    public string sText;
    public List<NodeLink> daOutcomes;
    public string sScriptName;

    public ConversationNode()
    {
        daOutcomes = new List<NodeLink>();
    }

    public void Init(string _sName, string _sText, Vector2 _vPosStart)
    {
        base.Init(_sName, _vPosStart);

        sText = _sText;
        sScriptName = "";
    }

    public void LinkTo(NodeLink _link)
    {
        if(_link.node != this)
            daOutcomes.Add(_link);
    }

    public override void DrawBackground(Vector2 _vNodeOffset)
    {
        Handles.BeginGUI();
        foreach (NodeLink link in daOutcomes)
        {
            Vector2 vP1 = _vNodeOffset + vPosStart + vSize * 0.5f;
            Vector2 vP2 = _vNodeOffset + link.node.vPosStart + vSize * 0.5f;
            if(link.daKeywords.Count > 0)
                Handles.color = Color.white;
            else
                Handles.color = Color.magenta;
            Handles.DrawLine(vP1, vP2);
            Vector2 vP1ToP2 = (vP2 - vP1);
            float fVecLength = vP1ToP2.magnitude;
            Vector2 vDirection = vP1ToP2.normalized;
            Vector2 right = Quaternion.LookRotation(vDirection) * new Vector2(0, -1);
            Vector2 left = Quaternion.LookRotation(vDirection) * new Vector2(0, 1);
            float fRelativeDistToEnd = (fVecLength - ARROW_BRANCH_DISTANCE_FROM_END) / fVecLength;
            Vector2 vStartPos = vP1 + vP1ToP2 * fRelativeDistToEnd;
            Handles.DrawLine(vP2, vStartPos + right * ARROW_BRANCH_DISTANCE_FROM_MAIN);
            Handles.DrawLine(vP2, vStartPos + left * ARROW_BRANCH_DISTANCE_FROM_MAIN);
        }
        Handles.EndGUI();

        base.DrawBackground(_vNodeOffset);
    }

    public override void Draw(Vector2 _vNodeOffset)
    {
        base.Draw(_vNodeOffset);
    }
}
