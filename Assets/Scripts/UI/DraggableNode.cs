using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DraggableNode : EditorWindow
{
    public static uint iIDCount = 0;
    public static float DRAGGABLE_NODE_DEFAULT_WIDTH = 40.0f;
    public static float DRAGGABLE_NODE_DEFAULT_HEIGHT = 20.0f;

    public string sName;
    public Vector2 vPosStart;
    public Vector2 vSize;
    public uint iID;

    private bool bIsSelected;
    private bool bIsDragged;

    public DraggableNode()
    {
        iID = iIDCount++;
        sName = "";
        vPosStart = Vector2.zero;
        vSize = new Vector2(DRAGGABLE_NODE_DEFAULT_WIDTH, DRAGGABLE_NODE_DEFAULT_HEIGHT);
        bIsSelected = false;
    }

    public void Init(string _sName, Vector2 _vPosStart)
    {
        sName = _sName;
        vPosStart = _vPosStart;
        vSize = Vector2.zero;
    }

    public virtual void Draw(Vector2 _vNodeOffset)
    {
        if(vSize == Vector2.zero)
        {
            vSize = GUI.skin.label.CalcSize(new GUIContent(sName));
        }

        Handles.color = Color.white;
        Color cBorder = Color.white;
        if (bIsSelected)
            cBorder = Color.black;
        Handles.DrawSolidRectangleWithOutline(new Rect(_vNodeOffset + vPosStart, vSize), Color.white, cBorder);
        Handles.Label(new Vector3(_vNodeOffset.x + vPosStart.x, _vNodeOffset.y + vPosStart.y, 0.0f), sName);
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
        if (_e.type == EventType.MouseDown)
        {
            if (vPosStart.x + vSize.x > -_vNodesPosOffset.x)
            {
                bIsDragged = true;
            }
        }
        else if (_e.type == EventType.MouseUp)
        {
            bIsDragged = false;
        }
        else if (bIsDragged && _e.type == EventType.MouseDrag)
        {
            vPosStart += _e.delta;
        }
    }

    public void SetSelected(bool _bSelected)
    {
        bIsSelected = _bSelected;
    }
}
