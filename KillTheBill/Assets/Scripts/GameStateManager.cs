using System.Collections;
using UnityEngine;
using TMPro;

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
    

    private PlayerStateEngine _playerState;
    private GameState _currentGameState = GameState.GS_RUNNING;


    void Start()
    {
        _playerState = (PlayerStateEngine)GameObject.FindFirstObjectByType(typeof(PlayerStateEngine));
        // Get all enemy object sin scene
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
        if(_currentGameState == GameState.GS_LOSE){
            Debug.Log("LOSER");
        }
        if(_currentGameState == GameState.GS_WIN){
            int stars = getNumStarsEarned();
            Debug.Log("WINNER - " + stars + " have been earned");
        }
        
        // This fixed update is checking for current state of the game to see if we won or, ran out of shots and timed out
        if(_currentGameState != GameState.GS_RUNNING){
            return;
        }
        if(Input.GetKeyDown(KeyCode.F)){
            _currentGameState = GameState.GS_WIN;
        }

        checkPlayerWeaponCount();
        updateUI();
        // Need some form of enemy check here
    }

    private void updateUI() {
        int numWeaponsLeft = _playerState.GetNumWeaponsInQueue();
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
        
        if(_playerState){
            int numWeaponsLeft = _playerState.GetNumWeaponsInQueue();
            _countText.text = "Weapons: " + numWeaponsLeft;
            if(numWeaponsLeft <= 0) {
                StartCoroutine(WaitForGameCompleteState());
            }
        }
    }

    private int getNumStarsEarned() {
        int numWeaponsLeft = _playerState.GetNumWeaponsInQueue();
        int i = 0;
        for (i = 0; i < _starShotRequirements.Length; i++) {
            if(numWeaponsLeft < _starShotRequirements[i]){
                return i;
            }
        }
        return i;
    }
}
