using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCell : MonoBehaviour
{
    GameObject Maze;
    public GameObject AddMaze;

    void Start()
    {
        Maze = GameObject.Find("Maze");
        AddMaze.transform.SetParent(Maze.transform);
        AddMaze.transform.transform.localScale = Vector3.one;
        AddMaze.transform.position = new Vector3(transform.position.x, transform.position.y, -40);
        AddMaze.transform.eulerAngles = new Vector3(25,0,0);
    }

}
