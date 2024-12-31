using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TeamManager : MonoBehaviour
{
    public static List<Token> TeamsPlayer1 = new List<Token>();
    public static List<Token> TeamsPlayer2 = new List<Token>();
    public static string NamePlayer1;
    public static string NamePlayer2;
    public TMP_Text player1Name;
    public TMP_Text player2Name;
    public GameObject Music;
    public GameObject SoundObject;
    bool[] presset = {false, false, false, false, false, false, false, false, false, false};
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
        NamePlayer1 = player1Name.text;
        NamePlayer2 = player2Name.text;

        if((TeamsPlayer1.Count == TeamsPlayer2.Count) && (TeamsPlayer1.Count > 0) && (player1Name.text != "" || player1Name.text != ""))
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
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer1.Add(DataBaseTokens.Character[0]);
            presset[0] = true;
            AsiganteImage(DataBaseTokens.Character[0]);
        }
    }
    public void AddLucy()
    {
        if(!presset[1])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer1.Add(DataBaseTokens.Character[1]);
            presset[1] = true;
            AsiganteImage(DataBaseTokens.Character[1]);
        }
    }
    public void AddGray()
    {
        if(!presset[2])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer1.Add(DataBaseTokens.Character[2]);
            presset[2] = true;
            AsiganteImage(DataBaseTokens.Character[2]);
        }
    }
    public void AddErza()
    {
        if(!presset[3])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer1.Add(DataBaseTokens.Character[3]);
            presset[3] = true;
            AsiganteImage(DataBaseTokens.Character[3]);
        }
    }
    public void AddWendy()
    {
        if(!presset[4])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer1.Add(DataBaseTokens.Character[4]);
            presset[4] = true;
            AsiganteImage(DataBaseTokens.Character[4]);
        }
    }
    public void AddLaxus()
    {
        if(!presset[5])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer2.Add(DataBaseTokens.Character[5]);
            presset[5] = true;
            AsiganteImage(DataBaseTokens.Character[5]);
        }
    }
    public void AddMirajane()
    {
        if(!presset[6])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer2.Add(DataBaseTokens.Character[6]);
            presset[6] = true;
            AsiganteImage(DataBaseTokens.Character[6]);
        }
    }
    public void AddGajeel()
    {
        if(!presset[7])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer2.Add(DataBaseTokens.Character[7]);
            presset[7] = true;
            AsiganteImage(DataBaseTokens.Character[7]);
        }
    }
    public void AddJuvia()
    {
        if(!presset[8])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer2.Add(DataBaseTokens.Character[8]);
            presset[8] = true;
            AsiganteImage(DataBaseTokens.Character[8]);
        }
    }
    public void AddMystogan()
    {
        if(!presset[9])
        {
            SoundObject.GetComponent<AudioSource>().Play();
            TeamsPlayer2.Add(DataBaseTokens.Character[9]);
            presset[9] = true;
            AsiganteImage(DataBaseTokens.Character[9]);
        }
    }
    private void AsiganteImage(Token token)
    {
        GameObject button = GameObject.Find(token.Name);
        Image image = button.GetComponent<Image>();
        image.sprite = token.SpriteTokenSelect;
    }
}
