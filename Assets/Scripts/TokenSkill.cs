using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TokenSkill
{
    public static void DestroyTrampInRadius(params object[] param) //Detecta todas las trampas que hay en el radio de desplazamiento
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();
        int positionX = currentObject.GetComponent<TokenDisplay>().Token.PosX;
        int positionY = currentObject.GetComponent<TokenDisplay>().Token.PosY;

        int x = positionX-6, y = positionY-6;
        int dx = positionX+7, dy = positionY+7;

        for(int i = x; i < dx; i++)
            for(int j = y; j < dy; j++)
                if(Check(MazeGenerator.gameObjects,i,j))
                {
                    Cell aux = MazeGenerator.gameObjects[i,j].GetComponent<CellDisplay>().cell;
                    if(aux.Tramp && (!aux.Start) && (!aux.FinishLine)) 
                    {
                        MazeGenerator.gameObjects[i,j].GetComponent<CellDisplay>().cell.Tramp = false;
                        UpdateSprites(MazeGenerator.gameObjects[i,j]);
                    }
                }
    }
    public static void IncreseSpeed(params object[] param) //Aumenta el desplazamiento en 3 
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject(); //Referencia al objeto que se esta desplazando
        currentObject.GetComponent<TokenDisplay>().Token.CurrentSpeed+=5;
    }
    public static void SendTeleportZone(params object[] param)//Envia la ficha para una zona de Teletransporte aleatoria
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
    public static void Exchange(params object[] param)//Intercambia dos fichas aletorias del laberinto
    {
        GameObject currentObject = GameManager.currentPlayer.CurrentTokenObject();

        if(GameManager.currentPlayer.Name == GameManager.player1.Name)
            AsignateValues(currentObject, GameManager.player2);
        
        else if(GameManager.currentPlayer.Name == GameManager.player2.name)
            AsignateValues(currentObject, GameManager.player1);
        
    }  
    public static void AdversaryTokenInit(params object[] param)//Manda una ficha aleatoria del campo del adversario para el inicio
    {
        if(GameManager.currentPlayer.Name == GameManager.player1.Name)
        {
            System.Random random= new System.Random();
            int aleatory = random.Next(0, GameManager.player2.ObjectsInMaze.Count);
            GameObject objectAdversary = GameManager.player2.ObjectsInMaze[aleatory];

            if(objectAdversary.activeInHierarchy)
            {
                objectAdversary.transform.SetParent(MazeGenerator.gameObjects[1,1].transform);
                objectAdversary.transform.position = MazeGenerator.gameObjects[1,1].transform.position;
                objectAdversary.GetComponent<TokenDisplay>().Token.PrePosX = objectAdversary.GetComponent<TokenDisplay>().Token.PosX;
                objectAdversary.GetComponent<TokenDisplay>().Token.PrePosY = objectAdversary.GetComponent<TokenDisplay>().Token.PosY;
                objectAdversary.GetComponent<TokenDisplay>().Token.PosX = 1;
                objectAdversary.GetComponent<TokenDisplay>().Token.PosY = 1;
            }       
        }
        else if(GameManager.currentPlayer.Name == GameManager.player2.Name)
        {
            System.Random random= new System.Random();
            int aleatory = random.Next(0, GameManager.player1.ObjectsInMaze.Count);
            GameObject objectAdversary = GameManager.player1.ObjectsInMaze[aleatory];

            if(objectAdversary.activeInHierarchy)
            {
                objectAdversary.transform.SetParent(MazeGenerator.gameObjects[1,1].transform);
                objectAdversary.transform.position = MazeGenerator.gameObjects[1,1].transform.position;
                objectAdversary.GetComponent<TokenDisplay>().Token.PosX = 1;
                objectAdversary.GetComponent<TokenDisplay>().Token.PosY = 1;
            }  
        }
    }
    public static void MoveBackTokensAdv(params object[] param)//Hace que la fichas vuelvan a la posicion anterior
    {
        if(GameManager.currentPlayer.Name == GameManager.player1.Name)
        {
            foreach(var item in GameManager.player2.ObjectsInMaze)
            {
                TokenDisplay itemD = item.GetComponent<TokenDisplay>();
                item.transform.SetParent(MazeGenerator.gameObjects[itemD.Token.PrePosX,itemD.Token.PrePosY].transform);
                item.transform.position = MazeGenerator.gameObjects[itemD.Token.PrePosX,itemD.Token.PrePosY].transform.position;
                itemD.Token.PosX = itemD.Token.PrePosX;
                itemD.Token.PosY = itemD.Token.PrePosY;
            }
        }
        else if(GameManager.currentPlayer.Name == GameManager.player2.Name)
        {
            foreach(var item in GameManager.player1.ObjectsInMaze)
            {
                TokenDisplay itemD = item.GetComponent<TokenDisplay>();
                item.transform.SetParent(MazeGenerator.gameObjects[itemD.Token.PrePosX,itemD.Token.PrePosY].transform);
                item.transform.position = MazeGenerator.gameObjects[itemD.Token.PrePosX,itemD.Token.PrePosY].transform.position;
                itemD.Token.PosX = itemD.Token.PrePosX;
                itemD.Token.PosY = itemD.Token.PrePosY;
            }
        }
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
            currentObject.GetComponent<TokenDisplay>().Token.PrePosX = currentObject.GetComponent<TokenDisplay>().Token.PosX;
            currentObject.GetComponent<TokenDisplay>().Token.PrePosY = currentObject.GetComponent<TokenDisplay>().Token.PosY;

            objectAdversary.transform.SetParent(ParentObject);
            objectAdversary.transform.position = ParentObject.position;

            objectAdversary.GetComponent<TokenDisplay>().Token.PrePosX = objectAdversary.GetComponent<TokenDisplay>().Token.PosX;
            objectAdversary.GetComponent<TokenDisplay>().Token.PrePosY = objectAdversary.GetComponent<TokenDisplay>().Token.PosY;
            objectAdversary.GetComponent<TokenDisplay>().Token.PosX = x;
            objectAdversary.GetComponent<TokenDisplay>().Token.PosY = y;
        }
    }
    private static bool Check(GameObject[,] gameObjects,int x,int y)
    {
        if(x<0||y<0) return false;
        if(x>=gameObjects.GetLength(0)||y>=gameObjects.GetLength(1)) return false;

        return true;
    }
    private static void UpdateSprites(GameObject gameObject)
    {
        Image image = gameObject.GetComponent<Image>();
        image.sprite = gameObject.GetComponent<CellDisplay>().cell.SpriteDefault;
    }
}
