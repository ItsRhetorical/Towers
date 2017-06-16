using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Connections : MonoBehaviour {

    public List<GameObject> NodeList = new List<GameObject>();
    public List<Vector3> ConnectionEndpoints = new List<Vector3>();

    // Use this for initialization
    void Start () {
        

    }

    // Update is called once per frame
    void Update() {
        DrawConnections();
    }

    public void DrawConnections()
    {
            ConnectionEndpoints.Clear();

        if (name == "Preview Tower")
        {
            NodeList.Clear();
            FindNearbyTowers(true);
        }
            
            foreach (GameObject node in NodeList)
            {
                //we have to bounce the line back and forth to look like the lines originate from the object
                ConnectionEndpoints.Add(transform.position); //add self
                ConnectionEndpoints.Add(node.transform.position); //add next node in list
            }

           // print("I am " + name + ", Connected to " + NodeList.Count + " Showing connections " + (ConnectionEndpoints.Count / 2));

            GetComponent<LineRenderer>().positionCount = ConnectionEndpoints.Count;
            GetComponent<LineRenderer>().SetPositions(ConnectionEndpoints.ToArray());
            GetComponent<LineRenderer>().sortingLayerName = "Player";
        
    }

    public void FindNearbyTowers(bool previewMode)
    {
        var PlayerInterface = GameObject.Find("Main Camera").GetComponent<PlayerInterface>();
        foreach (GameObject node in PlayerInterface.allNodesList)
        {
            //print("Distance to " + node.name + " is " + Vector3.Distance(newTowerObj.transform.position, node.transform.position));
            if (Vector3.Distance(transform.position, node.transform.position) <= PlayerInterface.TowerConnectionDistance)
            {
                NodeList.Add(node);
                if (!previewMode)
                    node.GetComponent<Connections>().NodeList.Add(this.gameObject);

                //    print("Connecting:"+node.name+node.transform.position.ToString()+" and "+newTowerObj.name + newTowerObj.transform.position.ToString());
            }

        }
    }
}
