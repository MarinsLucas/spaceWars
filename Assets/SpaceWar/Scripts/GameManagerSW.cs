using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameManagerSW : MonoBehaviour
{
    [Header("Parametros")]
    [SerializeField] float intervalTimeChange;
    [SerializeField] float timeIncrease; 
    float timeChange;

    [Header("Canvas")]
    [SerializeField] GameObject inGamePanel;
    [SerializeField] GameObject restartPanel;
    [SerializeField] Text healthText;
    [SerializeField] Text scoreText; 

    public bool isRunning;

    public int score; 
    int nextSuply = 100; 

    public PlayerSW player; 

    public static GameManagerSW instance;

    void Awake()
    {
        instance = this; 
        isRunning = true; 
    }

    void Start()
    {
        restartPanel.SetActive(!isRunning);
        inGamePanel.SetActive(isRunning);
        Time.timeScale = 1f; 
        timeChange = intervalTimeChange; 
    }

    public void GameOver()
    {
        isRunning = false;
        restartPanel.SetActive(!isRunning);
        inGamePanel.SetActive(isRunning);
        Time.timeScale = 0f; 
    }

    public bool AddPoints(int points)
    {
        score += points; 
        nextSuply -= points;
        if(nextSuply <= 0)
        {
            nextSuply = 100; 
            return true;
        }
        return false; 
    }

    void Update()
    {
        if(player == null)
        {
            GameOver(); 
        }
        
        scoreText.text = score.ToString();
        healthText.text = player.health.ToString();

        //aumenta a dificuldade do jogo
        if(timeChange<=0)
        {  
            Time.timeScale += timeIncrease;
            timeChange = intervalTimeChange;
            Debug.Log("Aumento de dificuldade");
        }
        timeChange -= Time.deltaTime; 
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
