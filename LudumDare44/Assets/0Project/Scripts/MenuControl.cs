using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    public GameObject credits, mainmenu;

    public void LoadLevel(int level) { SceneManager.LoadScene(level); }

    public void QuitGame() { Application.Quit(); }

    public void ToggleCredits() {
        mainmenu.SetActive(!mainmenu.activeInHierarchy);
        credits.SetActive(!mainmenu.activeInHierarchy);
    }

}
