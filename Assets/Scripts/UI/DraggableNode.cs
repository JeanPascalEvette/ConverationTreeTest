using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DraggableNode : EditorWindow, SelectableItem
{
    public static float DRAGGABLE_NODE_DEFAULT_WIDTH = 40.0f;
    public static float DRAGGABLE_NODE_DEFAULT_HEIGHT = 20.0f;

    public string sName;
    public Vector2 vPosStart;
    public Vector2 vSize;

    private bool bIsSelected;
    private bool bIsDragged;

    public DraggableNode()
    {
        sName = "";
        vPosStart = Vector2.zero;
        vSize = Vector2.zero;
        bIsSelected = false;
    }

    public void Init(string _sName, Vector2 _vPosStart, Vector2 _vSize)
    {
        sName = _sName;
        vPosStart = _vPosStart;
        vSize = _vSize;
    }

    public virtual void Draw(Vector2 _vNodeOffset)
    {
        Handles.BeginGUI();
        Handles.color = Color.white;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        Handles.DrawSolidRectangleWithOutline(new Rect(_vNodeOffset + vPosStart, vSize), Color.white, Color.black);
        Handles.Label(new Vector3(_vNodeOffset.x + vPosStart.x, _vNodeOffset.y + vPosStart.y, 0.0f), sName);
        Handles.EndGUI();
        /*
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.black;
        EditorGUI.DrawRect(new Rect(_vNodeOffset + vPosStart, vSize), Color.white);
        EditorGUI.LabelField(new Rect(_vNodeOffset + vPosStart, vSize), sName, labelStyle);
        */
    }

    public virtual void DrawBackground(Vector2 _vNodeOffset)
    {

    }

    public bool IsPosWithin(Vector2 _vPos)
    {
        return _vPos.x > vPosStart.x && _vPos.x < vPosStart.x + vSize.x &&
               _vPos.y > vPosStart.y && _vPos.y < vPosStart.y + vSize.y;
    }

    public bool IsBeingDragged()
    {
        return bIsDragged;
    }

    public void HandleMouseEvents(Event _e, Vector2 _vNodesPosOffset)
    {
        if(_e.type == EventType.MouseDown)
        {
            bIsSelected = true;
            bIsDragged = true;
        }
        else if (_e.type == EventType.MouseUp)
        {
            bIsDragged = false;
        }
        else if (bIsDragged  && _e.type == EventType.MouseDrag)
        {
            vPosStart += _e.delta;
            if (vPosStart.x < LeftMenu.LEFT_MENU_WIDTH - _vNodesPosOffset.x)
                vPosStart.x = LeftMenu.LEFT_MENU_WIDTH - _vNodesPosOffset.x;
        }
    }
}
