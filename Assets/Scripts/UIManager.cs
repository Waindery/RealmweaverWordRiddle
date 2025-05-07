using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void PlayButton ()
    {
        Debug.Log("PlayButton Basildi");

        SceneManager.LoadScene("GameScene");
    }

    public void OptionsButton()
    {
        Debug.Log("OptionsButton Basildi");
    }

    public void CreditsButton()
    {
        Debug.Log("CreditsButton Basildi");
    }

    public void ExitButton()
    {
        Debug.Log("ExitButton Basildi");

        Application.Quit();
    }

}