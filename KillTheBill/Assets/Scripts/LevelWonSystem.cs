using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWonSystem : MonoBehaviour
{

    [SerializeField] private GameObject winMenuObject;
    [SerializeField] private GameObject _starContainer;


    public void levelWon(int stars) {
        for(int i = 0; i < stars; i++) {
            GameObject star = _starContainer.transform.GetChild(i).gameObject;
            star.SetActive(true);
        }
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
        MenuInteractions.goNextLevel();
    }


}
