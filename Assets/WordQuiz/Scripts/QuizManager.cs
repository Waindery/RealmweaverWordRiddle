using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance; //Instance to make is available in other scripts without reference

    [SerializeField] private GameObject gameComplete;
    [SerializeField] private QuizDataScriptable questionDataScriptable;
    [SerializeField] private Image questionImage;           //image element to show the image
    [SerializeField] private WordData[] answerWordList;     //list of answers word in the game
    [SerializeField] private WordData[] optionsWordList;    //list of options word in the game
    [SerializeField] private AudioClip correctSoundClip;    //sound clip to play when answer is correct
    [SerializeField] private AudioClip wrongSoundClip;      //sound clip to play when answer is wrong
    [SerializeField] private AudioClip wololoSoundClip;     //sound clip to play when game is completed
    private AudioSource audioSource;                        //audio source component for playing sounds


    private GameStatus gameStatus = GameStatus.Playing;     //to keep track of game status
    private char[] wordsArray = new char[12];               //array which store char of each options

    private List<int> selectedWordsIndex;                   //list which keep track of option word index w.r.t answer word index
    private int currentAnswerIndex = 0, currentQuestionIndex = 0;   //index to keep track of current answer and current question
    private bool correctAnswer = true;                      //bool to decide if answer is correct or not
    private string answerWord;                              //string to store answer of current question

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedWordsIndex = new List<int>();           //create a new list at start
        
        // AudioSource component'ini al veya ekle
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        
        SetQuestion();                                  //set question
    }

    void SetQuestion()
    {
        gameStatus = GameStatus.Playing;                //set GameStatus to playing 

        //set the answerWord string variable
        answerWord = questionDataScriptable.questions[currentQuestionIndex].answer;
        //set the image of question
        questionImage.sprite = questionDataScriptable.questions[currentQuestionIndex].questionImage;
            
        ResetQuestion();                               //reset the answers and options value to orignal     

        selectedWordsIndex.Clear();                     //clear the list for new question
        Array.Clear(wordsArray, 0, wordsArray.Length);  //clear the array

        //add the correct char to the wordsArray
        for (int i = 0; i < answerWord.Length; i++)
        {
            wordsArray[i] = char.ToUpper(answerWord[i]);
        }

        //add the dummy char to wordsArray
        for (int j = answerWord.Length; j < wordsArray.Length; j++)
        {
            wordsArray[j] = (char)UnityEngine.Random.Range(65, 90);
        }

        wordsArray = ShuffleList.ShuffleListItems<char>(wordsArray.ToList()).ToArray(); //Randomly Shuffle the words array

        //set the options words Text value
        for (int k = 0; k < optionsWordList.Length; k++)
        {
            optionsWordList[k].SetWord(wordsArray[k]);
        }

    }

    //Method called on Reset Button click and on new question
    public void ResetQuestion()
    {
        //activate all the answerWordList gameobject and set their word to "_"
        for (int i = 0; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(true);
            answerWordList[i].SetWord('_');
        }

        //Now deactivate the unwanted answerWordList gameobject (object more than answer string length)
        for (int i = answerWord.Length; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(false);
        }

        //activate all the optionsWordList objects
        for (int i = 0; i < optionsWordList.Length; i++)
        {
            optionsWordList[i].gameObject.SetActive(true);
        }

        currentAnswerIndex = 0;
    }

    /// <summary>
    /// When we click on any options button this method is called
    /// </summary>
    /// <param name="value"></param>
    public void SelectedOption(WordData value)
    {
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;

        selectedWordsIndex.Add(value.transform.GetSiblingIndex());
        value.gameObject.SetActive(false);
        answerWordList[currentAnswerIndex].SetWord(value.wordValue);

        currentAnswerIndex++;
        if (currentAnswerIndex == answerWord.Length)
        {
            correctAnswer = true;
            for (int i = 0; i < answerWord.Length; i++)
            {
                if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordList[i].wordValue))
                {
                    correctAnswer = false;
                    break;
                }
            }

            if (correctAnswer)
            {
                Debug.Log("Correct Answer");
                
                gameStatus = GameStatus.Next;
                currentQuestionIndex++;

                if (currentQuestionIndex < questionDataScriptable.questions.Count)
                {
                    // Doğru cevap sesini çal (son soru değilse)
                    PlayCorrectSound();
                    Invoke("SetQuestion", 0.5f);
                }
                else
                {
                    Debug.Log("Game Complete");
                    // GameComplete ekranını gösterme, sadece Wololo sesini çal
                    
                    // Scene geçişinden önce Wololo sesini çal ve bekle
                    StartCoroutine(LoadNextSceneWithWololo());
                }
            }
            else
            {
                Debug.Log("Wrong Answer");
                
                // Yanlış cevap sesini çal
                PlayWrongSound();
                
                // Kelime ekranda kalacak, kullanıcı silme/geri alma tuşlarını kullanabilir
            }
        }
    }

    public void ResetLastWord()
    {
        if (selectedWordsIndex.Count > 0)
        {
            int index = selectedWordsIndex[selectedWordsIndex.Count - 1];
            optionsWordList[index].gameObject.SetActive(true);
            selectedWordsIndex.RemoveAt(selectedWordsIndex.Count - 1);

            currentAnswerIndex--;
            answerWordList[currentAnswerIndex].SetWord('_');
        }
    }

    private void PlayCorrectSound()
    {
        if (correctSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(correctSoundClip);
        }
    }

    private void PlayWrongSound()
    {
        if (wrongSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(wrongSoundClip);
        }
    }


    private IEnumerator LoadNextSceneWithWololo()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string nextSceneName = "";

        // Mevcut scene'e göre bir sonraki scene'i belirle
        switch (currentSceneName)
        {
            case "SampleScene": // Level 1
                nextSceneName = "Game2Scene";
                break;
            case "Sample2Scene": // Level 2
                nextSceneName = "Game3Scene";
                break;
            case "Sample3Scene": // Level 3
                nextSceneName = "Cinematic3Scene"; // ara sahne
                break;
            case "Sample4Scene": // Level 4
                nextSceneName = "Game4Scene"; // Tekrar Game4Scene'e dön
                break;
            default:
                // Eğer bilinmeyen bir scene'deyse varsayılan olarak Game2Scene'e git
                nextSceneName = "Game2Scene";
                Debug.LogWarning($"Unknown scene: {currentSceneName}, loading Game2Scene as default");
                break;
        }

        // Wololo sesini çal
        if (wololoSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(wololoSoundClip);
            
            // Sesin bitmesini bekle
            yield return new WaitForSeconds(wololoSoundClip.length);
        }

        Debug.Log($"Level completed! Loading next scene: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }

}

[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}

public enum GameStatus
{
   Next,
   Playing
}
