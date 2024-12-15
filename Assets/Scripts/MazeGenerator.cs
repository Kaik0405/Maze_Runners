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
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

public struct Cells
    {
        public int x, y;
        public Cells(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
public class MazeGenerator : MonoBehaviour
{
    public GameObject CellObjectScene;
    GameObject CellObject;
    Image CellImage;
    const int Width = 13;
    const int Height = 13;
    const bool Wall = true; //representa una pared
    const bool Path = false; //representa un camino
    public static Cell[,] Maze = new Cell[Height + 2, Width + 2];
    public static Cells cellEnd;
    public static GameObject[,] gameObjects = new GameObject[Maze.GetLength(0),Maze.GetLength(1)] ;
    public static List<GameObject> TeleportZones = new List<GameObject>();
    void Awake()
    {
        CreateMatrix();
        GenerateMaze(1, 1);
        GenerateTeleports();
        GenerateTramps();
        CellObject = CellObjectScene;
        CellImage = CellObject.GetComponent<Image>();
        GenerateMazeInScene();
        AsiganateValuesToCellsScrips();
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
    void GenerateMaze(int x,int y) // Algoritmo recursivo para generar el laberinto
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
    Cells LongestPath(Cell[,] maze) // código recursivo para determinar el camino mas largo del laberinto
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
    void GenerateMazeInScene() //instanciacion en escena de las celdas del laberinto
    {
        Maze[1,1].Start = true;
        Cells finish = LongestPath(Maze);
        cellEnd = new Cells(finish.x,finish.y);

        Maze[finish.x,finish.y].FinishLine = true;

        for (int i = 0; i < Maze.GetLength(0); i++)
        {
            for (int j = 0; j < Maze.GetLength(1); j++)
            {
                Maze[i,j].PosX = i;
                Maze[i,j].PosY = j;
                
                if(Maze[i,j].Obstacle == true)
                {
                    CellImage.color = Color.grey;
                    GameObject cellObjectReference = Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);

                    gameObjects[i,j] = cellObjectReference;    
                }
                else if(Maze[i,j].Start == true)
                {
                    CellImage.color = Color.blue;
                    GameObject cellObjectReference = Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);

                    gameObjects[i,j] = cellObjectReference;
                }
                else if(Maze[i,j].FinishLine == true)
                {
                    CellImage.color = Color.red;
                    GameObject cellObjectReference = Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);

                    gameObjects[i,j] = cellObjectReference;
                }
                else
                {  
                    if(Maze[i,j].cellTeleport)
                    {
                        CellImage.color = Color.cyan;
                        gameObjects[i,j] = Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);
                        TeleportZones.Add(gameObjects[i,j]);
                    }

                    else if(Maze[i,j].Tramp)
                    {
                        CellImage.color = Color.magenta;
                        gameObjects[i,j] = Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);
                    }
                    else
                    {
                        CellImage.color = Color.white;
                        gameObjects[i,j] = Instantiate(CellObject,CellObject.transform.position,CellObject.transform.rotation);
                    }
                                
                }
            }
        }
    }
    private TrampType GenerateTrampType() // metodo para generar un tipo de trampa aleatorio
    {
        System.Random ranNumber = new System.Random();
        int aleatory = ranNumber.Next(1,5);

        switch (aleatory)
        {
            case 1:
                return TrampType.Freeze;
            case 2:
                return TrampType.Sleep;
            case 3:
                return TrampType.Spines;
            case 4:
                return TrampType.returnInit;
            default:
                return TrampType.Unknown;        
        }
    }
    void GenerateTramps() // Metodo para generar las trampas de forma aleatoria
    {
        System.Random ranNumber = new System.Random();

        for (int i = 0;i<Maze.GetLength(0);i++)
        {
            for (int j = 0;j<Maze.GetLength(1);j++)
            {
                if(!Maze[i,j].Obstacle && !Maze[i,j].cellTeleport) // Detecta si la posicion de la matriz no es un obtaculo
                {
                    int aleatory = ranNumber.Next(1,101);
                    
                    if(Contains(aleatory)) 
                    {
                        Maze[i,j].Tramp = true;
                        Maze[i,j].trampType = GenerateTrampType(); //Asigna un enum aleatorio del tipo de trampa
                    }
                }

            }
        }
    }
    private bool Contains(int num)
    {
        int[] numbers = {4,15,56,83,31,77,99,29,62,45}; // frecuencia de generacion de trampas 10%
        
        for(int i=0;i<numbers.Length;i++)
            if(numbers[i] == num) return true;

        return false;    
    }
    private void AsignateTeleportZone(List<Cells> zonesTeleport) //Metodo para asignar la propiedad de Zona de Teletransporte a la celda correspondiente
    {
        foreach(Cells item in zonesTeleport){
            Maze[item.x,item.y].cellTeleport = true;
        }
    }
    void GenerateTeleports() // Metodo para generar las zonas de teletransporte 
    {
        List<Cells> zonesTeleport = new List<Cells>();
        bool[,] mazeAux = new bool[Maze.GetLength(0),Maze.GetLength(1)];
        CopyValues(mazeAux);
        Cells finishCell = LongestPath(Maze);
        GenerateTeleports(mazeAux,zonesTeleport,finishCell);
        AsignateTeleportZone(zonesTeleport);
    }
    private void GenerateTeleports(bool[,] booleanMask,List<Cells> zonesTeleport,Cells cellFinish)
    {
        for(int i = 0; i < booleanMask.GetLength(0); i++)
        {
            for(int j = 0;j < booleanMask.GetLength(1); j++)
            {
                if(!booleanMask[i,j]&&NotExit(booleanMask,i,j))
                {
                    if((i==1&&j==1)||(i == cellFinish.x && j==cellFinish.y)) continue;
                    
                    else 
                        zonesTeleport.Add(new Cells(i,j));
                }
            }
        }
    }
    private bool NotExit(bool[,] booleanMask,int i,int j) //Metodo para verificar que la celda no tiene salida
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };
        int count = 0;

        for(int s = 0;s<dx.Length;s++)
        {
            int sumX = i + dx[s];
            int sumY = j + dy[s];

            if(Verification(sumX,sumY,booleanMask.GetLength(0)-1,booleanMask.GetLength(1)-1))
            {
                if(booleanMask[sumX,sumY])
                    count++;
            }  
        }
        if(count == 3) return true;

        return false;
    }
    private void AsiganateValuesToCellsScrips() //Metodo para asignarle a la matriz de referencia las propiedades de las celdas
    {
        for(int i = 0; i < Maze.GetLength(0); i++)
        {
            for (int j = 0; j < Maze.GetLength(1); j++)
            {
                gameObjects[i,j].GetComponent<CellDisplay>().cell = Maze[i,j];
            }
        }
    }
    
}
