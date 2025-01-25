using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWonSystem : MonoBehaviour
{

    [SerializeField] private GameObject winMenuObject;


    public void levelWon() {
        winMenuObject.SetActive(true);
        
    }

    public void resetLevel() {
        Debug.Log("resetting level");
        GameObject.Find("PFB_LevelWonMenu/Canvas/loading screen");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void goNextLevel() {
        Debug.Log("go to next level");
    }
}
