using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string Name;
    public bool Turn = false;
    public int TokensInFinishLine = 0;
    public List<Token> TokensList = new List<Token>(); // Guarda las fichas disponibles para el jugador
    public List<GameObject> ObjectsInMaze = new List<GameObject>(); // Almacena las referencias de las fichas en escena
    public Player(string name,bool turn,List<Token> tokens)
    {
        Name = name;
        Turn = turn;    
        TokensList = tokens;
    }
    public void InstantiateTokens() //Instancia las fichas en escena
    {
        GameObject TokenRef = GameManager.StaticTokenInScene;

        foreach (var item in TokensList)
        {
            GameObject tokenInScene = Instantiate(TokenRef,TokenRef.transform.position,TokenRef.transform.rotation);
            tokenInScene.GetComponent<TokenDisplay>().Token = item;
            if(Name == "Player1")
            {
                Image image = tokenInScene.GetComponent<Image>();
                image.color = Color.white;
            }
            else
            {
                Image image = tokenInScene.GetComponent<Image>();
                image.color = Color.white;
            }
            tokenInScene.GetComponent<TokenDisplay>().Asignate();
            ObjectsInMaze.Add(tokenInScene); 
        }
        foreach (var item in ObjectsInMaze)
            item.SetActive(false);
    }
    public bool CheckObjectsActive() //Activa los objetos que son seleccionados
    {
        foreach (var item in ObjectsInMaze)
        {
            if(item.activeInHierarchy)
                return true;
        }
        return false;
    }
    public void DesactiveObject() //Desactiva los objetos de la posicion inicial
    {
        foreach (var item in ObjectsInMaze)
        {
            if(item.activeInHierarchy)
            {
                Token gameObjectAux = item.GetComponent<TokenDisplay>().Token;
                if(gameObjectAux.PosX == 1 && gameObjectAux.PosY == 1)
                item.SetActive(false);
            }
        }
    }
    public void SwitchTurn() //Metodo para el cambio de turno del jugador
    {
        Turn = !Turn;
        DisableScrolling();
    }
    private void DisableScrolling() //Desabilita el movimiento de la ficha actual del jugador
    {
        foreach(var item in ObjectsInMaze)
            item.GetComponent<TokenDisplay>().Token.Available = false;
    }
    public void ResetMoveDistance() // Resetea el desplazamiento disponible
    {
        foreach(var item in ObjectsInMaze)
            item.GetComponent<TokenDisplay>().Token.CurrentSpeed = item.GetComponent<TokenDisplay>().Token.GetSpeed();   
    }
    public void AddTokentoEnd() //Metodo que actualiza la cantidad de fichas que han llegado a la meta
    {
        TokensInFinishLine++;
    }
    public void DecreaseCoolDown()
    {
        foreach(var item in ObjectsInMaze)
        {
            if(item.GetComponent<TokenDisplay>().Token.CurrentCooldown>0)
                item.GetComponent<TokenDisplay>().Token.CurrentCooldown--;
        }
    }
    public bool IsAvaliable()
    {
        foreach (var item in ObjectsInMaze)
        {
            if(item.GetComponent<TokenDisplay>().Token.Available)
                return true;
        }
        return false;
    }
    public GameObject CurrentTokenObject()
    {
        foreach (var item in ObjectsInMaze)
        {
            if(item.GetComponent<TokenDisplay>().Token.Available)
                return item;
        }
        return null;
    }
} 
