using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Quiz quiz;
    EndScreen endScreen;
    
    void Awake() 
    {
        quiz = FindObjectOfType<Quiz>();
        endScreen = FindObjectOfType<EndScreen>();
    }
    // Awake() at very beginning of Unity Script Execution order; Start() when game starts, before Update()
    // best to use FindObjectOfType on Awake() vs Start() to avoid bugs; will "find obj's" before "game start"

    void Start()
    {        
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
    }
    // setting "found obj's" in Awake() to "active" or not at "game start"

    void Update()
    {
        if(quiz.isComplete)
        {
            quiz.gameObject.SetActive(false);
            endScreen.gameObject.SetActive(true);
            endScreen.ShowFinalScore();
        }
    }

    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // gets buildIndex of currentlyActiveScene, and tells sceneManager to load that scene again; refreshes screen, scores, and allows user to play scene again (our game is just 1 scene, can be used to for level selection)
    }
}
