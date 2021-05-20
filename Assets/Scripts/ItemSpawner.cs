using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject checkpointPrefab; //Variable para referenciar el prefab del checkpoint en el editor
    [SerializeField] GameObject[] powerUpPrefab; //Variable array para referenciar los prefabs de los power ups en el editor
    [SerializeField] int checkpointSpawnDelay = 10; //Tiempo entre cada aparicion de checkpoint
    [SerializeField] int powerUpSpawnDelay = 12;
    [SerializeField] float spawnRadio = 5; //Radio en el que apareceran
    
    void Start()
    {
        StartCoroutine(SpawnCheckpointRoutine()); //Inicia la corutina
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnCheckpointRoutine()
    {
        while (true) //Como siempre es verdadero todo el tiempo se ejecutara el codigo
        {
            yield return new WaitForSeconds(checkpointSpawnDelay); //Espera determinados segundos y...
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadio; //Variable que guarda la posicion aleatoria dentro de cierto radio, primero se indica que sera en un radio y luego el tamaño de este
            Instantiate(checkpointPrefab, randomPosition, Quaternion.identity); //Instancia el checkpoint en la posicion aleatoria del radio y con rotacion predeterminada
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(powerUpSpawnDelay);
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadio;
            int random = Random.Range(0, powerUpPrefab.Length); //Randomizador para elegir un numero entre 0 y el largo de powerUpPrefab
            Instantiate(powerUpPrefab[random], randomPosition, Quaternion.identity);
        }
    }

    /*    
    private void OnDrawGizmos() //Funcion del sistema para mostrar gizmos. Muestra en el editor el radio del spawnRadio
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadio); //De forma esferica, con origen en el game object con este script y de determinado radio
    }
    */
}