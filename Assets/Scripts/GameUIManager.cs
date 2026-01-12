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

    public void Level1Button()
    {
        Debug.Log("Level1Button Basildi");

        SceneManager.LoadScene("SampleScene");
    }

    public void Level2Button()
    {
        Debug.Log("Level2Button Basildi");

        SceneManager.LoadScene("Sample2Scene");
    }

    public void Level3Button()
    {
        Debug.Log("Level3Button Basildi");

        SceneManager.LoadScene("Sample3Scene");
    }

    public void Level4Button()
    {
        Debug.Log("Level4Button Basildi");

        SceneManager.LoadScene("Sample4Scene");
    }

    public void Level5Button()
    {
        Debug.Log("Level5Button Basildi");

        SceneManager.LoadScene("Sample5Scene");
    }
}
