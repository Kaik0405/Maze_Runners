using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControler : MonoBehaviour
{
    public GameObject Music;
    public void ChangeSceneWithDelay(string sceneName, float delay)
    {
        if (Music.GetComponent<AudioSource>() != null)
        {
            Music.GetComponent<AudioSource>().Play();
        }
        Invoke("TraslateToGame", delay);
    }
    public void TraslateToGame()
    {
        ChangeSceneWithDelay("TraslateToScene", 1.0f);
        DontDestroyOnLoad(Music);
        SceneManager.LoadSceneAsync("MazeRunnerGame");
    }
    public void ChangeSceneWithDelayOptions(string sceneName,float delay)
    {
        if(Music.GetComponent<AudioSource>() != null)
        {
            Music.GetComponent<AudioSource>().Play();
        }
        Invoke("TraslateToOptions",delay);
    }
    public void TraslateToOptions()
    {
        ChangeSceneWithDelayOptions("TraslateToScene", 1.0f);
        DontDestroyOnLoad(Music);
        SceneManager.LoadSceneAsync("Options");
    }
    public void ChangeSceneWithDelayTutorial(string sceneName, float delay)
    {
        if (Music.GetComponent<AudioSource>() != null)
        {
            Music.GetComponent<AudioSource>().Play();
        }
        Invoke("TraslateToTutorial", delay);
    }
    public void TraslateToTutorial()
    {
        ChangeSceneWithDelayTutorial("TraslateToScene", 1.0f);
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
