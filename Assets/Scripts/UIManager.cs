using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; //Este script(UIManager) es un singleton. Todos los manager deben ser un singleton
    [SerializeField] Text scoreText;
    [SerializeField] Text healthText;
    [SerializeField] Text timeText;
    [SerializeField] Text finalScoreText;
    [SerializeField] GameObject gameOverScreen;

    private void Awake()
    {
        if (Instance == null) //Este if junto con la variable Instance hacen de este script un singleton
        {
            Instance = this;
        }
    }

    public void UpdateUIScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    public void UpdateUIHealth(int newHealth)
    {
        healthText.text = newHealth.ToString();
    }

    public void UpdateUITime(int newTime)
    {
        timeText.text = newTime.ToString();
    }

    public void ShowGameOverScreen()
    {
        Time.timeScale = 0; //Detiene el tiempo en el juego
        gameOverScreen.SetActive(true);
        finalScoreText.text = "SCORE: " + GameManager.Instance.Score; //Al concatenar con letras no es necesario escribir .ToString() despues de .Score porque entiende que va a ser texto
    }
}