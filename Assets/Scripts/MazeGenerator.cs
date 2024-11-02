using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    const int Width = 9;
    const int Heigth = 7;

    const int wall = 0; //representa una pared
    const int path = 1; //representa un camino

    int[,] Maze = new int[Heigth + 2, Width + 2];
    struct Cells
    {
        public int x, y;
        public Cells(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    bool IsInBounds(int x, int y) // Metodo para verificar si una celda esta en una posicion valida de la matriz
    {
        return x > 0 && x < Width + 1 && y > 0 && y < Heigth + 1; // Ajusta los límites
    }
    void Shuffle(Cells[] directions) //Selecciona las celdas en el Radio 2 de la celda actual y las mezcla de forma aleatoria 
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
        Maze[x, y] = path;
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

            if (IsInBounds(newX, newY) && Maze[newX, newY] == wall) // Verifica si la direccion a la que se movio es valida y si es una pared
            {
                Maze[x + direction.x / 2, y + direction.y / 2] = path; // Elimina la pared del medio
                GenerateMaze(newX, newY);
            }
        }
    }
    Cells LongestPath(int[,] maze,Stack<Cells> cellTeletransport) // codigo recursivo para determinar el camino mas largo del laberinto
    {
        int max = 0;
        Cells cellF = new Cells();

        return cellF;
    } 
    void LongestPath(int[,] maze,ref int max,ref Cells cellF,Stack<Cells> cellTeletransport) 
    {
        // type code here...
        throw new NotImplementedException();
    }

    void Awake()
    {
        GenerateMaze(1, 1);

        for(int i = 0; i < Maze.GetLength(0); i++)
        {
            for(int j = 0; j < Maze.GetLength(1); j++)
            {
                //type code here....
            }
        }
    }
}
