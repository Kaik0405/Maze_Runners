using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToken : MonoBehaviour
{
    GameObject Token;
    public GameObject TokenAdd;
    void Start() //Define la posicion donde va a ser instanciada la ficha
    {
        Token = MazeGenerator.gameObjects[1,1];

        TokenAdd.transform.SetParent(Token.transform);
        TokenAdd.transform.transform.localScale = Vector3.one;
        TokenAdd.transform.position = new Vector3(MazeGenerator.gameObjects[1,1].transform.position.x, MazeGenerator.gameObjects[1,1].transform.position.y, MazeGenerator.gameObjects[1,1].transform.position.z);
        TokenAdd.transform.eulerAngles = new Vector3(0,0,0);
    }

}
