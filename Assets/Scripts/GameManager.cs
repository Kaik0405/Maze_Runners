using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Xml.Serialization;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    public GameObject TonkenInScene;
    public static GameObject StaticTokenInScene;
    public GameObject PanelInScene;
    public GameObject ChangeBotton;
    public GameObject PanelSelect;
    public GameObject EndTurnButton;
    public static Player player1 = new Player("Player1",true,TeamManager.TeamsPlayer1);
    public static Player player2 = new Player("Player2",false,TeamManager.TeamsPlayer2);
    List<GameObject> BottonList = new List<GameObject>();
    public static Player currentPlayer;
    public static bool IsPress = false;

    void Start()    
    {
        StaticTokenInScene = TonkenInScene;
        player1.InstantiateTokens();
        player2.InstantiateTokens();
        currentPlayer = player1;
    }
    void Update()
    {
        if (player1.TokensInFinishLine == player1.ObjectsInMaze.Count)
        {
            Debug.Log("Player 1 Win");
            ChangeBotton.SetActive(false);
            EndTurnButton.SetActive(false);
        }
    }
    public void EndTurn()
    {
        IsPress = false;
        ChangeBotton.SetActive(true);
        player1.SwitchTurn();
        player2.SwitchTurn();
        currentPlayer.ResetMoveDistance();
        if(player1.Turn) currentPlayer = player1;
        else if(player2.Turn) currentPlayer = player2;
    }
    public void ChangeToken()
    {
        PanelInScene.SetActive(true);
        EndTurnButton.SetActive(false);
        ChangeBotton.SetActive(false);

        Player playerRef = new Player("PLayer",false,new List<Token>());

        if(player1.Turn) playerRef = player1;
        else if(player2.Turn) playerRef = player2;    
        
        for(int i = 0;i<playerRef.ObjectsInMaze.Count;i++)
        {
            GameObject buttonObject = new GameObject("Botton");
            Button buttonComponent = buttonObject.AddComponent<Button>();
            RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
            TokenDisplay tokenDisplay = buttonObject.AddComponent<TokenDisplay>();
            Image image = buttonComponent.AddComponent<Image>();

            if(playerRef.Name == player1.Name)
                image.color = Color.red;
            else
                image.color = Color.blue; 

            buttonObject.transform.SetParent(PanelSelect.transform);
            buttonObject.transform.position = PanelSelect.transform.position;
            buttonObject.GetComponent<TokenDisplay>().Token = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token; 

            buttonComponent.onClick.AddListener(()=>
            {
                if(!playerRef.CheckObjectsActive())
                {
                    foreach(GameObject item in playerRef.ObjectsInMaze)
                        if(item.GetComponent<TokenDisplay>().Token.Name == buttonObject.GetComponent<TokenDisplay>().Token.Name)
                        {
                            item.SetActive(true);
                            item.GetComponent<TokenDisplay>().Token.Available = true;
                        }
                }    
                else if(playerRef.CheckObjectsActive())
                {
                    playerRef.DesactiveObject();
                    
                    foreach(GameObject item in playerRef.ObjectsInMaze)
                        item.GetComponent<TokenDisplay>().Token.Available = false;

                    foreach(GameObject item in playerRef.ObjectsInMaze)
                        if(item.GetComponent<TokenDisplay>().Token.Name == buttonObject.GetComponent<TokenDisplay>().Token.Name)
                        {
                            item.SetActive(true);
                            item.GetComponent<TokenDisplay>().Token.Available = true;
                        }
                }    
            });
            BottonList.Add(buttonObject);
        }
    }
    public void Confirm(){
        PanelInScene.SetActive(false);

        foreach(var botton in BottonList)
            Destroy(botton);

        BottonList.Clear();
        IsPress = true;
        EndTurnButton.SetActive(true);    
    }
}
