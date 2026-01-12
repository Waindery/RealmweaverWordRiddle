using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void PlayButton ()
    {
        Debug.Log("PlayButton Basildi");


        SceneManager.LoadScene("CinematicScene");
    }

    public void OptionsButton()
    {
        Debug.Log("OptionsButton Basildi");
    }

    public void Continue1Button()
    {
        Debug.Log("Continue1Button Basildi");

        SceneManager.LoadScene("Cinematic2Scene");
    }

    public void Continue2Button()
    {
        Debug.Log("Continue2Button Basildi");

        // Kaydedilmiş GameScene'i al, yoksa varsayılan olarak GameScene
        string targetScene = GameProgressManager.GetLastGameScene();
        Debug.Log($"Loading saved GameScene: {targetScene}");
        
        SceneManager.LoadScene(targetScene);
    }

    public void Continue3Button()
    {
        Debug.Log("Continue3Button Basildi");

        SceneManager.LoadScene("Game4Scene");
    }

    public void Continue4Button()
    {
        Debug.Log("Continue4Button Basildi");

        SceneManager.LoadScene("MainScene");
    }

    public void ExitButton()
    {
        Debug.Log("ExitButton Basildi");

        Application.Quit();
    }

}