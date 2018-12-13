using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LeftMenu : EditorWindow
{
    public static float LEFT_MENU_WIDTH = 300.0f;


    public void Draw()
    {
        Vector2 vPosStart = new Vector2(0, 0);
        Vector2 vSize = new Vector2(LEFT_MENU_WIDTH, maxSize.y); 
        EditorGUI.DrawRect(new Rect(vPosStart, vSize), Color.grey);
    }
}
