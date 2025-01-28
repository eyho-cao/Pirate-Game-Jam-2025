using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosiveAmmo : BaseAmmo 
{
    [SerializeField]
    private float _timeUntilExplosion = 5.0f;
    [SerializeField]
    private float _explosionRadius = 50.0f;
    [SerializeField]
    private float _explosionForce = 500.0f;
    [SerializeField]
    private GameObject _countdownTextObject;
    private bool _isReadyToExplode = false;
    private bool _isWeaponOnFlight = false;
    private float _currentTime = 0.0f;
    private PlayerStateEngine _playerStateEngine;
    private TMP_Text _countdownText;

    protected override void Start ()
    {
        base.Start();
        
        _playerStateEngine = PlayerStateEngine.Instance;
        if (_playerStateEngine != null)
        {
            _playerStateEngine.OnWeaponFired += HandleWeaponFired;
        }
        else
        {
            Debug.LogWarning("No PlayerStateEngine is available");
        }
        Debug.Log("Starting");
        _currentTime = _timeUntilExplosion;
        _countdownTextObject.SetActive(true);
        _countdownText = _countdownTextObject.GetComponent<TMP_Text>();
        UpdateText(((int)_currentTime).ToString());
    }

    void Update()
    {
        if (_currentTime <= 0 && !_isReadyToExplode)
        {
            _isReadyToExplode = true;
            Explode();
            return;
        }

        _currentTime -= Time.deltaTime;
        UpdateText(((int)_currentTime).ToString());
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        int layerMask = (1 << 8) | (1 << 9);

        foreach(var coll in colliders)
        {
            if (coll != null)
            {
                if ((layerMask & (1 << coll.gameObject.layer)) != 0)
                {
                    Rigidbody rb = coll.GetComponentInParent<Rigidbody>();
                    rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
                }
            }
        }

        _countdownTextObject.SetActive(false);
        
        // Do this in the case of the ammo is not fired and it explodes to move on to next ammo
        if (!_isWeaponOnFlight)
        {
            _playerStateEngine.OnFinishedFiring();
        }

        Destroy(this.gameObject);
    }

    private void UpdateText(string text)
    {
        _countdownText.text = text;
    }
    
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

    private void OnDestroy()
    {
        _playerStateEngine.OnWeaponFired -= HandleWeaponFired;
    }

    private void HandleWeaponFired(GameObject currentAmmo)
    {
        if (gameObject == currentAmmo.gameObject)
        {
            _isWeaponOnFlight = true;
        }
    }

}