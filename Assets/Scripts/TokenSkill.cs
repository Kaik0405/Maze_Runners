using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TokenSkill
{
    public static void DestroyTrampInRadius(params object[] param) //Detecta todas las trampas que hay en el radio de desplazamiento
    {
        Debug.Log("HABILIDAD ACTIVADA");
    }
    public static void IncreseSpeed(params object[] param) //Aumenta el desplazamiento en 3 
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();
        currentObject.GetComponent<TokenDisplay>().Token.CurrentSpeed+=3;
    }
    public static void SendTeleportZone(params object[] param)//Envia la ficha para una zona de Teletransporte aleatoria
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();
        Debug.Log(currentObject.GetComponent<TokenDisplay>().Token.Name);
        
        System.Random random= new System.Random();
        int aleatory = random.Next(0, MazeGenerator.TeleportZones.Count);
        
        GameObject ZoneTeleports = MazeGenerator.TeleportZones[aleatory];
        
        currentObject.transform.SetParent(ZoneTeleports.transform);
        currentObject.transform.position = ZoneTeleports.transform.position;

        currentObject.GetComponent<TokenDisplay>().Token.PosX = ZoneTeleports.GetComponent<CellDisplay>().cell.PosX;
        currentObject.GetComponent<TokenDisplay>().Token.PosY = ZoneTeleports.GetComponent<CellDisplay>().cell.PosY;

    }
    public static void Exchange(params object[] param)//Intercambia dos fichas aletorias del laberinto
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();

        if(GameManager.currentPlayer == GameManager.player1)
            AsignateValues(currentObject, GameManager.player2);
        
        else
            AsignateValues(currentObject, GameManager.player1);
        
    }  
    public static void AdversaryTokenInit(params object[] param)//Manda una ficha aleatoria del campo del adversario para el inicio
    {
        
    }
    public static void MoveBackTokensAdv(params object[] param)//Retrocede en un paso todas las fichas del adversario
    {
        Debug.Log("HABILIDAD ACTIVADA");
    }
    private static void AsignateValues(GameObject currentObject, Player player)
    {
        System.Random random= new System.Random();
        int aleatory = random.Next(0, player.ObjectsInMaze.Count);

        GameObject objectAdversary = player.ObjectsInMaze[aleatory];
        Transform ParentObject = currentObject.transform.parent;

        if(objectAdversary.activeInHierarchy)
        {
            int x = currentObject.GetComponent<TokenDisplay>().Token.PosX;
            int y = currentObject.GetComponent<TokenDisplay>().Token.PosY;

            currentObject.transform.SetParent(objectAdversary.transform.parent);
            currentObject.transform.position = objectAdversary.transform.parent.position;

            currentObject.GetComponent<TokenDisplay>().Token.PosX = objectAdversary.GetComponent<TokenDisplay>().Token.PosX;
            currentObject.GetComponent<TokenDisplay>().Token.PosY = objectAdversary.GetComponent<TokenDisplay>().Token.PosY;

            objectAdversary.transform.SetParent(ParentObject);
            objectAdversary.transform.position = ParentObject.position;

            objectAdversary.GetComponent<TokenDisplay>().Token.PosX = x;
            objectAdversary.GetComponent<TokenDisplay>().Token.PosY = y;
        }
    }
}
