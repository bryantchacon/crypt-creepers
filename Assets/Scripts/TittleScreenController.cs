using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TittleScreenController : MonoBehaviour
{
    [SerializeField] AudioClip buttonClip;

    public void ClickOnPlayGameButton()
    {
        AudioSource.PlayClipAtPoint(buttonClip, transform.position);
        Invoke("LoadGame", 0.5f);
    }

    void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
}