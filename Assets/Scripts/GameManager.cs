using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject Token;
    public GameObject TonkenInScene;
    void Start()
    {
          
    }
    public void InstantiateObjectIn() 
    {
        Token = TonkenInScene; 
        Token.GetComponent<TokenDisplay>().Token.PosX = 1;
        Token.GetComponent<TokenDisplay>().Token.PosY = 1;
        Instantiate(Token,MazeGenerator.gameObjects[1,1].transform.position,MazeGenerator.gameObjects[1,1].transform.rotation);       
    }
}
