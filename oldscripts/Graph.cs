using System;
using System.Collections.Generic;

//class Program
//{
//    static void Main(string[] args)
//    {
//        Console.WriteLine("Hello World!");
//        NodeList<string> nl = new NodeList<string>();
//        GraphNode<string> node1 = new GraphNode<string>();
//        GraphNode<string> node2 = new GraphNode<string>();
//        node1.Data = "First Node";
//        node2.Data = "Second Node";
//                nl.Add(node2);

//                Node<string> secondNodeFindObject = nl.Fin
//                dByValue("Second Node");

//                node1.Connections = nl;
//                Console.WriteLine(node1.Data);
//                Console.WriteLine(secondNodeFindObject.Data);

//                Console.WriteLine(node1.Connections[0].Data);
        
//        Graph<string> g = new Graph<string>();
//        g.AddNode(node1);
//        g.AddNode(node2);
//        GraphNode<string> node3 = g.AddNode("Node Three");
//        g.AddDirectedEdge(node1, node2);
//        g.AddUndirectedEdge(node2, node1);

//        Console.WriteLine(g.Nodes);
        
//    }
//}

public class Node<T>
{
    public T Data { get; set; }
    public NodeList<T> Connections = new NodeList<T>();

    public Node() { }
    public Node(T data) : this(data, null) { }
    public Node(T data, NodeList<T> connections)
    {
        this.Data = data;
        if (connections == null)
            this.Connections = new NodeList<T>();
        else
            this.Connections = connections;
    }

}

public class NodeList<T> : List<Node<T>>
{
    public NodeList() : base() {}
    public NodeList(Node<T> node)
    {
        base.Add(node);
    }

    public Node<T> FindByValue(T value)
    {
        var result = FindAll(item => item != null && item.Data.Equals(value));

        if (result.Count == 0)
            return null;
        return result[0];
    }

}

public class GraphNode<T> : Node<T>
{
    private List<int> costs;
    public GraphNode() : base() { }
    public GraphNode(T data) : base(data) { }
    public GraphNode(T data, NodeList<T> connections) : base(data, connections) { }
}

public class Graph<T> : List<GraphNode<T>>
{
    private NodeList<T> nodeSet;

    public Graph() : this(null) { }
    public Graph(NodeList<T> nodeSet)
    {
        if (nodeSet == null)
            this.nodeSet = new NodeList<T>();
        else
            this.nodeSet = nodeSet;
    }

    public void AddNode(GraphNode<T> node)
    {
        //add node that exists
        nodeSet.Add(node);
        //return node;
    }
    public void AddNode(Node<T> node)
    {
        //add node that exists
        nodeSet.Add(node);
        //return node;
    }
    public GraphNode<T> AddNode(T data)
    {
        //create node with only value
        GraphNode<T> node = new GraphNode<T>(data);
        nodeSet.Add(node);
        return node;
    }
    public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to)
    {
        from.Connections.Add(to);
    }
    public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to)
    {
        from.Connections.Add(to);
        to.Connections.Add(from);
    }

    /**
        new public bool Contains(T value)
        {
            //contains by node data "name"
            return nodeSet.FindByValue(value) != null;
        }
        new public int Count
        {
            get { return nodeSet.Count; }
        }
    **/
}