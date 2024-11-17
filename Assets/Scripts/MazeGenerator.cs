using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Security;
using Unity.Mathematics;

public class MazeGenerator : MonoBehaviour
{
    public GameObject CellObjectScene;
    GameObject CellObject;
    Image CellImage;
    const int Width = 19;
    const int Height = 15;
    const bool Wall = true; //representa una pared
    const bool Path = false; //representa un camino
    public static Cell[,] Maze = new Cell[Height + 2, Width + 2]; 
    private struct Cells
    {
        public int x, y;
        public Cells(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    void Awake()
    {
        CreateMatrix();
        GenerateMaze(1, 1);
        CellObject = CellObjectScene;
        CellImage = CellObject.GetComponent<Image>();
        GenerateMazeInScene();

    }
    private void CreateMatrix() //Metodo para generar una matriz de celdas clasificadas como obstaculos
    {
        for (int i = 0;i<Maze.GetLength(0);i++)
            for (int j = 0;j<Maze.GetLength(1);j++)    
                Maze[i,j] = new Cell(Wall);        
    }    
    private bool IsInBounds(int x, int y) // Método para verificar si una celda esta en una posición valida de la matriz
    {
        return x >= 0 && x < Maze.GetLength(0) && y >= 0 && y < Maze.GetLength(1); // Ajusta los limites
    }
    private void Shuffle(Cells[] directions) //Selecciona las celdas en el Radio 2 de la celda actual y las mezcla de forma aleatoria 
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < directions.Length; i++)
        {
            int j = rand.Next(i, directions.Length);
            Cells temp = directions[i];
            directions[i] = directions[j];
            directions[j] = temp;
        }
    }
    private void GenerateMaze(int x,int y) // Algoritmo recursivo para generar el laberinto
    {
        Maze[x, y].Obstacle = Path;

        Cells[] directions = new Cells[]
        {
            new Cells(0, 2), 
            new Cells(2, 0),  
            new Cells(0, -2),  
            new Cells(-2, 0)   
        };

        Shuffle(directions); // Mezcla las direcciones

        foreach (var direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (IsInBounds(newX, newY) && Maze[newX, newY].Obstacle == Wall) // Verifica si la dirección a la que se movió es valida y si es una pared
            {
                Maze[x + direction.x / 2, y + direction.y / 2].Obstacle = Path; // Elimina la pared del medio
                GenerateMaze(newX, newY);
            }
        }
    }
    private bool Verification(int x, int y, int li, int lj) //Metodo para verificar si la matriz esta en rango
    {
        if (x < 0 || y < 0) return false;
        if (x > li || y > lj) return false;

        return true;
    }
    private bool[,] CopyValues(bool[,] matrix) // metodo para hacer una copia de la matriz original
    {
        for (int i = 0;i<matrix.GetLength(0);i++)        
            for(int j = 0;j<matrix.GetLength(1);j++)
                matrix[i,j] = Maze[i,j].Obstacle;

        return matrix;    
    }
    private Cells LongestPath(Cell[,] maze) // código recursivo para determinar el camino mas largo del laberinto
    {
        int max = 0;
        Cells cellF = new Cells(0,0);
        bool[,] mazeAux = new bool[Maze.GetLength(0), Maze.GetLength(1)];
        
        CopyValues(mazeAux);
        LongestPath(mazeAux,ref cellF,ref max);

        return cellF;
    } 
    private void LongestPath(bool[,] booleanMask,ref Cells cellF,ref int max,int currentMax = 0,int x = 1,int y = 1) 
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        for(int s = 0;s < dx.Length;s++)
        {
            int sumX = x + dx[s];
            int sumY = y + dy[s];

            if(Verification(sumX,sumY,booleanMask.GetLength(0),booleanMask.GetLength(1)) && !booleanMask[sumX,sumY])
            {
                if(!booleanMask[sumX,sumY])
                {
                    booleanMask[sumX,sumY]=true;
                    LongestPath(booleanMask,ref cellF,ref max,currentMax + 1,sumX,sumY);
                    booleanMask[sumX,sumY]=false;
                }
            }
        }
        if(currentMax > max)
        {
            max = currentMax;
            cellF.x = x;
            cellF.y = y;
        }
    }
    private void GenerateMazeInScene() //instanciacion en escena de las celdas del laberinto
    {
        Maze[1,1].Start = true;
        Cells finish = LongestPath(Maze);
        Maze[finish.x,finish.y].FinishLine = true;

        for (int i = 0; i < Maze.GetLength(0); i++)
        {
            for (int j = 0; j < Maze.GetLength(1); j++)
            {
                if(Maze[i,j].Obstacle == true)
                {
                    CellImage.color = Color.grey;
                    Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);    
                }
                else if(Maze[i,j].Start == true)
                {
                    CellImage.color = Color.blue;
                    Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);
                }
                else if(Maze[i,j].FinishLine == true)
                {
                    CellImage.color = Color.red;
                    Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);
                }
                else
                {
                    CellImage.color = Color.white;
                    Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);
                }
            }
        }
    }
    public void GenerateTramps() // metodo para generar las trampas de forma aleatoria
    {
        //type code here....
    }
    public void GenerateTeleports() // metodo para generar las zonas de teletransporte 
    {
        //type code here....
    }
}
