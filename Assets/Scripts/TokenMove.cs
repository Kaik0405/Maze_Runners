using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;

public class TokenMove : MonoBehaviour
{
    public GameObject CurrentToken;    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            Displace(-1,0);
        
        if (Input.GetKeyDown(KeyCode.S))
            Displace(1,0);

        if (Input.GetKeyDown(KeyCode.D))
            Displace(0,1);

        if (Input.GetKeyDown(KeyCode.A))
            Displace(0,-1);
    }
    private void Displace(int x,int y) //Metoddo para efectuar el desplazamiento segun la tecla presionada
    {

        int currentPosX = CurrentToken.GetComponent<TokenDisplay>().Token.PosX;
        int currentPosY = CurrentToken.GetComponent<TokenDisplay>().Token.PosY;
        
        int dX = currentPosX + x;
        int dY = currentPosY + y;

        if((MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.Obstacle == false) && CurrentToken.GetComponent<TokenDisplay>().Token.Available && GameManager.IsPress && (CurrentToken.GetComponent<TokenDisplay>().Token.CurrentSpeed>0)&&(!CurrentToken.GetComponent<TokenDisplay>().Token.Lock))
        {
            CurrentToken.transform.SetParent(MazeGenerator.gameObjects[dX,dY].transform); //Cambia el padre de la ficha a la celda a la que se desplazo
            CurrentToken.transform.position = MazeGenerator.gameObjects[dX,dY].transform.position; //Le Asigna los valores de posicion a la que se desplazo
            
            CurrentToken.GetComponent<TokenDisplay>().Token.PosX = dX; //actualiza los valores de ubicacion en la matriz de la ficha
            CurrentToken.GetComponent<TokenDisplay>().Token.PosY = dY;
            
            CurrentToken.GetComponent<TokenDisplay>().Token.PrePosX = currentPosX; //actualiza los valores de la posicion anterior de la ficha
            CurrentToken.GetComponent<TokenDisplay>().Token.PrePosY = currentPosY;

            CurrentToken.GetComponent<TokenDisplay>().Token.CurrentSpeed--;

            GameObject AudioValue = MazeGenerator.gameObjects[dX,dY];
            AudioValue.GetComponent<CellDisplay>().Asignate(); 

            if(MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.Tramp) //Activacion de trampa
            {
                MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.trampEffect();
                MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.Tramp = false; //deshabilita la trampa

                //Le asigna el valor de la imagen de la trampa a la ficha 
                CurrentToken.GetComponent<TokenDisplay>().SpriteTrampImage.sprite = MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.ImageTramp;
                CurrentToken.GetComponent<TokenDisplay>().IncreaseA();
                
                AudioValue.GetComponent<CellDisplay>().audioSource.Play();
                
                Image image = MazeGenerator.gameObjects[dX,dY].GetComponent<Image>();
                image.sprite = MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.SpriteDefault;
            }
            if(MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.cellTeleport)//Activacion de teletransporte
            {
                MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.trampEffect(); 
                AudioValue.GetComponent<CellDisplay>().audioSource.Play();
            }
            if(MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.EnergyCell)//Recoger energia
            {
                Image aux = MazeGenerator.gameObjects[dX,dY].GetComponent<Image>();
                aux.sprite = Resources.Load<Sprite>("None");
                
                //Aumenta la velocidad de la ficha y quita la botella de energia
                CurrentToken.GetComponent<TokenDisplay>().Token.CurrentSpeed = CurrentToken.GetComponent<TokenDisplay>().Token.CurrentSpeed + 3;
                MazeGenerator.gameObjects[dX,dY].GetComponent<CellDisplay>().cell.EnergyCell = false;
                
                AudioValue.GetComponent<CellDisplay>().audioSource.Play(); 
            }
            if(dX == MazeGenerator.cellEnd.x && dY == MazeGenerator.cellEnd.y)//Condicion de llegada a la meta
            {
                GameManager.currentPlayer.AddTokentoEnd();
                CurrentToken.SetActive(false);
                
                for(int i=0;i < GameManager.currentPlayer.ObjectsInMaze.Count;i++)
                {
                    if(GameManager.currentPlayer.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token.Name==CurrentToken.GetComponent<TokenDisplay>().Token.Name)
                        GameManager.currentPlayer.ObjectsInMaze.Remove(GameManager.currentPlayer.ObjectsInMaze[i]);    
                }
            }
        }            
    }
}
