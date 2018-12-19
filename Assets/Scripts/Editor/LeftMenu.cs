using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LeftMenu : EditorWindow
{
    public static float LEFT_MENU_WIDTH = 300.0f;
    public static float SMALL_LABEL_WIDTH = LEFT_MENU_WIDTH * 0.3f;
    public static float NORMAL_BUTTON_WIDTH = LEFT_MENU_WIDTH * 0.5f;
    public static float SMALL_BUTTON_WIDTH = LEFT_MENU_WIDTH * 0.2f;
    public static float NORMAL_LINE_HEIGHT = 30.0f;

    public void Draw()
    {
        GUIStyle styleCentered = new GUIStyle();
        styleCentered.alignment = TextAnchor.MiddleLeft;

        float fXCurrentPos = 0.0f;
        float fYCurrentPos = 0.0f;
        EditorGUI.DrawRect(new Rect(fXCurrentPos, fYCurrentPos, LEFT_MENU_WIDTH, maxSize.y), Color.grey);
        if (GUI.Button(new Rect(fXCurrentPos, fYCurrentPos, NORMAL_BUTTON_WIDTH, NORMAL_LINE_HEIGHT), "Load"))
        {
            ConversationTreeEditor.Instance.LoadFromJSON();
        }
        fXCurrentPos = NORMAL_BUTTON_WIDTH;
        if (GUI.Button(new Rect(fXCurrentPos, fYCurrentPos, NORMAL_BUTTON_WIDTH, NORMAL_LINE_HEIGHT), "Save"))
        {
            ConversationTreeEditor.Instance.SaveToJSON();
        }
        fXCurrentPos = 0.0f;
        fYCurrentPos += NORMAL_LINE_HEIGHT;
        if (GUI.Button(new Rect(fXCurrentPos, fYCurrentPos, NORMAL_BUTTON_WIDTH, NORMAL_LINE_HEIGHT), "Add Node"))
        {
            ConversationTreeEditor.Instance.AddNode();
        }
        fXCurrentPos = NORMAL_BUTTON_WIDTH;
        if (GUI.Button(new Rect(fXCurrentPos, fYCurrentPos, NORMAL_BUTTON_WIDTH, NORMAL_LINE_HEIGHT), "Remove Node"))
        {
            ConversationTreeEditor.Instance.RemoveNode();
        }
        ConversationNode selectedNode = ConversationTreeEditor.Instance.GetSelectedNode();
        if (selectedNode)
        {
            {
                fXCurrentPos = 0.0f;
                fYCurrentPos += NORMAL_LINE_HEIGHT;
                GUI.Label(new Rect(fXCurrentPos, fYCurrentPos, SMALL_LABEL_WIDTH, NORMAL_LINE_HEIGHT), "Node Name", styleCentered);
                fXCurrentPos += SMALL_LABEL_WIDTH;
                EditorGUI.BeginChangeCheck();
                selectedNode.sName = GUI.TextField(new Rect(fXCurrentPos, fYCurrentPos, LEFT_MENU_WIDTH - fXCurrentPos, NORMAL_LINE_HEIGHT), selectedNode.sName);
                if (EditorGUI.EndChangeCheck())
                {
                    selectedNode.vSize = GUI.skin.label.CalcSize(new GUIContent(selectedNode.sName));
                }
            }
            {
                fXCurrentPos = 0.0f;
                fYCurrentPos += NORMAL_LINE_HEIGHT;
                GUI.Label(new Rect(fXCurrentPos, fYCurrentPos, SMALL_LABEL_WIDTH, NORMAL_LINE_HEIGHT), "Node Text", styleCentered);
                fXCurrentPos += SMALL_LABEL_WIDTH;
                GUI.Label(new Rect(fXCurrentPos, fYCurrentPos, NORMAL_BUTTON_WIDTH, NORMAL_LINE_HEIGHT), selectedNode.sText, styleCentered);
                fXCurrentPos += NORMAL_BUTTON_WIDTH;
                if (GUI.Button(new Rect(fXCurrentPos, fYCurrentPos, LEFT_MENU_WIDTH - fXCurrentPos, NORMAL_LINE_HEIGHT), "Edit"))
                {
                    PopupWindow.Show(new Rect(fXCurrentPos, fYCurrentPos, LEFT_MENU_WIDTH - fXCurrentPos, NORMAL_LINE_HEIGHT), new EditNodeContentPopup(selectedNode));
                }
            }
            {
                fXCurrentPos = 0.0f;
                fYCurrentPos += NORMAL_LINE_HEIGHT;
                GUI.Label(new Rect(fXCurrentPos, fYCurrentPos, LEFT_MENU_WIDTH, NORMAL_LINE_HEIGHT), "Links", styleCentered);
            }
            for (int i = 0; i < selectedNode.daOutcomes.Count; i++)
            {
                fXCurrentPos = 0.0f;
                fYCurrentPos += NORMAL_LINE_HEIGHT;
                GUI.Label(new Rect(fXCurrentPos, fYCurrentPos, SMALL_LABEL_WIDTH, NORMAL_LINE_HEIGHT), "-> "+selectedNode.daOutcomes[i].node.sName, styleCentered);
                fXCurrentPos += SMALL_LABEL_WIDTH;
                if (selectedNode.daOutcomes[i].daKeywords.Count > 0)
                {
                    GUI.Label(new Rect(fXCurrentPos, fYCurrentPos, SMALL_LABEL_WIDTH, NORMAL_LINE_HEIGHT), selectedNode.daOutcomes[i].daKeywords[0]);
                }
                fXCurrentPos += SMALL_LABEL_WIDTH;
                if (GUI.Button(new Rect(fXCurrentPos, fYCurrentPos, SMALL_BUTTON_WIDTH, NORMAL_LINE_HEIGHT), "Edit"))
                {
                    PopupWindow.Show(new Rect(fXCurrentPos, fYCurrentPos, SMALL_BUTTON_WIDTH, NORMAL_LINE_HEIGHT), new EditLinkContentPopup(selectedNode.daOutcomes[i]));
                }
                fXCurrentPos += SMALL_BUTTON_WIDTH;
                if (GUI.Button(new Rect(fXCurrentPos, fYCurrentPos, SMALL_BUTTON_WIDTH, NORMAL_LINE_HEIGHT), "Delete"))
                {
                    selectedNode.daOutcomes.RemoveAt(i);
                }
            }
        }
    }
}
