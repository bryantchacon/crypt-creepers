using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //Singleton del GameManager. NOTA: No es recomendable usar variables estaticas todo el tiempo porque estas nunca se eliminan de la memoria en todo el tiempo de ejecucion del juego, asi que, si una variable no se usara mucho, no debe ser estatica, si no, se usara mucha memoria
    public int time = 30; //Tiempo que durara la partida
    public int difficulty = 1;
    public bool gameOver = false;
    [SerializeField] int score; //Variable con propiedad Score

    public int Score //Propiedad de la variable score
    {
        get { return score; } //El valor de la propiedad Score es el valor de la variable score

        set
        {
            score = value;

            UIManager.Instance.UpdateUIScore(score); //Actualiza el score en pantalla por medio del UIManager
            if (score % 1000 == 0) //Si el score modulo de 1000 da 0, o sea, cada 1000 puntos...
            {
                difficulty++; //... aumenta la dificultad
            }
        }
    }
    //NOTA: Una propiedad tiene dos secciones: get; que devuelve el valor de una variable innacesible desde fuera de la clase cuando la propiedad es consultada, y set; que recibe un valor dentro de la palabra reservada value, la cual es del mismo tipo de la propiedad(en este caso la variable score), y es utilizada dentro de la misma

    private void Awake() //Al iniciar el juego...
    {
        if (Instance == null) //... si Instance esta vacia...
        {
            Instance = this; //... contendra toda la informacion que guarde esta clase
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(CountdownRoutine()); //Iniciar el contador
        UIManager.Instance.UpdateUIScore(score); //Este codigo es de la propiedad Score, pero esta aqui para que al iniciar el juego el contador de puntos no tenga los tres 0 que se le asigna por default, si no solo uno, por que asi inicia la variable en el editor
        UIManager.Instance.UpdateUITime(time); //Este codigo es del contador, pero esta aqui para que al iniciar el juego el contador de tiempo no tenga tres 0 que se le asigna por default, si no la cantidad del tiempo asignado en el editor
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
        Time.timeScale = 1; //Reanuda el tiempo en el juego al jugar otra partida
        SceneManager.LoadScene("Game");
    }    
}