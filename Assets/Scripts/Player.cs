using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string Name;
    public bool Turn = false;
    int TokensInFinishLine;
    public List<Token> TokensList = new List<Token>();
    public List<GameObject> ObjectsInMaze = new List<GameObject>();
    public Player(string name,bool turn,List<Token> tokens)
    {
        Name = name;
        Turn = turn;    
        TokensList = tokens;
    }
    public void InstantiateTokens()
    {
        GameObject TokenRef = GameManager.StaticTokenInScene;

        foreach (var item in TokensList)
        {
            GameObject tokenInScene = Instantiate(TokenRef,TokenRef.transform.position,TokenRef.transform.rotation);
            tokenInScene.GetComponent<TokenDisplay>().Token = item;
            if(Name == "Player1")
            {
                Image image = tokenInScene.GetComponent<Image>();
                image.color = Color.red;
            }
            else{
                Image image = tokenInScene.GetComponent<Image>();
                image.color = Color.blue;
            }
            ObjectsInMaze.Add(tokenInScene); 
        }
        foreach (var item in ObjectsInMaze)
            item.SetActive(false);
    }
    public bool CheckObjectsActive()
    {
        foreach (var item in ObjectsInMaze)
        {
            if(item.activeInHierarchy)
                return true;
        }
        return false;
    }
    public void DesactiveObject()
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
    public void SwitchTurn(){
        Turn = !Turn;
        DisableScrolling();
    }
    private void DisableScrolling()
    {
        foreach(var item in ObjectsInMaze)
            item.GetComponent<TokenDisplay>().Token.Available = false;
    }
} 
