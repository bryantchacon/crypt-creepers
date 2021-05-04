using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    float originalSpeed;
    Player player;
    [SerializeField] float speedReductionRatio = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>(); //Al iniciar, el juego busca el game object que sea de tipo Player, o sea, que tenga el script Player y lo obtiene
        originalSpeed = player.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) //Cuandl el player entra al charco...
    {
        if (collision.CompareTag("Player"))
        {
            player.speed *= speedReductionRatio; //Indica a la velocidad de player en su script que su velocidad se redujo
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Cuando sale de el...
    {
        if (collision.CompareTag("Player"))
        {
            player.speed = originalSpeed; //Restaura la velocidad original de player
        }
    }
}