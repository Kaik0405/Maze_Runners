using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrampEffects
{
    public static void ReturnInit(params object[] param) //Devuelve la ficha para el inicio
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();
        currentObject.transform.SetParent(MazeGenerator.gameObjects[1,1].transform);
        currentObject.transform.position = MazeGenerator.gameObjects[1,1].transform.position;

        currentObject.GetComponent<TokenDisplay>().Token.PrePosX = currentObject.GetComponent<TokenDisplay>().Token.PosX;
        currentObject.GetComponent<TokenDisplay>().Token.PrePosY = currentObject.GetComponent<TokenDisplay>().Token.PosY;

        currentObject.GetComponent<TokenDisplay>().Token.PosX = 1;
        currentObject.GetComponent<TokenDisplay>().Token.PosY = 1;    
    }
    public static void Freeze(params object[] param) //Evita que la ficha se mueva mas por el resto del turno
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();
        currentObject.GetComponent<TokenDisplay>().Token.CurrentSpeed = 0;
    }
    public static void Sleep(params object[] param) //Evita que la ficha se mueva por 2 turnos
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();
        currentObject.GetComponent<TokenDisplay>().Token.Lock = true;
        currentObject.GetComponent<TokenDisplay>().Token.CurrentCountLock = currentObject.GetComponent<TokenDisplay>().Token.GetCountLock();        
    }
    public static void Spines(params object[] param) //Hace que la ficha se mueva la mitad de los pasos restantes
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();
        currentObject.GetComponent<TokenDisplay>().Token.CurrentSpeed = currentObject.GetComponent<TokenDisplay>().Token.CurrentSpeed/2; 
    }
    public static void Teleport(params object[] param) //Teletransporta la ficha a una zona de teletransporte aleatoria
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();
        
        System.Random random= new System.Random();
        int aleatory = random.Next(0, MazeGenerator.TeleportZones.Count);
        
        GameObject ZoneTeleports = MazeGenerator.TeleportZones[aleatory];
        
        currentObject.transform.SetParent(ZoneTeleports.transform);
        currentObject.transform.position = ZoneTeleports.transform.position;

        currentObject.GetComponent<TokenDisplay>().Token.PosX = ZoneTeleports.GetComponent<CellDisplay>().cell.PosX;
        currentObject.GetComponent<TokenDisplay>().Token.PosY = ZoneTeleports.GetComponent<CellDisplay>().cell.PosY;
    }
}//El teletransporte es un caso particular de trampa que no se considera trampa lo que está aquí para evitar repetir código ;)