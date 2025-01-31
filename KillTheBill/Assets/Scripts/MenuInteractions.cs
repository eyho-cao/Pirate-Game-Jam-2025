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
}
