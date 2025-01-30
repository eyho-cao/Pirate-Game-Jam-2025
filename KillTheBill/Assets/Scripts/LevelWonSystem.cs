using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWonSystem : MonoBehaviour
{

    [SerializeField] private GameObject winMenuObject;


    public void levelWon() {
        winMenuObject.SetActive(true);
        
    }

    public void resetLevel() {
        //Need to modularize this method as it will be used in various places
        MenuInteractions.restartLevel();
    }

    public void goMainMenu() {
        MenuInteractions.goMainMenu();
    }

    public void goNextLevel() {
        Debug.Log("go to next level");
    }


}
