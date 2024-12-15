using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour
{
    public static List<Token> TeamsPlayer1 = new List<Token>();
    public static List<Token> TeamsPlayer2 = new List<Token>();
    public GameObject Music;
    bool[] presset = {false, false, false, false, false, false, false, false, false, false};
    void Start()
    {


    }
    void Update()
    {
        
    }
    public void ChangeSceneWithDelayReady(string sceneName, float delay)
    {
        if (Music.GetComponent<AudioSource>() != null)
        {
            Music.GetComponent<AudioSource>().Play();
        }
        Invoke("Ready", delay);
    }
    public void Ready()
    {
        if((TeamsPlayer1.Count == TeamsPlayer2.Count)&&(TeamsPlayer1.Count > 0))
        {
            ChangeSceneWithDelayReady("TranslateToScene", 1.0f);
            DontDestroyOnLoad(Music);   
            SceneManager.LoadSceneAsync("MazeRunnerGame");
        }
    }
    public void AddNatsu()
    {
        if(!presset[0])
        {
            TeamsPlayer1.Add(DataBaseTokens.Character[0]);
            presset[0] = true;
        }
    }
    public void AddLucy()
    {
        if(!presset[1])
        {
            TeamsPlayer1.Add(DataBaseTokens.Character[1]);
            presset[1] = true;
        }
    }
    public void AddGray()
    {
        if(!presset[2])
        {
            TeamsPlayer1.Add(DataBaseTokens.Character[2]);
            presset[2] = true;
        }
    }
    public void AddErza()
    {
        if(!presset[3])
        {
            TeamsPlayer1.Add(DataBaseTokens.Character[3]);
            presset[3] = true;
        }
    }
    public void AddWendy()
    {
        if(!presset[4])
        {
            TeamsPlayer1.Add(DataBaseTokens.Character[4]);
            presset[4] = true;
        }
    }
    public void AddLaxus()
    {
        if(!presset[5])
        {
            TeamsPlayer2.Add(DataBaseTokens.Character[5]);
            presset[5] = true;
        }
    }
    public void AddMirajane()
    {
        if(!presset[6])
        {
            TeamsPlayer2.Add(DataBaseTokens.Character[6]);
            presset[6] = true;
        }
    }
    public void AddGajeel()
    {
        if(!presset[7])
        {
            TeamsPlayer2.Add(DataBaseTokens.Character[7]);
            presset[7] = true;
        }
    }
    public void AddJuvia()
    {
        if(!presset[8])
        {
            TeamsPlayer2.Add(DataBaseTokens.Character[8]);
            presset[8] = true;
        }
    }
    public void AddMystogan()
    {
        if(!presset[9])
        {
            TeamsPlayer2.Add(DataBaseTokens.Character[9]);
            presset[9] = true;
        }
    }
     
}
