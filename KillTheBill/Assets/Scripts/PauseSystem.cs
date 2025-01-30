using System.Collections;
using UnityEngine;

public class PauseSystem : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;
    private float gameTimeScale = 1.0f;


    public void pauseGame() {
        gameTimeScale = Time.timeScale;
        Time.timeScale = 0;
        Debug.Log("game paused");
        //open pause menu
        pauseMenu.SetActive(true);
    }

    public void unpauseGame() {
        Time.timeScale = gameTimeScale;
        Debug.Log("game unpaused");
        //close pause menu
        pauseMenu.SetActive(false);

    }

    public void restartLevel() {
        Time.timeScale = gameTimeScale;
        MenuInteractions.restartLevel();
    }

    public void goMainMenu() {
        Time.timeScale = gameTimeScale;
        MenuInteractions.goMainMenu();
    }
}