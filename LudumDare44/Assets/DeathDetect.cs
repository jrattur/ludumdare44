using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathDetect : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject gameController;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        gameController.GetComponent<InGameMenu>().PauseGameToggle();
        gameOverMenu.SetActive(true);
    }

    public void RestartLevel() {
        SceneManager.LoadScene(1);
        gameController.GetComponent<InGameMenu>().PauseGameToggle();
    }

    public void Quit() { Application.Quit(); }

}
