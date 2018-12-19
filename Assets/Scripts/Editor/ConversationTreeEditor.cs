using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

[InitializeOnLoad]
public class ConversationTreeEditor : EditorWindow
{
    private static ConversationTreeEditor _instance;

    public static ConversationTreeEditor Instance { get { return _instance; } }
    
    static ConversationTreeEditor()
    {
        EditorApplication.update += Update;
    }

    List<ConversationNode> daNodes;
    Vector2 vNodesPosOffset;
    bool bIsScrollingOffset;
    LeftMenu menu;
    ConversationNode nodeCurrentlyLinked;
    ConversationNode nodeCurrentlySelected;

    [MenuItem("Window/ConversationTreeEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ConversationTreeEditor));
    }

    static void Update()
    {
        if (!_instance)
        {
            _instance = ScriptableObject.CreateInstance<ConversationTreeEditor>();
            _instance.daNodes = new List<ConversationNode>();
            _instance.menu = ScriptableObject.CreateInstance<LeftMenu>();
            _instance.nodeCurrentlyLinked = null;
            _instance.nodeCurrentlySelected = null;

            _instance.vNodesPosOffset = Vector2.zero; 
            _instance.bIsScrollingOffset = false;
            _instance.LoadFromJSON();
        }
    }

    void OnGUI()
    {
        if (!_instance) 
            return;

        _instance.HandleInputs();
        _instance.HandleUpdate();
        _instance.HandleRender();
    }

    void HandleInputs()
    {
        bool bIsUsed = false;
        Event e = Event.current;
        if (e.type == EventType.Layout || e.type == EventType.Repaint)
            return;

        if (e.keyCode == KeyCode.E)
        {
            bool bFoundNode = false;
            foreach (ConversationNode Node in _instance.daNodes)
            {
                if (Node.IsPosWithin(e.mousePosition - GetNodesOffset()))
                {
                    bFoundNode = true;
                    if (e.type == EventType.KeyDown && nodeCurrentlyLinked == null)
                    {
                        nodeCurrentlyLinked = Node;
                        break;
                    }
                    if (e.type == EventType.KeyUp && nodeCurrentlyLinked != null)
                    {
                        nodeCurrentlyLinked.LinkTo(new NodeLink(new List<string> { }, Node));
                        nodeCurrentlyLinked = null;
                        bIsUsed = true;
                        break;
                    }
                }
            }
            if(!bFoundNode && e.type == EventType.KeyUp && nodeCurrentlyLinked != null)
            {
                nodeCurrentlyLinked = null;
                bIsUsed = true;
            }
        }
        else
        {
            if (e.type == EventType.MouseDown)
            {
                GUI.FocusControl(null);
                if (nodeCurrentlySelected)
                    nodeCurrentlySelected.SetSelected(false);
            }
            foreach (ConversationNode Node in _instance.daNodes)
            {
                if(Node.IsPosWithin(e.mousePosition - GetNodesOffset()) && e.button == 0 && e.type == EventType.MouseDown)
                {
                    nodeCurrentlySelected = Node;
                    nodeCurrentlySelected.SetSelected(true);

                }
                if ((Node.IsBeingDragged() && (e.type == EventType.MouseDrag || e.type == EventType.MouseUp)) ||
                    (Node.IsPosWithin(e.mousePosition - GetNodesOffset()) && e.button == 0))
                {
                    bIsUsed = true;
                    Node.HandleMouseEvents(e, vNodesPosOffset);
                    break;
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

    void HandleUpdate()
    {

    }

    void HandleRender()
    {
        if (_instance)
        {
            Handles.BeginGUI();
            foreach (DraggableNode Node in _instance.daNodes)
            {
                Node.DrawBackground(GetNodesOffset());
            }
            foreach (DraggableNode Node in _instance.daNodes)
            {
                Node.Draw(GetNodesOffset());
            }
            _instance.menu.Draw();
            if (nodeCurrentlyLinked != null)
            {
                Vector2 vP1 = GetNodesOffset() + nodeCurrentlyLinked.vPosStart + nodeCurrentlyLinked.vSize * 0.5f;
                Vector2 vP2 = Event.current.mousePosition;
                Handles.color = Color.magenta;
                Handles.DrawLine(vP1, vP2);
            }
            Handles.EndGUI();
        }
        UnityEditor.HandleUtility.Repaint();
    }

    public void SaveToJSON()
    {
        Node[] daJsonNodes = new Node[daNodes.Count];
        for(int i = 0; i < daNodes.Count; i++)
        {
            Node n = new Node();
            n.sName = daNodes[i].sName;
            n.sText = daNodes[i].sText;
            n.vPosStart = daNodes[i].vPosStart;
            n.iID = daNodes[i].iID;
            foreach(NodeLink link in daNodes[i].daOutcomes)
            {
                n.daOutcomes.Add(new Link(link.daKeywords.ToArray(), link.node.iID));
            }
            daJsonNodes[i] = n;
        }

        string json = JsonHelper.ToJson(daJsonNodes, true);

        string destination = "Assets/Export/TreeNodes.json";
        FileStream file = new FileStream(destination, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        byte[] data = Encoding.ASCII.GetBytes(json);
        file.Write(data, 0, data.Length);
        file.Close();
    }

    public void LoadFromJSON()
    {
        string source = "Assets/Export/TreeNodes.json";
        using (StreamReader streamReader = File.OpenText(source))
        {
            string jsonString = streamReader.ReadToEnd();
            Node[] daJsonNodes = JsonHelper.FromJson<Node>(jsonString);

            daNodes.Clear();
            uint iHighestID = 0;
            for (int i = 0; i < daJsonNodes.Length; i++)
            {
                ConversationNode n = ScriptableObject.CreateInstance<ConversationNode>();
                n.Init(daJsonNodes[i].sName, daJsonNodes[i].sText, daJsonNodes[i].vPosStart);
                n.iID = daJsonNodes[i].iID;
                iHighestID = (iHighestID > n.iID) ? iHighestID : n.iID;
                daNodes.Add(n);
            }

            DraggableNode.iIDCount = iHighestID + 1;

            List<string> daKeyWords;
            for (int i = 0; i < daJsonNodes.Length; i++)
            {
                foreach (Link link in daJsonNodes[i].daOutcomes)
                {
                    for (int u = 0; u < daNodes.Count; u++)
                    {
                        if (link.iID == daNodes[u].iID)
                        {
                            if (link.daKeywords != null)
                                daKeyWords = new List<string>(link.daKeywords);
                            else
                                daKeyWords = new List<string> { };
                            daNodes[i].daOutcomes.Add(new NodeLink(daKeyWords, daNodes[u]));
                            break;
                        }
                    }
                }
            }
        }
    }

    private Vector2 GetNodesOffset()
    {
        return vNodesPosOffset + new Vector2(LeftMenu.LEFT_MENU_WIDTH, 0.0f);
    }

    public void AddNode()
    {
        ConversationNode n = ScriptableObject.CreateInstance<ConversationNode>();
        n.sName = "";
        n.vPosStart = -vNodesPosOffset;
        daNodes.Add(n);
    }

    public void RemoveNode()
    {
        if(nodeCurrentlySelected)
        {
            daNodes.Remove(nodeCurrentlySelected);
        }
    }

    public ConversationNode GetSelectedNode()
    {
        return nodeCurrentlySelected;
    }
}
