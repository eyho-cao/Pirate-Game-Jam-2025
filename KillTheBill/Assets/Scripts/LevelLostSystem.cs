using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLostSystem : MonoBehaviour {
    [SerializeField] private GameObject defeatObject;


    public void levelLost() {
        defeatObject.SetActive(true);
        
    }

    public void resetLevel() {
        Debug.Log("resetting level");
        GameObject.Find("PFB_LevelLostMenu/Canvas/loading screen");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}