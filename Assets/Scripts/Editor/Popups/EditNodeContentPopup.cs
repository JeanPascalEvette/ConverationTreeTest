using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditNodeContentPopup : PopupWindowContent
{
    private ConversationNode cNode;
    private string sTextAreaValue;

    public EditNodeContentPopup(ConversationNode _node)
    {
        cNode = _node;
        sTextAreaValue = cNode.sText;
    }

    public override void OnGUI(Rect rect)
    {
        Vector2 vMaxSize = this.GetWindowSize();
        sTextAreaValue = GUI.TextArea(new Rect(0.0f, 0.0f, vMaxSize.x, vMaxSize.y * 0.9f), sTextAreaValue);
        if(GUI.Button(new Rect(0.0f, vMaxSize.y * 0.9f, vMaxSize.x, vMaxSize.y * 0.1f), "OK"))
        {
            cNode.sText = sTextAreaValue;
            this.editorWindow.Close();
        }
    }
}
