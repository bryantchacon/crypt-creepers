using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab; //Variable de arrays de game objects para referenciar los PREFABS de los enemigos en el editor, es tipo GameObject porque se instanciara todo el game object en si
    [Range(1, 10)][SerializeField] float spawnRate = 1; //Range limita que se indique que la variable sea menor a 1 y mayor a 10, esto hara que aparezca un slider en el editor
    
    void Start() //Al primer frame del juego...
    {
        StartCoroutine(SpawnNewEnemy()); //Iniciar la coroutina SpawnNewEnemy()
    }

    IEnumerator SpawnNewEnemy()
    {
        while (true) //Bucle infinito
        {
            yield return new WaitForSeconds(1/spawnRate); //Espera la cantidad de segundos del resultado de 1 entre el spawnRate ...
            float random = Random.Range(0.0f, 1.0f); //Generador de numeros aleatorios entre 0.0 y 1.0

            if (random < GameManager.Instance.difficulty * 0.1f) //Si random es menor a 1 * 0.1, o sea, menor a 10% de 1...
            {
                Instantiate(enemyPrefab[0]); //... instancia un enemigo del array enemyPrefab en el indice 0, o sea, el enemigo fuerte
            }
            else //Si no...
            {
                Instantiate(enemyPrefab[1]); //... instancia el enemigo normal
            }
            //Este if hace que la posibilidad de que salga un enemigo fuerte sea de 10% y del normal 90% porque la dificultad en el game manager es de 1 y ese numero aqui siempre se multiplicara por 0.1
        }
    }
}