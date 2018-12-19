using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditLinkContentPopup : PopupWindowContent
{
    private NodeLink cLink;
    private List<string> daKeyWords;

    private float fVerticalOffset;

    public EditLinkContentPopup(NodeLink _cLink)
    {
        cLink = _cLink;
        daKeyWords = new List<string>(_cLink.daKeywords);
        fVerticalOffset = 0.0f;
    }

    public override void OnGUI(Rect rect)
    {
        Vector2 vMaxSize = this.GetWindowSize();
        float fLineHeight = 30.0f;
        float fButtonWidth = 50.0f;

        float fCurrentX = 0.0f;
        float fCurrentY = 0.0f;
        for (int i = 0; i < daKeyWords.Count; i++)
        {
            fCurrentX = 0.0f;
            daKeyWords[i] = GUI.TextField(new Rect(fCurrentX, fCurrentY - fVerticalOffset, vMaxSize.x - fButtonWidth, fLineHeight), daKeyWords[i]);
            fCurrentX += vMaxSize.x - fButtonWidth;
            if(GUI.Button(new Rect(fCurrentX, fCurrentY - fVerticalOffset, fButtonWidth, fLineHeight), "Delete"))
            {
                daKeyWords.RemoveAt(i);
            }
            fCurrentY += fLineHeight;
        }

        if (GUI.Button(new Rect(0.0f, vMaxSize.y - fLineHeight, vMaxSize.x * 0.5f, fLineHeight), "Add"))
        {
            daKeyWords.Add("");
        }
        if (GUI.Button(new Rect(vMaxSize.x * 0.5f, vMaxSize.y - fLineHeight, vMaxSize.x * 0.5f, fLineHeight), "OK"))
        {
            cLink.daKeywords = daKeyWords;
            this.editorWindow.Close();
        }

        Event e = Event.current;
        float fMaxHeight = ((daKeyWords.Count + 1) * 30.0f);
        if (fMaxHeight > vMaxSize.y)
        {
            fMaxHeight -= vMaxSize.y;
            if (e.type == EventType.ScrollWheel)
            {
                fVerticalOffset += e.delta.y;
            }

            if (fVerticalOffset < 0.0f)
                fVerticalOffset = 0.0f;
            if (fVerticalOffset > fMaxHeight)
                fVerticalOffset = fMaxHeight;
        }

        this.editorWindow.Repaint();
    }
}
