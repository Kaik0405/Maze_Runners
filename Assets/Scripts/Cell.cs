using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell:ScriptableObject
{
    public bool Obstacle{get;set;}
    public bool Tramp{get;set;}
    public bool cellTeleport{get;set;}
    public bool Start{get;set;}
    public bool FinishLine{get;set;}
    public TrampType trampType = TrampType.Unknown;

    public Cell(bool Obstacle)
    {
        this.Obstacle = Obstacle;  
    } 
}
public enum TrampType{
    returnInit, //mueve la ficha para la salida
    Freeze,     //evita que la ficha se mueva mas
    Sleep,      //evita que la ficha se mueva por 2 turnos
    Spines,     //hace que la ficha se mueva la mitad de los pasos restantes
    Unknown

}