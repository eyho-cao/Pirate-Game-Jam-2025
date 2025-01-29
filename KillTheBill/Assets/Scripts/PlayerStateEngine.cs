using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateEngine : MonoBehaviour
{

    [SerializeField] private Vector3 _startPos;
    [SerializeField] private List<GameObject> _weaponQueue = new List<GameObject>();
    [SerializeField] private float _spacing;
    [SerializeField] private float _firstPosOffset;

    private int _numObjOnScreen;
    private List<Vector3> _weaponPos = new List<Vector3>();
    private List<GameObject> _weaponTracker = new List<GameObject>();
    public Action<GameObject> OnWeaponFired;
    public static PlayerStateEngine Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start() {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Found another instance of PlayerStateEngine. There should only be one per level");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        determineSpaces();
        createWeaponObj();
        //what needs to be sent from the Player Controls script to tell state manager to pop first element
        // --> in weapon control script --> setup a ref to the playerstate engine and call the method to pop

        /*
        determine possible # of objs on screen, stop check after first obj that is off screen 
        --> store that #, stop doing calculations for any elements after that
        */
    }

    IEnumerator moveObject(GameObject weapon, Vector3 targetPos, bool enableControl) {
        Vector3 weaponPos = weapon.transform.position;
        float length = 0;
        while(weapon.transform.position.x != targetPos.x) {
            weapon.transform.position = Vector3.Lerp(weaponPos, targetPos, length);
            yield return new WaitForFixedUpdate();
            length += Time.fixedDeltaTime;
        }
        weapon.transform.position = targetPos;
        EnableScripts(weapon, enableControl);
        yield return null;
    }

    public void afterWeaponFired() {
        _weaponTracker[0].GetComponent<PlayerControls>().enabled = false;
        OnWeaponFired?.Invoke(_weaponTracker[0]);
        _weaponTracker.RemoveAt(0);
        _weaponQueue.RemoveAt(0);
        updateQueue();
    }

    private void updateQueue() {
        for(int i = 0; i < _weaponTracker.Count; i++) {
            StartCoroutine(moveObject(_weaponTracker[i], _weaponPos[i], i==0));
        }

        if(_weaponQueue.Count > _numObjOnScreen) {
            GameObject weaponClone = Instantiate(_weaponQueue[_numObjOnScreen], _weaponPos[_numObjOnScreen], Quaternion.identity);
            weaponClone.GetComponent<PlayerControls>()._playerStateEngine = this;
            _weaponTracker.Add(weaponClone);
        }

    }

    private void determineSpaces() {
        //determine the position of all objs from front to back, stop after first element that would be off the screen
        //start pos, start subtracting the x value and check if that world pos value is on screen
        int i = 0;
        while(true) {
            Vector3 weaponPos;
            if(i == 0) {
                weaponPos = new Vector3(_startPos.x-(_spacing*i), _startPos.y + _firstPosOffset, _startPos.z);
            }
            else {
                weaponPos = new Vector3(_startPos.x-(_spacing*i), _startPos.y, _startPos.z);
            }
            Vector3 nextPos = Camera.main.WorldToViewportPoint(weaponPos);
            _weaponPos.Add(weaponPos);
            
            if(nextPos.x < -0.01 || nextPos.x > 1 || nextPos.y < 0 || nextPos.y > 1) {
                _numObjOnScreen = i;
                break;
            }
            i++;
        }
    }

    private void createWeaponObj() {
        //spawn obj's based on values in _weaponPos
        for(int i = 0; i < _weaponPos.Count; i++) {
            GameObject weaponClone = Instantiate(_weaponQueue[i], _weaponPos[i], Quaternion.identity);
            if(i == 0) {
                EnableScripts(weaponClone, true);
            }
            _weaponTracker.Add(weaponClone);
        }
    }

    private void EnableScripts(GameObject obj, bool isEnabled)
    {
        PlayerControls weaponPlayerControls = obj.GetComponent<PlayerControls>();

        weaponPlayerControls.enabled = isEnabled;
        var baseAmmo = obj.GetComponent<BaseAmmo>();
        var reactivateAmmo = obj.GetComponent<ReactivateAmmo>();

        if (baseAmmo != null)
        {
            baseAmmo.enabled = isEnabled;
        }
        else if (reactivateAmmo != null)
        {
            reactivateAmmo.enabled = isEnabled;
        }    
    }

    public int GetNumWeaponsInQueue(){
        return _weaponQueue.Count;
    }
}


/*
pedestal is start pos 

offset for firing pos, set via editor

at round start, generate where each unit will be based on # of units, spaced evenly

whenever unit is fired, pop from list & move unit x to x-1 pos via lerp
*/
