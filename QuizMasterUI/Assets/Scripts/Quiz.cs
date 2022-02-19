using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")] // "Header" grouping of SerializeField's
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>(); // bc serialized, " = new List<QuestionSO>()" not needed, unaffects code function; including adds more protection if field changes to non-serialized, as will be needed in that event
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true; // if timer ran out or clicked button to show answer
                                  // initialized to true to auto-fail "else if" conditional of first Update() loop of game, forcing next loop, in which if/else block will function as intended

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progrssBar;

    public bool isComplete;
    
    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progrssBar.maxValue = questions.Count;
        progrssBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        
        // check state of timer, to check if getNextQ
        if(timer.loadNextQuestion)
        {
            if (progrssBar.value == progrssBar.maxValue) // on final q, when tries to loadNextQ and List empty, will be caught w/ this "if"
            {
                isComplete = true; // changes gameComplete flag to true, signaling end of q's/game
                return; // returns us out of Update() loop early
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false; // stops from getNewQ every frame
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion) // timer not in answeringQ state
        {
            DisplayAnswer(-1); // if timer has run to 0, needs to display answer on screen
                               // int index param needed; not clicking buttons, don't want to accidentally pass in a potentially correct answer, so can't use a number between 0-3 (answerArrayElms); this forces us into else block of DispalyAnswer()
                               // common practice in this case is to pass in -1 as param; still works because it definently will not by chance match the correctAnswerIndex; thus will auto-fall into else block
            SetButtonState(false);
        }

    }

    public void onAnswerSelected(int index) // click an answer button
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer(); // cuts timer short, puts it into its next state
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage;

        if (index == currentQuestion.GetCorrectAnswerIndex()) // clicked correct answer button
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, but the correct answer was:\n" + correctAnswer;

            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void GetNextQuestion()
    {
        if(questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progrssBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }       
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count); // returns random int between 0 and "last elm" in List
        currentQuestion = questions[index]; // q from List<QuestionSO> at int (random^) index
        
        if(questions.Contains(currentQuestion)) // best practice: when removing an item from a List, always check first to see if it exists within the List before trying to remove it; avoids any potential errors, best to err on side of caution
        {
            questions.Remove(currentQuestion); // remove q from List<QuestionSO>, to avoid repeat
        }
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }

    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++) 
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

}
