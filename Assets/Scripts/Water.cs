using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Player player;
    float originalSpeed;
    [SerializeField] float speedReductionRatio = 0.5f;
        
    void Start()
    {
        player = FindObjectOfType<Player>(); //Al iniciar, el juego busca el game object que sea de tipo Player, o sea, que tenga el script Player y lo obtiene
        originalSpeed = player.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) //Cuandl el player entra al charco...
    {
        if (collision.CompareTag("Player"))
        {
            player.speed *= speedReductionRatio; //Reduce la velocidad de player en su propio script
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Cuando sale de el...
    {
        if (collision.CompareTag("Player"))
        {
            player.speed = originalSpeed; //Restaura la velocidad original del player
        }
    }
}