using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 5;
    [SerializeField] int health = 3;
    public bool powerShot; //Es publico porque se accede a el desde el script del player

    private void Start()
    {
        Destroy(gameObject, 5); //Luego de ser instanciada, la misma bala(gameObject) se destruira despues de 5 segundos
    }
    
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed; //Hace que la bala salga disparada hacia la derecha
    }

    private void OnTriggerEnter2D(Collider2D collision) //NOTA: Para que OnTriggerEnter2D() funcione, el game object ademas de un trigger collider, debe tener un rigidbody con el parametro Gravity Scale en 0
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().EnemyTakeDamage(); //Obtiene el componente Enemy(el script) del enemigo y ejecuta la funcion TakeDamage(), todo esto siempre y cuando sea el enemigo contra el que se choca, y como es al chocar contra el, no se necesita que se pase como parametro desde el editor

            if (!powerShot) //Si la bala no es power shot...
            {
                Destroy(gameObject); //La bala se destruye al colisionar con el enemy, gameObject se refiere a la misma bala
            }

            //Si el if de arriba se cumple, la bala se destruira y el codigo siguiente no se ejecutara porque la bala ya no existe

            health--; //Si el if no se cumple y si es un power shot, la vida de la bala se reduce en 1 hasta que llegue a 0(por colisionar con tres enemigos) y...

            if (health <= 0) //... si la vida de la bala llega a 0...
            {
                Destroy(gameObject); //... la bala se destruira
            }
        }
    }
}