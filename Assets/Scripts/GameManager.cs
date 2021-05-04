using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //Esta variable es para acceder al codigo del Game Manager desde cualquier parte del codigo sin necesidad de crear una instancia. NOTA: No es recomendable usar variables estaticas todo el tiempo porque estas nunca se eliminan de la memoria en todo el tiempo de ejecucion del juego, asi que, si una variable no se usara mucho, no debe ser estatica, si no, se usara mucha memoria
    public int time = 30; //Tiempo que durara la partida
    public int difficulty = 1;
    public bool gameOver = false;
    [SerializeField] int score; //Variable ligada a la propiedad Score

    public int Score //Propiedad ligada a la variable score
    {
        get => score; //Obtiene el valor de la variable score por el nombre de la propiedad Score(la primer letra en mayuscula)
        set
        {
            score = value; //Asigna a la variable score el valor que se le envie a Score(esta propiedad)
            UIManager.Instance.UpdateUIScore(score); //Actualiza el score en pantalla por medio del UIManager(el cual es un singleton) con la funcion que corresponde
            if (score % 1000 == 0) //Si el score modulo de 1000 da 0, o sea, cada 1000 puntos...
            {
                difficulty++; //... aumenta la dificultad
            }
        }
    }

    private void Awake() //Al iniciar el juego...
    {
        if (Instance == null) //... si Instance esta vacia...
        {
            Instance = this; //... contendra toda la informacion que guarde esta clase
        }
    }

    private void Start()
    {
        StartCoroutine(CountdownRoutine()); //Iniciar el contador
        UIManager.Instance.UpdateUIScore(score); //Este codigo es de la propiedad Score, pero se duplica aca para que al iniciar el juego el contador de puntos no este en 000
    }

    IEnumerator CountdownRoutine() //Contador en retroceso
    {
        while (time > 0) //Mientras tiempo sea mayor a 0...
        {
            yield return new WaitForSeconds(1); //... espera un segundo y...
            time--; //... resta 1 a time
            UIManager.Instance.UpdateUITime(time);
        }

        gameOver = true;
        UIManager.Instance.ShowGameOverScreen();
    }

    public void PlayAgain()
    {
        Time.timeScale = 1; //Reanuda el tiempo en el juego
        SceneManager.LoadScene("Game");
    }    
}