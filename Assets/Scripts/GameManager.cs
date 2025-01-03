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
    public TMP_Text TextPlayerWin;
    public GameObject PanelSwitchTurn;
    public static Player player1 = new Player(TeamManager.NamePlayer1,true,TeamManager.TeamsPlayer1); // Instanciacion del jugador1 
    public static Player player2 = new Player(TeamManager.NamePlayer2,false,TeamManager.TeamsPlayer2); // Instanciacion del jugador2 
    List<GameObject> BottonList = new List<GameObject>(); // Lista de botones que se generan en la escena
    public static Player currentPlayer;
    public static bool IsPress = false;
    private bool SkillAvaliable = false;
    private bool ExistWin = false;
    void Start()    
    {
        currentPlayer = new Player("",false,TeamManager.TeamsPlayer1);
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
            if (player1.TokensInFinishLine == player1.NumToken)
            {
                Debug.Log("Player 1 Win");
                ChangeBotton.SetActive(false);
                EndTurnButton.SetActive(false);
                ExistWin = true;
                PanelWin.SetActive(true);
                TextPlayerWin.text = player1.Name+" Gana";
            }
            else if(player2.TokensInFinishLine == player2.NumToken)
            {
                Debug.Log("Player 2 Win");
                ChangeBotton.SetActive(false);
                EndTurnButton.SetActive(false);
                ExistWin = true;
                PanelWin.SetActive(true);
                TextPlayerWin.text = player2.Name+" Gana";
            }
        }
    }
    public void EndTurn() // Metodo boton para el cambio de turno
    {
        StartCoroutine(ActivateP(PanelSwitchTurn));
        SoundObject.GetComponent<AudioSource>().Play();
        
        IsPress = false;
        ChangeBotton.SetActive(true);
        
        player1.SwitchTurn();
        player2.SwitchTurn();
        currentPlayer.ResetMoveDistance();

        if(player1.Turn) 
        {
            currentPlayer = player1;
            player1.DecreaseCoolDown();
        }
        else if(player2.Turn) 
        {
            currentPlayer = player2;
            player2.DecreaseCoolDown();
        }
        ActivateSkillButton.SetActive(false);
    }
    public void ChangeToken() // Metodo para el boton de cambio y seleccion de token
    {
        SoundObject.GetComponent<AudioSource>().Play();

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

            buttonObject.transform.SetParent(PanelSelect.transform);
            buttonObject.transform.position = PanelSelect.transform.position;
            buttonObject.GetComponent<TokenDisplay>().Token = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token; 

            image.sprite = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token.SpriteTokenNormal;

            Button button = buttonObject.GetComponent<Button>();
            button.transition = Selectable.Transition.SpriteSwap;
            button.targetGraphic = buttonObject.GetComponent<Image>();

            SpriteState spriteState = new SpriteState(); 
            spriteState.highlightedSprite = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token.SpriteTokenSelect; 
            spriteState.selectedSprite = playerRef.ObjectsInMaze[i].GetComponent<TokenDisplay>().Token.SpriteTokenSelect;
            buttonComponent.spriteState = spriteState;   

            buttonComponent.onClick.AddListener(()=>
            {
                SoundObject.GetComponent<AudioSource>().Play();
                
                if(!playerRef.CheckObjectsActive())
                {
                    foreach(GameObject item in playerRef.ObjectsInMaze)
                        if(item.GetComponent<TokenDisplay>().Token.Name == buttonObject.GetComponent<TokenDisplay>().Token.Name)
                        {
                            item.SetActive(true);
                            item.GetComponent<TokenDisplay>().Token.Available = true;
                            SkillAvaliable = true;
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

                            if(item.GetComponent<TokenDisplay>().Token.CurrentCooldown == 0)
                                SkillAvaliable = true;
                        }
                }    
            });
            BottonList.Add(buttonObject);
        }
    }
    public void Confirm() // Metodo del boton de confirmar en el panel de seleccion
    {
        if(currentPlayer.IsAvaliable())
        {
            SoundObject.GetComponent<AudioSource>().Play();
    
            PanelInScene.SetActive(false);
    
            foreach(var botton in BottonList)
                Destroy(botton);
    
            BottonList.Clear();
            IsPress = true;
            EndTurnButton.SetActive(true);
    
            if(SkillAvaliable) ActivateSkillButton.SetActive(true); 
        }   
    }
    public void ActivateSkillToken() //Metodo para activar la habilidad de la ficha
    {
        SoundObject.GetComponent<AudioSource>().Play();

        GameObject currentToken = currentPlayer.CurrentTokenObject();
        StartCoroutine(StopTime(currentToken));
        
        ActivateSkillButton.SetActive(false);
        currentToken.GetComponent<TokenDisplay>().Token.CurrentCooldown = currentToken.GetComponent<TokenDisplay>().Token.GetCooldown();
        currentToken.GetComponent<TokenDisplay>().Token.SpriteTokenFull =  currentToken.GetComponent<TokenDisplay>().Token.SpriteTokenSkillActive;
        SkillAvaliable = false; 
    }
    public void BackToMenu()
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
    public void DestroyObjectsScene()
    {
        foreach (var item in player1.ObjectsInMaze)
            Destroy(item);
        foreach (var item in player2.ObjectsInMaze)
            Destroy(item);    
    }
    IEnumerator StopTime(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<TokenDisplay>().audioSource.Play();
        yield return new WaitForSeconds(7.0f);
        gameObject.GetComponent<TokenDisplay>().Token.Skill();
    }
    IEnumerator ActivateP(GameObject gameObject)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}