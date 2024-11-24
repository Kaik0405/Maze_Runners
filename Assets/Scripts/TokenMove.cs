using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class TokenMove : MonoBehaviour
{
    public GameObject CurrentToken;    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("MOVIENDO ARRIBA");
            Displace(-1,0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("MOVIENDO ABAJO");
            Displace(1,0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("MOVIENDO DERECHA");
            Displace(0,1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("MOVIENDO IZQUIERDA");
            Displace(0,-1);
        }    
    }
    private void Displace(int x,int y) //Metoddo para efectuar el desplazamiento segun la tecla presionada
    {

        int currentPosX = CurrentToken.GetComponent<TokenDisplay>().Token.PosX;
        int currentPosY = CurrentToken.GetComponent<TokenDisplay>().Token.PosY;
        
        int dX = currentPosX + x;
        int dY = currentPosY + y;

        if(MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.Obstacle == false)
        {
            CurrentToken.transform.SetParent(MazeGenerator.gameObjects[dX,dY].transform); //Cambia el padre de la ficha a la celda a la que se desplazo
            CurrentToken.transform.position = MazeGenerator.gameObjects[dX,dY].transform.position; //Le Asigna los valores de posicion a la que se desplazo
            CurrentToken.GetComponent<TokenDisplay>().Token.PosX = dX; //actualiza los valores de ubicacion en la matriz
            CurrentToken.GetComponent<TokenDisplay>().Token.PosY = dY;
        }
            
    }
}
