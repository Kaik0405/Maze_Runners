using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool Obstacle{get;set;}
    public bool Tramp{get;set;}
    public bool cellTeleport{get;set;}
    public bool Start{get;set;}
    public bool FinishLine{get;set;}

    public Cell(bool Obstacle)
    {
        this.Obstacle = Obstacle;  
    } 
}
