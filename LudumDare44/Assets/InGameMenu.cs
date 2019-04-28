using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    private float originalTimeScale;
    public GameObject pauseMenu;

    private bool gamePaused = false;

    public void Start() {originalTimeScale = Time.timeScale;}

    public void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) {
            PauseGameToggle();
            pauseMenu.SetActive(gamePaused);
        }
    }

    public void PauseGameToggle() {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0f : originalTimeScale;
    }
}
