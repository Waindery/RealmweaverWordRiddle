using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple background music controller. Attach once (e.g. in MainScene)
/// and mark it DontDestroyOnLoad so it persists across scenes.
/// </summary>
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip mainMenuClip;       // MainScene
    [SerializeField] private AudioClip levelSelectionClip; // GameScene, Game2Scene, Game3Scene, Game4Scene
    [SerializeField] private AudioClip gameClip;          // SampleScene, Sample2Scene, Sample3Scene, Sample4Scene

    private AudioSource _audioSource;
    private float gameMusicTime = 0f; // Game.mp3'in kaldığı pozisyonu sakla

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForScene(scene.name);
    }

    private void Update()
    {
        // Game.mp3 çalıyorsa pozisyonunu sürekli güncelle
        if (_audioSource.clip == gameClip && _audioSource.isPlaying)
        {
            gameMusicTime = _audioSource.time;
        }
    }

    private void PlayForScene(string sceneName)
    {
        AudioClip target = null;

        // Main menu music
        if (sceneName == "MainScene")
        {
            target = mainMenuClip;
        }
        // Level selection music (GameScene'ler)
        else if (sceneName == "GameScene"
                 || sceneName == "Game2Scene"
                 || sceneName == "Game3Scene"
                 || sceneName == "Game4Scene")
        {
            target = levelSelectionClip;
        }
        // Gameplay music (SampleScene'ler + Cinematic3Scene) - Scene'ler arasında devam eder
        else if (sceneName == "SampleScene"
                 || sceneName == "Sample2Scene"
                 || sceneName == "Sample3Scene"
                 || sceneName == "Sample4Scene"
                 || sceneName == "Cinematic3Scene")
        {
            target = gameClip;
        }

        // If no clip mapped, do nothing
        if (target == null)
        {
            return;
        }

        // Eğer Game.mp3 çalıyorsa, pozisyonunu sakla
        if (_audioSource.clip == gameClip && _audioSource.isPlaying)
        {
            gameMusicTime = _audioSource.time;
        }

        // GameScene'lere geçildiğinde Game.mp3'i durdur ve LevelSelection.mp3'e geç
        if (sceneName == "GameScene"
            || sceneName == "Game2Scene"
            || sceneName == "Game3Scene"
            || sceneName == "Game4Scene")
        {
            // LevelSelection.mp3 çalıyorsa devam et
            if (_audioSource.clip == levelSelectionClip && _audioSource.isPlaying)
            {
                return;
            }

            // LevelSelection.mp3'e geç
            _audioSource.clip = levelSelectionClip;
            _audioSource.time = 0f; // Baştan başla
            _audioSource.Play();
            return;
        }

        // SampleScene'lerde Game.mp3 çal
        if (target == gameClip)
        {
            // Eğer zaten Game.mp3 çalıyorsa ve aynı pozisyondaysa devam et
            if (_audioSource.clip == gameClip && _audioSource.isPlaying)
            {
                return;
            }

            // Game.mp3'i kaldığı yerden devam ettir
            _audioSource.clip = gameClip;
            _audioSource.time = gameMusicTime;
            _audioSource.Play();
            return;
        }

        // MainScene için
        if (target == mainMenuClip)
        {
            if (_audioSource.clip == mainMenuClip && _audioSource.isPlaying)
            {
                return;
            }

            _audioSource.clip = mainMenuClip;
            _audioSource.time = 0f;
            _audioSource.Play();
        }
    }
}
