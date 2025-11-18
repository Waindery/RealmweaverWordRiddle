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

        SceneManager.LoadScene("GameScene");
    }

    public void ExitButton()
    {
        Debug.Log("ExitButton Basildi");

        Application.Quit();
    }

}