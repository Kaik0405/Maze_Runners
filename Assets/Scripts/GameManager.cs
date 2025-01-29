using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine;
using System.Xml.Serialization;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject TonkenInScene; // Referencia a el prefab de la ficha
    public static GameObject StaticTokenInScene; // Referencia a la ficha de la escena
    public GameObject PanelInScene; // Referencia a el panel de seleccion de fichas
    public GameObject ChangeBotton; // Referencia a el boton de cambio de fichas
    public GameObject PanelSelect; // Referencia a el panel de seleccion organizacion
    public GameObject EndTurnButton; // Referencia a el boton de terminar turno
    public GameObject ActivateSkillButton; // Referencia a el boton de activar habilidad
    public GameObject SoundObject; //Referencia al Objeto de sonido
    public GameObject PanelWin; //Refernecia al panel de victoria
    public TMP_Text TextPlayerWin; //Texto del jugador ganador
    public GameObject PanelSwitchTurn; //Panel de cambio de turno
    public static Player player1 = new Player(TeamManager.NamePlayer1,true,TeamManager.TeamsPlayer1); // Instanciacion del jugador1 
    public static Player player2 = new Player(TeamManager.NamePlayer2,false,TeamManager.TeamsPlayer2); // Instanciacion del jugador2 
    List<GameObject> BottonList = new List<GameObject>(); // Lista de botones que se generan en la escena
    public static Player currentPlayer; //Referencia al jugador actual por turno
    public static bool IsPress = false; //
    private bool SkillAvaliable = false; //Valor para detectar si el boton de habilidad se puede activar
    private bool ExistWin = false; //Detecta si hay ganador
    void Start()    
    {
        currentPlayer = new Player("",false,TeamManager.TeamsPlayer1); //Asignacion del jugador actual
        StaticTokenInScene = TonkenInScene; 
        player1.InstantiateTokens();
        player2.InstantiateTokens();
        currentPlayer = player1;
    }
    void Update()
    {
        if(ExistWin)
        {

        }
        else
        {
            if (player1.TokensInFinishLine == player1.NumToken) //Condicion de victoria del jugador1
            {
                ChangeBotton.SetActive(false);
                EndTurnButton.SetActive(false);
                ExistWin = true;
                PanelWin.SetActive(true);
                TextPlayerWin.text = player1.Name+" Gana";
            }
            else if(player2.TokensInFinishLine == player2.NumToken) //Condicion de victoria del jugador2
            {
                ChangeBotton.SetActive(false);
                EndTurnButton.SetActive(false);
                ExistWin = true;
                PanelWin.SetActive(true);
                TextPlayerWin.text = player2.Name+" Gana";
            }
        }
    }
    public void EndTurn() // Metodo del boton para el cambio de turno
    {
        StartCoroutine(ActivateP(PanelSwitchTurn)); // Muestra el panel de cambio de turno
        SoundObject.GetComponent<AudioSource>().Play(); 
        
        IsPress = false;
        ChangeBotton.SetActive(true);
        
        player1.SwitchTurn();
        player2.SwitchTurn();

        currentPlayer.ResetMoveDistance(); //Restablece la velocidad de la ficha

        if(player1.Turn) 
        {
            currentPlayer = player1;
            player1.DecreaseCoolDown(); //Disminuye el cooldown de las fichas 
        }
        else if(player2.Turn) 
        {
            currentPlayer = player2;
            player2.DecreaseCoolDown(); //Disminuye el cooldown de las fichas 
        }
        ActivateSkillButton.SetActive(false);
    }
    public void ChangeToken() // Metodo para el boton de cambio y seleccion de token
    {
        SoundObject.GetComponent<AudioSource>().Play();
        //Activacion del panel de cambio de ficha y desactivacion de los botones innecesarios y que hacen que explote todo XD
        PanelInScene.SetActive(true);
        EndTurnButton.SetActive(false);
        ChangeBotton.SetActive(false);

        Player playerRef = new Player("PLayer",false,new List<Token>());

        if(player1.Turn) playerRef = player1;
        else if(player2.Turn) playerRef = player2;    
        
        for(int i = 0;i<playerRef.ObjectsInMaze.Count;i++) //Recorre la lista del jugador que hace referencia a las fichas en escena
        {
            //Creacion de un boton y componentes
            GameObject buttonObject = new GameObject("Botton");
            Button buttonComponent = buttonObject.AddComponent<Button>();
            RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
            TokenDisplay tokenDisplay = buttonObject.AddComponent<TokenDisplay>();
            Image image = buttonComponent.AddComponent<Image>(); 

            //Asignacion del padre y de la posicion del padres
            buttonObject.transform.SetParent(PanelSelect.transform);
            buttonObject.transform.position = PanelSelect.transform.position;
            buttonObject.GetComponent<TokenDisplay>().Token = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token; 

            //Asignacion del sprite del boton
            image.sprite = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token.SpriteTokenNormal;

            //Uso del Selectable.Transition para el cambio de imagen al pasar el cursor
            Button button = buttonObject.GetComponent<Button>();
            button.transition = Selectable.Transition.SpriteSwap;
            button.targetGraphic = buttonObject.GetComponent<Image>();

            //Logica del cambio de sprite segun la poscicion del cursor
            SpriteState spriteState = new SpriteState(); 
            spriteState.highlightedSprite = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token.SpriteTokenSelect; 
            spriteState.selectedSprite = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token.SpriteTokenSelect;
            buttonComponent.spriteState = spriteState;   

            buttonComponent.onClick.AddListener(()=> //Asigancion del la funcionalidad al presionar el boton
            {
                SoundObject.GetComponent<AudioSource>().Play(); 
                
                if(!playerRef.CheckObjectsActive()) // Verifica si no hay fichas activadas
                {
                    //Busca el boton que el boton que se presiono coincida con alguna ficha del jugador
                    foreach(GameObject item in playerRef.ObjectsInMaze) 
                        if(item.GetComponent<TokenDisplay>().Token.Name == buttonObject.GetComponent<TokenDisplay>().Token.Name)
                        {
                            item.SetActive(true); //Activa la ficha para que se muestre en escena
                            item.GetComponent<TokenDisplay>().Token.Available = true; // Marca la ficha como desplazable
                            SkillAvaliable = true; //Permite que se muestre el boton de activar hablidad
                        }
                }    
                else if(playerRef.CheckObjectsActive()) //Verifica si hay fichas activadas
                {
                    playerRef.DesactiveObject(); //Desactiva los objetos del jugador en la posicion (1,1)
                    
                    //Marca todos los objetos como no desplazables del jugador
                    foreach(GameObject item in playerRef.ObjectsInMaze) 
                        item.GetComponent<TokenDisplay>().Token.Available = false;

                    //Busca el boton que el boton que se presiono coincida con alguna ficha del jugador
                    foreach(GameObject item in playerRef.ObjectsInMaze)
                        if(item.GetComponent<TokenDisplay>().Token.Name == buttonObject.GetComponent<TokenDisplay>().Token.Name)
                        {
                            item.SetActive(true); //Activa la ficha
                            item.GetComponent<TokenDisplay>().Token.Available = true; //La marca como disponible

                            if(item.GetComponent<TokenDisplay>().Token.CurrentCooldown == 0) //Verifica si la ficha no tiene Cooldown
                                SkillAvaliable = true; //Le activa el boton de habilidad
                        }
                }    
            });
            BottonList.Add(buttonObject); //Agrega el boton a la lista de referencias de botones 
        }
    }
    public void Confirm() // Metodo del boton de confirmar en el panel de seleccion
    {
        if(currentPlayer.IsAvaliable()) //Comprueba si el jugador tiene alguna ficha desplazable
        {
            SoundObject.GetComponent<AudioSource>().Play();
    
            PanelInScene.SetActive(false); //Desactiva el panel
    
            foreach(var botton in BottonList) //Destruye los botones creados
                Destroy(botton);
    
            BottonList.Clear(); //Limpia la lista de botones
            IsPress = true;
            EndTurnButton.SetActive(true);//Activa el boton de cambio de turno
    
            if(SkillAvaliable) ActivateSkillButton.SetActive(true); //Si puede usar la habilidad activa el boton de habilidad
        }   
    }
    public void ActivateSkillToken() //Metodo para activar la habilidad de la ficha
    {
        SoundObject.GetComponent<AudioSource>().Play();

        GameObject currentToken = currentPlayer.CurrentTokenObject();
        StartCoroutine(StopTime(currentToken)); //Activa la habilidad luego de finalizar el sonido de la ficha
        
        //Desactiva la disponibildad de la hablidad de la ficha y asigna el cooldown ademas de cambiar el sprite de la ficha
        ActivateSkillButton.SetActive(false);
        currentToken.GetComponent<TokenDisplay>().Token.CurrentCooldown = currentToken.GetComponent<TokenDisplay>().Token.GetCooldown();
        currentToken.GetComponent<TokenDisplay>().Token.SpriteTokenFull =  currentToken.GetComponent<TokenDisplay>().Token.SpriteTokenSkillActive;
        SkillAvaliable = false; 
    }
    public void BackToMenu() //Logica de volver al menu
    {
        ChangeSceneWithDelay("TranslateToScene", 1.0f);
        DontDestroyOnLoad(SoundObject);
        Destroy(player1);
        Destroy(player2);
        DestroyObjectsScene();
        SceneManager.LoadSceneAsync("Main Menu");
    }
    public void ChangeSceneWithDelay(string sceneName, float delay)
    {
        if (SoundObject.GetComponent<AudioSource>() != null)
        {
            SoundObject.GetComponent<AudioSource>().Play();
        }
        Invoke("BackToMenu", delay);
    }
    public void DestroyObjectsScene() //Destruye las fichas de ambos jugadores
    {
        foreach (var item in player1.ObjectsInMaze)
            Destroy(item);
        foreach (var item in player2.ObjectsInMaze)
            Destroy(item);    
    }
    IEnumerator StopTime(GameObject gameObject) //Corrutina para la activacion de la habilidad 
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<TokenDisplay>().audioSource.Play();

        yield return new WaitForSeconds(TimeForPlayer(gameObject.GetComponent<TokenDisplay>().Token.Name));
        gameObject.GetComponent<TokenDisplay>().Token.Skill();
    }
    IEnumerator ActivateP(GameObject gameObject) //Corrutina para la activacion del panel
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
    private float TimeForPlayer(string name)
    {
        string[] names = {"Natsu","Lucy","Gray","Erza","Wendy","Laxus","Gajeel","Mirajane","Juvia","Mystogan"};
        float[] times = {7.0f,7.0f,7.0f,8.0f,6.9f,4.0f,3.0f,9.0f,1.3f,9.0f};
        
        for (int i = 0;i<names.Length;i++)
            if(names[i] == name) 
                return times[i];
        
        return 0.0f;
    }
}