using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{

    public List<Neighbor> NeighborNodes = new List<Neighbor>();
    public List<Vector3> ConnectionEndpoints = new List<Vector3>();
    public PlayerInterface PlayerInterface;
    private LineRenderer Line;
    private AnimationCurve LineWidth = new AnimationCurve();
    public double Energy;
    public string Type;
    public double ThermalCoef;

    [System.Serializable]
    public struct Neighbor
    {
        public GameObject GameObjectNode;
        public double Distance;
    }

    // Use this for initialization
    void Start()
    {

        if (this.tag != "Preview")
            InvokeRepeating("EnergyFlow", 0f, .01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tag != "Preview")
            setColor();
        DrawPreviewConnections();
    }

    public void DrawPreviewConnections()
    {
        ConnectionEndpoints.Clear();
        NeighborNodes.Clear();
        AddNearNeighbors(true);

        DrawConnections();
    }

    public void ClearCurve(AnimationCurve curve)
    {
        for (int i = 0; i < curve.length; i++)
        {
            curve.RemoveKey(i);
        }
    }

    public void DrawConnections()
    {
        Line = GetComponent<LineRenderer>();
        ClearCurve(LineWidth);
        var count = 0;
        foreach (Neighbor node in NeighborNodes)
        {
            float lineSegmentLength = 1f / (NeighborNodes.Count + 2);
            //we have to bounce the line back and forth to look like the lines originate from the object
            ConnectionEndpoints.Add(transform.position); //add self
            ConnectionEndpoints.Add(node.GameObjectNode.transform.position); //add next node in list

            LineWidth.AddKey(lineSegmentLength*count, 0.3f);
            AnimationUtility.SetKeyLeftTangentMode(LineWidth, count, AnimationUtility.TangentMode.Constant);

            LineWidth.AddKey(lineSegmentLength * (count+1f), 0.0f);
            AnimationUtility.SetKeyLeftTangentMode(LineWidth, count+1, AnimationUtility.TangentMode.Constant);

            count += 2;
        }
        Line.widthCurve = LineWidth;
        Line.widthMultiplier = 1f;

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

    public void setColor()
    {
        float colorAdjust = 1f - ((float)Energy / 100f);
        var color = GetComponent<SpriteRenderer>().color;
        color.g = colorAdjust;
        color.b = colorAdjust;

        GetComponent<SpriteRenderer>().color = color;
    }

    //modeled after heat flow dQ/dt = -K*A*dT/x
    // K is thermal coeficient ~2 for metal,dt is differnece in temp, x is distance, and A is area
    public void EnergyFlow()
    {
        foreach (Neighbor neighbor in NeighborNodes)
        {
           
            double TemperatureDiff = this.Energy - neighbor.GameObjectNode.GetComponent<Tower>().Energy;

            var rate = TemperatureDiff * ThermalCoef/ neighbor.Distance;

            if (neighbor.GameObjectNode.GetComponent<Tower>().Type == "normal")
                neighbor.GameObjectNode.GetComponent<Tower>().Energy += rate;
            if (this.Type =="normal")
                this.Energy += -rate;
            this.GetComponent<LineRenderer>().startColor = Color.red;
            this.GetComponent<LineRenderer>().endColor = Color.white;

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