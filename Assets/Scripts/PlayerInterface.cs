using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInterface : MonoBehaviour {

    public GameObject Tower;
    public double TowerConnectionDistance;
    public List<GameObject> allNodesList = new List<GameObject>();

    // Use this for initialization
    void Start () {

        GameObject previewTower = Instantiate(Tower, new Vector2(0,0), Quaternion.identity) as GameObject;
        previewTower.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .2f);
        previewTower.name = "Preview Tower";
        previewTower.tag = "Preview";
        previewTower.SetActive(true);

        BuildTower(Tower, new Vector2(0, 0));
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            BuildTower(Tower,Get2DMousePosition());
        }
        GameObject previewTower = GameObject.Find("Preview Tower");
            previewTower.transform.position = Get2DMousePosition();
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

    public void BuildTower(GameObject Tower, Vector2 MousePos)
    {
        allNodesList.Clear();
        allNodesList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Tower")));

        GameObject newTowerObj = Instantiate(Tower, MousePos, Quaternion.identity) as GameObject;

        newTowerObj.GetComponent<Connections>().FindNearbyTowers(false);

        newTowerObj.name = "Tower (" + allNodesList.Count + ")";
        newTowerObj.SetActive(true);

        allNodesList.Add(newTowerObj);
        
    }


}
