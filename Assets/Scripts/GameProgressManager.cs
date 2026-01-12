using UnityEngine;

/// <summary>
/// Oyuncunun ilerlemesini kaydeden ve yöneten sınıf
/// </summary>
public static class GameProgressManager
{
    private const string LAST_GAME_SCENE_KEY = "LastGameScene";
    private const string COMPLETED_LEVEL_KEY = "CompletedLevel";

    /// <summary>
    /// Bir level tamamlandığında çağrılır ve ilerlemeyi kaydeder
    /// </summary>
    /// <param name="completedSceneName">Tamamlanan SampleScene adı (SampleScene, Sample2Scene, vb.)</param>
    public static void OnLevelCompleted(string completedSceneName)
    {
        string nextGameScene = GetNextGameScene(completedSceneName);
        
        if (!string.IsNullOrEmpty(nextGameScene))
        {
            PlayerPrefs.SetString(LAST_GAME_SCENE_KEY, nextGameScene);
            
            // Level numarasını da kaydet (SampleScene = 1, Sample2Scene = 2, vb.)
            int levelNumber = GetLevelNumber(completedSceneName);
            if (levelNumber > 0)
            {
                PlayerPrefs.SetInt(COMPLETED_LEVEL_KEY, levelNumber);
            }
            
            PlayerPrefs.Save();
            Debug.Log($"Level completed: {completedSceneName} -> Next GameScene: {nextGameScene}");
        }
    }

    /// <summary>
    /// Kaydedilmiş GameScene'i döndürür, yoksa varsayılan olarak "GameScene" döner
    /// </summary>
    public static string GetLastGameScene()
    {
        string lastScene = PlayerPrefs.GetString(LAST_GAME_SCENE_KEY, "GameScene");
        return lastScene;
    }

    /// <summary>
    /// Tamamlanan level numarasını döndürür
    /// </summary>
    public static int GetCompletedLevel()
    {
        return PlayerPrefs.GetInt(COMPLETED_LEVEL_KEY, 0);
    }

    /// <summary>
    /// İlerlemeyi sıfırlar (debug/test için)
    /// </summary>
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LAST_GAME_SCENE_KEY);
        PlayerPrefs.DeleteKey(COMPLETED_LEVEL_KEY);
        PlayerPrefs.Save();
        Debug.Log("Game progress reset!");
    }

    /// <summary>
    /// Tamamlanan SampleScene'e göre bir sonraki GameScene'i döndürür
    /// Not: Sample3Scene geçildiğinde Cinematic3Scene'e gidiyor ama sonra Game4Scene açılıyor
    /// </summary>
    private static string GetNextGameScene(string completedSceneName)
    {
        switch (completedSceneName)
        {
            case "SampleScene":
                return "Game2Scene"; // SampleScene geçildi → Game2Scene açık olmalı
            case "Sample2Scene":
                return "Game3Scene"; // Sample2Scene geçildi → Game3Scene açık olmalı
            case "Sample3Scene":
                return "Game4Scene"; // Sample3Scene geçildi → Game4Scene açık olmalı (Cinematic3Scene sonrası)
            case "Sample4Scene":
                return "Game4Scene"; // Son level, Game4Scene'de kal
            default:
                return null;
        }
    }

    /// <summary>
    /// Scene adından level numarasını çıkarır
    /// </summary>
    private static int GetLevelNumber(string sceneName)
    {
        if (sceneName == "SampleScene")
            return 1;
        else if (sceneName == "Sample2Scene")
            return 2;
        else if (sceneName == "Sample3Scene")
            return 3;
        else if (sceneName == "Sample4Scene")
            return 4;
        
        return 0;
    }
}
