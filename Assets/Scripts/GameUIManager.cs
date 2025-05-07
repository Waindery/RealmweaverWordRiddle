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
}
