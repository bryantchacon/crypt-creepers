using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float horizontalAxis;
    float verticalAxis;
    bool loadedGun = true;
    bool powerShotEnabled;
    bool invulnerable;
    public float speed = 8;
    Vector3 playerMoveDirection;
    Vector2 aimDirection; //Variable de la direccion hacia donde mirara la mira
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform aim; //Variable para poder modificar la posicion de Aim(ya que se vinculara desde el editor), esto porque es de tipo Transform
    [SerializeField] Transform bulletPrefab; //Variable para referenciar el PREFAB de la bala en el editor, es tipo Transform porque usara su posicion
    [SerializeField] int playerHealth = 10; //Esta variable es una propiedad
    [SerializeField] float fireRate = 1; //Velocidad de disparo
    [SerializeField] float invulnerableTime = 3;
    [SerializeField] Animator playerAnimation; //Variable para indicar que animacion mostrar
    [SerializeField] SpriteRenderer spriteRenderer; //Variable para indicar a que lado debe mirar el player al girar y para el parpadeo de su sprite
    [SerializeField] float blinkRate = 0.02f; //Frecuencia del parpadeo
    [SerializeField] CameraController camController; //Referencia a la CM vcam1(camara de cinemachine)

    public int PlayerHealth
    {
        get { return playerHealth; }
        set
        {
            playerHealth = value;
            UIManager.Instance.UpdateUIHealth(playerHealth);
        }
    } //playerHealth se asigna a value, que es el valor de set, y ahora con este valor solo se puede usar dentro de set

    // Start is called before the first frame update
    void Start()
    {
        camController = FindObjectOfType<CameraController>(); //FindObjectOfType se usa para buscar un componente de otro game object
        UIManager.Instance.UpdateUIHealth(playerHealth); //Este codigo es de la propiedad PlayerHealth, pero se duplica aca para que al iniciar el juego el contador de salud tenga tres 0, si no la que se le asigna desde el editor(por se SerializeField, porque no es publica)
    }
    
    void Update()
    {        
        ReadInput(); //Lee la entrada de la direccion del movimiento del player
                
        transform.position += playerMoveDirection * speed * Time.deltaTime; //Asignacion de la velocidad y movimiento del player. Time.deltaTime es el tiempo que transcurre entre un frame y otro, esto se usa para que el renderizado del game object no se trabe
                
        aimDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position; //Direccion en la que apuntara la mira. Indica que la direccion(coordenadas) de la mira sera la misma que la del mouse porque traduce el punto del espacio en pantalla donde este el mouse, al punto del espacio del mundo del juego donde este el mouse y se le resta la posicion del jugador para obtener la direccion
        aim.position = (Vector3)aimDirection.normalized + transform.position; //Normaliza(se vuelve 1) la distancia de la mira al jugador y se le suma la posicion del jugador para que lo siga. (Vector3) es para castear aimDirection que es de tipo Vector2, o sea, que lo interprete como tal, esto para que la distancia al player sea constante y al apuntar al lado opuesto no pase por encima de este, asi, la mira rota fija 360° al moverla
                
        if (Input.GetMouseButton(0) && loadedGun) //Instanciacion de la bala. Si se mantiene presionado el clic izquierdo del mouse y loadedGun es positivo
        {
            Shoot();
        }
                
        UpdatePlayerGraphics(); //Actualiza los graficos del player segun hacia donde mire
    }

    void ReadInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        playerMoveDirection.x = horizontalAxis;
        playerMoveDirection.y = verticalAxis;
    }

    void Shoot()
    {
        loadedGun = false; //loadedGun se vuelve falso...
        float bulletAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //ANGULO en que saldra la bala. aimDirection se puede usar aqui por que su valor se actualiza en el Update(). ... obtiene el angulo de la mira, esto al obtener sus coordenadas en "y" y "x"(en este orden) y se multiplica por Rad2Deg para pasarlo de radianes a grados y asi obtener el angulo. NOTA: Un radian es el arco de un angulo, su longitud es la misma que la del radio...
        Quaternion bulletAngleAndDirection = Quaternion.AngleAxis(bulletAngle, Vector3.forward); //DIRECCION en la que saldra la bala. ... se indica que la bala saldra en el angulo obtenido en bulletAngle y en direccion hacia adelante. La variable es de tipo Quaternion porque almacena un valor del mismo tipo...
        Transform bulletInstance = Instantiate(bulletPrefab, transform.position, bulletAngleAndDirection); //Instanciacion de la bala. ... la bala se instanciara, desde el player y con la inclinacion y direccion de bulletAngleAndDirection, o sea, hace coincidir la direccion de la mira con el de la bala al instanciarla, esto se guarda en esta variable bulletInstance, pero...
        if (powerShotEnabled) //... si powerShotEnabled es true(se detecta en el on trigger enter de aca)...
        {
            bulletInstance.GetComponent<Bullet>().powerShot = true; //... indicara al script Bullet de la bala que esta y las siguientes seran de ese tipo
        }
        StartCoroutine(ReloadGun()); //... y ejecuta la corutina de recarga para volver a disparar al hacer clic
    }

    void UpdatePlayerGraphics()
    {
        playerAnimation.SetFloat("Speed", playerMoveDirection.magnitude); //Indica al animator del player activar la animacion de correr por aumentar el parametro speed al pasarle el valor de la magnitud del movimiento de player
        if (aim.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (aim.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    IEnumerator ReloadGun() //Corutina de recarga para volver a disparar al hacer clic
    {
        yield return new WaitForSeconds(1/fireRate); //Espera por determinada cantidad de segundos o fraccion de estos para volver a disparar al hacer clic, para esto...
        loadedGun = true; //... loadedGun se vuelve true para poder disparar de nuevo al hacer clic
    }    

    public void PlayerTakeDamage()
    {
        if (invulnerable) //Si invulnerable es true, se ejecuta el return el cual evita que el codigo que sigue despues de este if se ejecute, o sea, evita que se siga quitando vida al player
        {
            return;
        }

        PlayerHealth--; //Si invulnerable es false el codigo se ejecuta a partir de aca
        invulnerable = true; //Invulnerable se hace true para evitar que se siga bajando la vida del personaje al chocar con los enemigos
        fireRate = 1;
        powerShotEnabled = false;
        camController.Shake(); //Sacudir la camara
        StartCoroutine(MakeVulnerableAgain()); //Se espera lo que indique el contador y luego de eso se puede hacer daño al personaje otra vez
        if (PlayerHealth <= 0)
        {
            GameManager.Instance.gameOver = true;
            UIManager.Instance.ShowGameOverScreen();
        }
    }

    IEnumerator MakeVulnerableAgain()
    {
        StartCoroutine(BlinkRoutine()); //Activa el parpadeo
        yield return new WaitForSeconds(invulnerableTime);
        invulnerable = false;
    }

    IEnumerator BlinkRoutine() //Genera un parpadeo en el player
    {
        int blinks = 10;
        while(blinks > 0)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinks * blinkRate);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinks * blinkRate);
            blinks--;
        }
        //Empieza parpadeando rapido y se va deteniendo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            switch (collision.GetComponent<PowerUp>().powerUpType) //Switch que checara si el valor de la variable que se indica coincide con alguno de los cases
            {
                case PowerUp.PowerUpType.FireRateIncrease: //Si es FireRateIncrease...
                    fireRate++; //Se incrementara fireRate
                    break; //Fin de la ejecucion de case
                case PowerUp.PowerUpType.PowerShot: //O si es PowerShot...
                    powerShotEnabled = true; //Esto para aumentar la vida de las balas siguientes desde su instanciacion aqui, indicando al script Bullet de cada bala que aumente
                    break;
            }
            Destroy(collision.gameObject, 0.1f); //Destruye el power up con el que se choco despues de una decima de segundo
        }
    }
}