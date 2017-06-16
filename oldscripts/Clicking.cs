using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Clicking : MonoBehaviour {

    public GameObject HomeBase;
    public GameObject Tower;
    public Graph<GameObject> towerGraph = new Graph<GameObject>();
    
    // Use this for initialization
    void Start (){
        GraphNode<GameObject> BaseNode = new GraphNode<GameObject>(HomeBase);
        towerGraph.Add(BaseNode);
        print(towerGraph[0].Data.transform.position.x.ToString());
        print(towerGraph[0].Data.transform.position.y.ToString());

    }

    // Update is called once per frame
    void Update () {
        
		if (Input.GetMouseButtonDown (0)) 
		{

            PlaceTower(Get2DMousePosition());

        }
		if (Input.GetMouseButtonDown (1)) 
		{
			//print ("Right Clicked!");
		}
		if (Input.GetMouseButtonDown (2)) 
		{
            var x = 1;
			//print ("Center Clicked!");
		}
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

    public void PlaceTower(Vector2 position)
    {
        //list of other towers
        GameObject[] Towers = GameObject.FindGameObjectsWithTag("Tower");

        //add new tower GameObject
        GameObject newTowerObj = Instantiate(Tower, Get2DMousePosition(), Quaternion.identity) as GameObject;
        //add our object to the graph
        GraphNode<GameObject> towerObjNode = new GraphNode<GameObject>(newTowerObj);
        // rename it to match/be unique
        towerObjNode.Data.name = "Tower (" + (towerGraph.Count - 1) + ")";

        //look through a list of other towers and find ones within 1
        foreach (GameObject obj in Towers)
        {
            if (Vector3.Distance(obj.transform.position,newTowerObj.transform.position) >= 1)
            {
                towerGraph.AddUndirectedEdge(towerGraph.Find, towerObjNode);
            }
        }
        


        
    }
}
