using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

enum GameState {
    GS_RUNNING,
    GS_PAUSED,
    GS_WIN,
    GS_LOSE
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private float _lastShotTimeout = 5.0f;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private int[] _starShotRequirements = new int[3];
    [SerializeField] private GameObject _starContainer;
    [SerializeField] private PauseSystem _pauseSystem;
    [SerializeField] private LevelLostSystem _levelLostSystem;
    [SerializeField] private LevelWonSystem _levelWonSystem;
     

    private GameState _currentGameState = GameState.GS_RUNNING;

    private List<GameObject> _enemies;
    private int _score = 0;

    void Start()
    {
        // Get all enemy objects in scene
        _enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (var e in _enemies)
        {
            BaseEnemy baseEnemyComponent = e.GetComponent<BaseEnemy>();
            baseEnemyComponent.OnEnemyDied += UpdateScore;
        }

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            if(_currentGameState != GameState.GS_PAUSED) {
                _currentGameState = GameState.GS_PAUSED;
                _pauseSystem.pauseGame();
            } else {
                _currentGameState = GameState.GS_RUNNING;
                _pauseSystem.unpauseGame();
            }
        }
        
    }

    void FixedUpdate()
    {   
        // Dont really need to do much here for these 2 states as the UI should reflect this
        //maybe we save the attempt in a win?
        if(_currentGameState == GameState.GS_LOSE) {
            _levelLostSystem.levelLost();
            Debug.Log("Game Lost");
        }
        if(_currentGameState == GameState.GS_WIN) {
            int stars = getNumStarsEarned();
            Debug.Log("WINNER - " + stars + " have been earned");
            _levelWonSystem.levelWon(stars);
        }
        
        // This fixed update is checking for current state of the game to see if we won or, ran out of shots and timed out
        if(_currentGameState != GameState.GS_RUNNING) {
            return;
        }
        checkForWinState();
        checkPlayerWeaponCount();
        updateUI();
        // Need some form of enemy check here
    }

    private void checkForWinState() {
        int enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemies <= 0) {
            _currentGameState = GameState.GS_WIN;
        }
    }

    private void updateUI() {
        int numWeaponsLeft = PlayerStateEngine.Instance.GetNumWeaponsInQueue();
        _countText.text = "Weapons: " + numWeaponsLeft;

        int stars = getNumStarsEarned();

        if(stars < _starShotRequirements.Length){
            GameObject star = _starContainer.transform.GetChild(stars).gameObject;
            star.SetActive(false);
        }

    }

    private IEnumerator WaitForGameCompleteState(){

        yield return new WaitForSeconds(_lastShotTimeout);

        //Check if game was won already before doing anything
        if(_currentGameState != GameState.GS_WIN){
            _currentGameState = GameState.GS_LOSE;
        }
        yield return null;        
    }
  
    private void checkPlayerWeaponCount() {
        
        if(PlayerStateEngine.Instance){
            int numWeaponsLeft = PlayerStateEngine.Instance.GetNumWeaponsInQueue();
            _countText.text = "Weapons: " + numWeaponsLeft;
            if(numWeaponsLeft <= 0) {
                StartCoroutine(WaitForGameCompleteState());
            }
        }
    }

    private int getNumStarsEarned() {
        int numWeaponsLeft = PlayerStateEngine.Instance.GetNumWeaponsInQueue();
        int i = 0;
        for (i = 0; i < _starShotRequirements.Length; i++) {
            if(numWeaponsLeft < _starShotRequirements[i]){
                return i;
            }
        }
        return i;
    }

    private void UpdateScore(int score)
    {
        if (score > 0)
            _score += score;
        
        Debug.Log($"Score: {_score}");
    }

}
