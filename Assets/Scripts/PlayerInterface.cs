using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInterface : MonoBehaviour {

    public GameObject Tower;
    public double TowerConnectionDistance;
    public List<GameObject> allNodesList = new List<GameObject>();
    private GameObject PreviewTower;
    private GameObject Tower1;
    private GameObject Tower2;

    // Use this for initialization
    void Start () {
        PreviewTower = BuildTower(Get2DMousePosition(), "Preview Tower", "Preview",0, "preview");
        Tower1 = BuildTower(new Vector2(10,-5), "Tower (source)", "Tower",100, "source");
        Tower2 = BuildTower(new Vector2(-10.0f,5.0f), "Tower (end)", "Tower",0, "sink");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            BuildTower(Get2DMousePosition(), "Tower (" + allNodesList.Count + ")","Tower",0,"normal");
        }
            PreviewTower.transform.position = Get2DMousePosition();
    }

    public Vector2 Get2DMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.orthographicSize;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        //print(mousePos.x);
        //print(mousePos.y);
        return mousePos2d;
    }

    public GameObject BuildTower(Vector2 Position, string name, string tag, int energy, string type)
    {
        GameObject newTower = Instantiate(Tower, Position, Quaternion.identity) as GameObject;

        newTower.name = name;
        newTower.tag = tag;
        newTower.GetComponent<Tower>().Energy = energy;
        newTower.GetComponent<Tower>().Type = type;

        if (tag == "Preview")
        {
            newTower.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .2f);

        }
        else if (tag == "Tower")
        {
            AddNode(newTower, Position);
            newTower.GetComponent<Tower>().DrawConnections();
        }

        
        newTower.SetActive(true);

        return newTower;
    }

    public GameObject AddNode(GameObject Structure, Vector2 Position)
    {
        allNodesList.Add(Structure); // add this node to the main list
        Structure.GetComponent<Tower>().AddNearNeighbors(false); // adds nearby connections

        return Structure;
    }
    public void RemoveNode(GameObject Structure)
    {
        // look through my own list of neighbor connections and delete the any references my neighbor has to me
       foreach( Tower.Neighbor node in Structure.GetComponent<Tower>().NeighborNodes)
        {
            node.GameObjectNode.GetComponent<Tower>().NeighborNodes.Remove(node);
        }
        allNodesList.Remove(Structure); // remove from main node list

        Structure.GetComponent<Tower>().NeighborNodes.Clear();
    }

}
