using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickableLevelBox : MonoBehaviour
{
    public string sceneToLoad;

    public void OnMouseDown()
    {
        Debug.Log("Level1Button Basildi");

        SceneManager.LoadScene("SampleScene");
    }
}
