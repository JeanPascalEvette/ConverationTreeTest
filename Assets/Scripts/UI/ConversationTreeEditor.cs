using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;


public class ConversationTreeEditor : EditorWindow
{
    private static ConversationTreeEditor instance;
    
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static ConversationTreeEditor()
    {
    }

    public static ConversationTreeEditor Instance
    {
        get
        {
            return instance;
        }
    }

    List<ConversationNode> daNodes;
    Vector2 vNodesPosOffset;
    bool bIsScrollingOffset;
    bool bIsLinking;
    LeftMenu menu;
    ConversationNode nodeCurrentlyLinked;

    [MenuItem("Window/ConversationTreeEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ConversationTreeEditor));
    }

    private void InitSingleton() 
    {
        instance = ScriptableObject.CreateInstance<ConversationTreeEditor>();
        instance.daNodes = new List<ConversationNode>();
        instance.menu = ScriptableObject.CreateInstance<LeftMenu>();
        nodeCurrentlyLinked = null;

        vNodesPosOffset = Vector2.zero;
        bIsScrollingOffset = false;
        bIsLinking = false;
        Vector2 vStart = new Vector2(LeftMenu.LEFT_MENU_WIDTH, 0);
        Vector2 vSize = new Vector2(DraggableNode.DRAGGABLE_NODE_DEFAULT_WIDTH, DraggableNode.DRAGGABLE_NODE_DEFAULT_HEIGHT);
        ConversationNode newNode = ScriptableObject.CreateInstance<ConversationNode>();
        newNode.Init("TEST1", vStart, vSize);
        instance.daNodes.Add(newNode);
        vStart = new Vector2(LeftMenu.LEFT_MENU_WIDTH + 60.0f, 60.0f);
        newNode = ScriptableObject.CreateInstance<ConversationNode>();
        newNode.Init("TEST2", vStart, vSize);
        instance.daNodes.Add(newNode);
        vStart = new Vector2(LeftMenu.LEFT_MENU_WIDTH + 180.0f, 180.0f);
        newNode = ScriptableObject.CreateInstance<ConversationNode>();
        newNode.Init("TEST3", vStart, vSize);
        instance.daNodes.Add(newNode);
    }

    void OnGUI()
    {
        if (!instance)
            InitSingleton(); 

        if (instance)
        {
            foreach (DraggableNode Node in instance.daNodes)
            {
                Node.DrawBackground(vNodesPosOffset);
            }
            instance.menu.Draw();
            foreach (DraggableNode Node in instance.daNodes)
            {
                Node.Draw(vNodesPosOffset);
            }
        }

        Event e = Event.current;
        if(e.type == EventType.KeyDown)
        {
            if(e.keyCode == KeyCode.E)
            {
                bIsLinking = true;
            }
        }
        else if (e.type == EventType.KeyUp)
        {
            if (e.keyCode == KeyCode.E)
            {
                bIsLinking = false;
            }
        }
        else if (e.type != EventType.Layout && e.type != EventType.Repaint)
        {
            bool bIsUsed = false;
            if (bIsLinking && e.button == 0)
            {
                foreach (ConversationNode Node in instance.daNodes)
                {
                    if (Node.IsPosWithin(e.mousePosition - vNodesPosOffset))
                    {
                        if (e.type == EventType.MouseDown)
                            nodeCurrentlyLinked = Node;
                        if (e.type == EventType.MouseUp && nodeCurrentlyLinked != null)
                        {
                            nodeCurrentlyLinked.LinkTo(new KeyWord(""), Node);
                            nodeCurrentlyLinked = null;
                            bIsUsed = true;
                        }
                    }
                }
            }
            else
            {
                foreach (ConversationNode Node in instance.daNodes)
                {
                    if ((Node.IsBeingDragged() && (e.type == EventType.MouseDrag || e.type == EventType.MouseUp)) ||
                        (Node.IsPosWithin(e.mousePosition - vNodesPosOffset) && e.button == 0))
                    {
                        bIsUsed = true;
                        Node.HandleMouseEvents(e, vNodesPosOffset);
                    }
                }

                if (e.button == 2)
                {
                    if (e.type == EventType.MouseDown)
                        bIsScrollingOffset = true;
                    if (e.type == EventType.MouseUp)
                        bIsScrollingOffset = false;
                    if (bIsScrollingOffset && e.type == EventType.MouseDrag)
                    {
                        bIsUsed = true;
                        vNodesPosOffset += e.delta;
                    }
                }
            }
            if (bIsUsed)
                e.Use();  //Eat the event so it doesn't propagate through the editor.
        }
    }

    public void SaveToJSON()
    {
        string json = JsonUtility.ToJson(daNodes);

        string destination = "Assets/Export/TreeNodes.json";
        FileStream file = new FileStream(destination, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        byte[] data = Encoding.ASCII.GetBytes(json);
        file.Write(data, 0, data.Length);
    }
}
