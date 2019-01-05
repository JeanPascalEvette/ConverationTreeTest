using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class EditScriptPopup : PopupWindowContent
{
    private ConversationNode cNode;
    private string sScriptName;

    private static float F_Y_SIZE = 15.0f;

    public EditScriptPopup(ConversationNode _node)
    {
        cNode = _node;
        sScriptName = cNode.sScriptName;
    }

    public override void OnGUI(Rect rect)
    {
        Vector2 vMaxSize = this.GetWindowSize();
        float yPos = 0.0f;
        string sShortName = "";
        GUIStyle defaultStyle = new GUIStyle();
        GUIStyle activeStyle = new GUIStyle();
        activeStyle.normal.textColor = Color.green;

        string sPath = "Assets/Scripts/NodeScripts/";
        DirectoryInfo directory = new DirectoryInfo(sPath);
        FileInfo[] tFileInfo = directory.GetFiles("*.cs", SearchOption.AllDirectories);
        foreach (FileInfo file in tFileInfo)
        {
            GUIStyle fileStyle = defaultStyle;
            sShortName = file.Name.Remove(file.Name.Length - 3);
            if (sShortName == cNode.sScriptName)
                fileStyle = activeStyle;

            if (GUI.Button(new Rect(0.0f, yPos, vMaxSize.x, F_Y_SIZE), sShortName, fileStyle))
            {
                cNode.sScriptName = sShortName;
                this.editorWindow.Close();
            }

            yPos += F_Y_SIZE;
        }

        yPos += F_Y_SIZE;
        if (GUI.Button(new Rect(0.0f, yPos, vMaxSize.x, F_Y_SIZE), "None"))
        {
            cNode.sScriptName = "";
            this.editorWindow.Close();
        }
    }
}
