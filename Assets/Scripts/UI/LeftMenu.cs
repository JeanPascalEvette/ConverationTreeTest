using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LeftMenu : EditorWindow
{
    public static float LEFT_MENU_WIDTH = 300.0f;
    public static float LEFT_MENU_HALF_OF_WIDTH = LEFT_MENU_WIDTH / 2.0f;
    public static float LEFT_MENU_THIRD_OF_WIDTH = LEFT_MENU_WIDTH / 3.0f;
    public static float LEFT_MENU_BUTTON_HEIGHT = 30.0f;

    public static Vector2 LEFT_MENU_DEFAULT_BUTTON_SIZE = new Vector2(LEFT_MENU_HALF_OF_WIDTH, LEFT_MENU_BUTTON_HEIGHT);


    public void Draw()
    {
        GUIStyle styleCentered = new GUIStyle();
        styleCentered.alignment = TextAnchor.MiddleLeft;

        EditorGUI.DrawRect(new Rect(Vector2.zero, new Vector2(LEFT_MENU_WIDTH, maxSize.y)), Color.grey);
        if (GUI.Button(new Rect(Vector2.zero, LEFT_MENU_DEFAULT_BUTTON_SIZE), "Load"))
        {
            ConversationTreeEditor.Instance.LoadFromJSON();
        }
        if (GUI.Button(new Rect(new Vector2(LEFT_MENU_DEFAULT_BUTTON_SIZE.x, 0.0f), LEFT_MENU_DEFAULT_BUTTON_SIZE), "Save"))
        {
            ConversationTreeEditor.Instance.SaveToJSON();
        }
        if (GUI.Button(new Rect(new Vector2(0.0f, LEFT_MENU_DEFAULT_BUTTON_SIZE.y), LEFT_MENU_DEFAULT_BUTTON_SIZE), "Add Node"))
        {
            ConversationTreeEditor.Instance.AddNode();
        }
        if (GUI.Button(new Rect(LEFT_MENU_DEFAULT_BUTTON_SIZE, LEFT_MENU_DEFAULT_BUTTON_SIZE), "Remove Node"))
        {
            ConversationTreeEditor.Instance.RemoveNode();
        }
        ConversationNode selectedNode = ConversationTreeEditor.Instance.GetSelectedNode();
        if (selectedNode)
        {
            GUI.Label(new Rect(new Vector2(0.0f, 2.0f * LEFT_MENU_DEFAULT_BUTTON_SIZE.y), LEFT_MENU_DEFAULT_BUTTON_SIZE), "Name", styleCentered);
            EditorGUI.BeginChangeCheck();
            selectedNode.sName = GUI.TextField(new Rect(new Vector2(LEFT_MENU_DEFAULT_BUTTON_SIZE.x, 2.0f * LEFT_MENU_BUTTON_HEIGHT), LEFT_MENU_DEFAULT_BUTTON_SIZE), selectedNode.sName);
            if (EditorGUI.EndChangeCheck())
            {
                selectedNode.vSize = GUI.skin.label.CalcSize(new GUIContent(selectedNode.sName));
            }
            GUI.Label(new Rect(new Vector2(0.0f, 3.0f * LEFT_MENU_BUTTON_HEIGHT), new Vector2(LEFT_MENU_WIDTH, LEFT_MENU_BUTTON_HEIGHT)), "Links", styleCentered);
            for (int i = 0; i < selectedNode.daOutcomes.Count; i++)
            { 
                GUI.Label(new Rect(new Vector2(0.0f, (4.0f + i) * LEFT_MENU_BUTTON_HEIGHT), new Vector2(LEFT_MENU_THIRD_OF_WIDTH, LEFT_MENU_BUTTON_HEIGHT)), "-> "+selectedNode.daOutcomes[i].node.sName, styleCentered);
                selectedNode.daOutcomes[i].sWord = GUI.TextField(new Rect(new Vector2(LEFT_MENU_THIRD_OF_WIDTH, (4.0f + i) * LEFT_MENU_BUTTON_HEIGHT), new Vector2(LEFT_MENU_THIRD_OF_WIDTH, LEFT_MENU_BUTTON_HEIGHT)), selectedNode.daOutcomes[i].sWord);
                if(GUI.Button(new Rect(new Vector2(2.0f * LEFT_MENU_THIRD_OF_WIDTH, (4.0f + i) * LEFT_MENU_BUTTON_HEIGHT), new Vector2(LEFT_MENU_THIRD_OF_WIDTH, LEFT_MENU_BUTTON_HEIGHT)), "Delete"))
                {
                    selectedNode.daOutcomes.RemoveAt(i);
                }
            }
        }
    }
}
