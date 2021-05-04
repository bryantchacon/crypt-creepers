using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int extraTime = 10; //Tiempo extra que se añadira al cronometro
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Si choca contra el player...
        {
            GameManager.Instance.time += extraTime; //Añade tiempo al cronometro, el cual esta en el Game Manager y...
            Destroy(gameObject, 0.1f); //... el checkpoint se destruye despues de 0.1 segundos
        }
    }
    //NOTA: Recordar que para que esto funcione el player que es quien se mueve debe tener un rigid body con el Gravity Scale en 0
}