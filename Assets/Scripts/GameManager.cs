using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        GenerateMazeInScene(MazeGenerator.Maze,MazeGenerator.WallObject,MazeGenerator.PathObject);   
    }
    void Update()
    {
        
    }
    void GenerateMazeInScene(int[,] maze,GameObject WallObject,GameObject PathObject)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == 0)
                {
                    Instantiate(WallObject, WallObject.transform.position, WallObject.transform.rotation);
                }
                if (maze[i, j] == 1)
                {
                    Instantiate(PathObject, PathObject.transform.position, PathObject.transform.rotation);
                }
            }
        }
    }
}
