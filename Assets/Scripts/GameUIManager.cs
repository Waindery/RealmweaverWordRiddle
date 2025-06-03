using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public void MainMenuButton()
    {
        Debug.Log("MainMenuButton Basildi");

        SceneManager.LoadScene("MainScene");
    }

    public void Level1Button ()
    {
        Debug.Log("Level1Button Basildi");

        SceneManager.LoadScene("SampleScene");
    }

    public void Level2Button()
    {
        Debug.Log("Level2Button Basildi");

        SceneManager.LoadScene("Sample2Scene");
    }
}
