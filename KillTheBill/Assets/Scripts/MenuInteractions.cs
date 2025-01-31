using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuInteractions : MonoBehaviour
{
    public static void restartLevel() {
        Debug.Log("Restarting level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void goMainMenu() {
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene("StartMenu");
    }

    public static void goNextLevel() {
        Debug.Log("Loading Next Level");
        Scene scene = SceneManager.GetActiveScene();
        int sceneName = 0;
        try {
            if (int.TryParse(scene.name, out sceneName)) {
                if(Application.CanStreamedLevelBeLoaded((sceneName+1).ToString())) {
                    SceneManager.LoadScene((sceneName+1).ToString());
                } else {
                    SceneManager.LoadScene("StartMenu");
                }
            }
        } catch(Exception e) {
            Debug.Log("Error: Unable to load next level due to error: " +e);
        }
    }
}
