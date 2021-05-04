using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin noise; //Esta variable Se guardara la sacudida de la camara
    
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>(); //Obtiene el componente de la camara virtual
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); //Obtiene el componente de la camara virtual que se refiere a la sacudida de la camara
    }
    
    public void Shake(float duration = 0.1f, float amplitude = 1.5f, float frecuency = 20) //1. En la funcion se indican el tipo y los valores por defecto de los parametros...
    {
        StopAllCoroutines(); //Para evitar que una corutina se inicie encima de otra, con este codigo se detienen las que esten activas
        StartCoroutine(ApplyNoiseRoutine(duration, amplitude, frecuency)); //3. Y al llamar la corutina solo el nombre del parametro
    }

    IEnumerator ApplyNoiseRoutine(float duration, float amplitude, float frecuency) //2. Pero si el codigo en si esta en una corutina, los mismos parametros se indican solo con su tipo...
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frecuency;
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}