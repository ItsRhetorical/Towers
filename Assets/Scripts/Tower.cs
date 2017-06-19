using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{

    public List<Neighbor> NeighborNodes = new List<Neighbor>();
    public List<Vector3> ConnectionEndpoints = new List<Vector3>();
    public PlayerInterface PlayerInterface;
    public int Energy;

    [System.Serializable]
    public struct Neighbor
    {
        public GameObject GameObjectNode;
        public double Distance;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DrawPreviewConnections();
    }

    public void DrawPreviewConnections()
    {
        ConnectionEndpoints.Clear();
        NeighborNodes.Clear();
        AddNearNeighbors(true);

        DrawConnections();
    }

    public void DrawConnections()
    {
        foreach (Neighbor node in NeighborNodes)
        {
            //we have to bounce the line back and forth to look like the lines originate from the object
            ConnectionEndpoints.Add(transform.position); //add self
            ConnectionEndpoints.Add(node.GameObjectNode.transform.position); //add next node in list
        }

        // print("I am " + name + ", Connected to " + NeighborNodes.Count + " Showing connections " + (ConnectionEndpoints.Count / 2));

        GetComponent<LineRenderer>().positionCount = ConnectionEndpoints.Count;
        GetComponent<LineRenderer>().SetPositions(ConnectionEndpoints.ToArray());
        GetComponent<LineRenderer>().sortingLayerName = "Player";

    }

    public void AddNearNeighbors(bool previewMode)
    {
        foreach (GameObject node in PlayerInterface.allNodesList)
        {
            double dist_between = Vector3.Distance(transform.position, node.transform.position);
            //print("Distance to " + node.name + " is " + Vector3.Distance(newTowerObj.transform.position, node.transform.position));
            if (dist_between <= PlayerInterface.TowerConnectionDistance && gameObject != node)
            {
                Neighbor nodeNeighbor = new Neighbor();
                nodeNeighbor.GameObjectNode = node;
                nodeNeighbor.Distance = dist_between;
                NeighborNodes.Add(nodeNeighbor);
                if (!previewMode)
                {
                    Neighbor selfNeighbor = new Neighbor();
                    selfNeighbor.GameObjectNode = this.gameObject;
                    selfNeighbor.Distance = dist_between;
                    node.GetComponent<Tower>().NeighborNodes.Add(selfNeighbor);

                }

                //    print("Connecting:"+node.name+node.transform.position.ToString()+" and "+newTowerObj.name + newTowerObj.transform.position.ToString());
            }

        }
    }
}

/*
 Code to add Text UI elements (have issues converrting from canvas sapce to world space
        GameObject UITextObject = new GameObject("My Test");
        UITextObject.transform.SetParent(Canvas.transform);
        UITextObject.transform.position = this.transform.position;
        Text TextComponent = UITextObject.AddComponent<Text>();
        TextComponent.text = Energy.ToString();
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        TextComponent.font = ArialFont;
        TextComponent.material = ArialFont.material;
        TextComponent.color = new Color(255, 0, 0);
*/