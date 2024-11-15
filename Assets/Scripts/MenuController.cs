using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject Music;
    public void ChangeSceneWithDelay(string sceneName, float delay)
    {
        if (Music.GetComponent<AudioSource>() != null)
        {
            Music.GetComponent<AudioSource>().Play();
        }
        Invoke("TranslateToGame", delay);
    }
    public void TranslateToGame()
    {
        ChangeSceneWithDelay("TranslateToScene", 1.0f);
        DontDestroyOnLoad(Music);
        SceneManager.LoadSceneAsync("MazeRunnerGame");
    }
    public void ChangeSceneWithDelayOptions(string sceneName,float delay)
    {
        if(Music.GetComponent<AudioSource>() != null)
        {
            Music.GetComponent<AudioSource>().Play();
        }
        Invoke("TranslateToOptions",delay);
    }
    public void TranslateToOptions()
    {
        ChangeSceneWithDelayOptions("TranslateToScene", 1.0f);
        DontDestroyOnLoad(Music);
        SceneManager.LoadSceneAsync("Options");
    }
    public void ChangeSceneWithDelayTutorial(string sceneName, float delay)
    {
        if (Music.GetComponent<AudioSource>() != null)
        {
            Music.GetComponent<AudioSource>().Play();
        }
        Invoke("TranslateToTutorial", delay);
    }
    public void TranslateToTutorial()
    {
        ChangeSceneWithDelayTutorial("TranslateToScene", 1.0f);
        DontDestroyOnLoad(Music);
        SceneManager.LoadSceneAsync("Tutorial");
    }
    public void ExitGame()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.Play();

        Application.Quit();
    }
}
