using UnityEngine;

public class LevelLostSystem : MonoBehaviour {
    [SerializeField] private GameObject defeatObject;


    public void levelLost() {
        defeatObject.SetActive(true);
        
    }

    public void resetLevel() {
        Debug.Log("Restarting level");
        MenuInteractions.restartLevel();
    }

    public void goMainMenu() {
        Debug.Log("Loading Main Menu");
        MenuInteractions.goMainMenu();
    }
}