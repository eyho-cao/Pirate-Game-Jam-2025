using System.Collections;
using UnityEngine;

public class PauseSystem : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;
    private float gameTimeScale;


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
}