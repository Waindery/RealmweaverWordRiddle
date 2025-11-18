using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [Header("Cinematic Settings")]
    [SerializeField] private float cinematicDuration = 5f; // Duration before auto-loading next scene
    [SerializeField] private string nextSceneName = "GameScene"; // Scene to load after cinematic
    
    [Header("UI Elements")]
    [SerializeField] private GameObject skipText; // Optional: UI text showing "Press Enter to Skip"
    
    private bool isSkipped = false;
    private Coroutine cinematicCoroutine;

    void Start()
    {
        // Show skip text if assigned
        if (skipText != null)
        {
            skipText.SetActive(true);
        }

        // Start the cinematic coroutine
        cinematicCoroutine = StartCoroutine(CinematicSequence());
    }

    void Update()
    {
        // Check for Enter key press to skip
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!isSkipped)
            {
                SkipCinematic();
            }
        }
    }

    private IEnumerator CinematicSequence()
    {
        // Wait for the cinematic duration
        yield return new WaitForSeconds(cinematicDuration);

        // Load next scene if not skipped
        if (!isSkipped)
        {
            LoadNextScene();
        }
    }

    private void SkipCinematic()
    {
        isSkipped = true;
        
        // Stop the coroutine
        if (cinematicCoroutine != null)
        {
            StopCoroutine(cinematicCoroutine);
        }

        // Hide skip text
        if (skipText != null)
        {
            skipText.SetActive(false);
        }

        // Load next scene immediately
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}


