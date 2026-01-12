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

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float mainMenuVolume = 1f;         // MainMenu.mp3 ses seviyesi (0-1 arası)
    [Range(0f, 1f)]
    [SerializeField] private float levelSelectionVolume = 1f;   // LevelSelection.mp3 ses seviyesi (0-1 arası)
    [Range(0f, 1f)]
    [SerializeField] private float gameVolume = 1f;             // Game.mp3 ses seviyesi (0-1 arası)

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

        // Her müziğin kendi volume ayarını AudioSource'a uygula
        if (_audioSource.clip == mainMenuClip && _audioSource.isPlaying)
        {
            _audioSource.volume = mainMenuVolume;
        }
        else if (_audioSource.clip == levelSelectionClip && _audioSource.isPlaying)
        {
            _audioSource.volume = levelSelectionVolume;
        }
        else if (_audioSource.clip == gameClip && _audioSource.isPlaying)
        {
            _audioSource.volume = gameVolume;
        }
    }

    /// <summary>
    /// MainMenu müziğinin ses seviyesini ayarlar (0-1 arası)
    /// </summary>
    public void SetMainMenuVolume(float volume)
    {
        mainMenuVolume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// LevelSelection müziğinin ses seviyesini ayarlar (0-1 arası)
    /// </summary>
    public void SetLevelSelectionVolume(float volume)
    {
        levelSelectionVolume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Game müziğinin ses seviyesini ayarlar (0-1 arası)
    /// </summary>
    public void SetGameVolume(float volume)
    {
        gameVolume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// MainMenu müziğinin ses seviyesini döndürür
    /// </summary>
    public float GetMainMenuVolume()
    {
        return mainMenuVolume;
    }

    /// <summary>
    /// LevelSelection müziğinin ses seviyesini döndürür
    /// </summary>
    public float GetLevelSelectionVolume()
    {
        return levelSelectionVolume;
    }

    /// <summary>
    /// Game müziğinin ses seviyesini döndürür
    /// </summary>
    public float GetGameVolume()
    {
        return gameVolume;
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
        // Gameplay music (SampleScene'ler + Cinematic3Scene + Cinematic4Scene) - Scene'ler arasında devam eder
        else if (sceneName == "SampleScene"
                 || sceneName == "Sample2Scene"
                 || sceneName == "Sample3Scene"
                 || sceneName == "Sample4Scene"
                 || sceneName == "Cinematic3Scene"
                 || sceneName == "Cinematic4Scene")
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
            _audioSource.volume = levelSelectionVolume;
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
            _audioSource.volume = gameVolume;
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
            _audioSource.volume = mainMenuVolume;
            _audioSource.Play();
        }
    }
}
