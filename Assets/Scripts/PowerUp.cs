using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        FireRateIncrease,
        PowerShot
    }

    public PowerUpType powerUpType; //Para poder usar los elementos de un enum, en la clase siempre debe haber una variable con el mismo tipo de dato que el enum
}