using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform player;
    [SerializeField] int enemyHealth = 1;
    [SerializeField] float enemySpeed = 1;
    [SerializeField] int scorePoints = 100;
    [SerializeField] AudioClip impactClip;

    private void Start() //Al instanciarse el enemigo desde el EnemySpawnController...
    {
        player = FindObjectOfType<Player>().transform; //Obtiene el transform del player, porque con el lo seguira

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint"); //Arreglo de spawn points, encuentra y almacena todos LOS(por que FindGameObjectsWithTag() esta en plural) game objects con la etiqueta SpawnPoint
        int randomSpawnPoint = Random.Range(0, spawnPoints.Length); //Elige aleatoriamente uno de los spawn points de la lista, porque su rango es entre 0 y el largo de la lista
        transform.position = spawnPoints[randomSpawnPoint].transform.position; //Asigna la posicion del spawn point elegido, al enemigo actual, y desde ahi aparece
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position; //Coordenadas para seguir al player
        transform.position += (Vector3)direction.normalized * enemySpeed * Time.deltaTime; //Velocidad en la que ira(o sea, hacia el player). .normalized hace lo mismo que la siguiente funcion comentada, la cual para usarla se deben comentar estas dos primeras        
    }

    public void EnemyTakeDamage()
    {
        enemyHealth--;
        AudioSource.PlayClipAtPoint(impactClip, transform.position);

        if (enemyHealth <= 0)
        {
            GameManager.Instance.Score += scorePoints;
            Destroy(gameObject, 0.1f); //Despues de una decima de segundo el enemigo se destruye
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //NOTA: Para que OnTriggerEnter2D() funcione, el game object ademas de un trigger collider, debe tener un rigidbody con el parametro Gravity Scale en 0
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().PlayerTakeDamage();
        }
    }
}