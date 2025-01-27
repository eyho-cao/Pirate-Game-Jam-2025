using System;
using System.Collections.Generic;
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
    private bool _isReadyToExplode = false;
    private bool _isWeaponOnFlight = false;
    private float _currentTime = 0.0f;
    private PlayerStateEngine _playerStateEngine;

    protected override void Awake ()
    {
        base.Awake();
        
        _playerStateEngine = PlayerStateEngine.Instance;
        if (_playerStateEngine != null)
        {
            _playerStateEngine.OnWeaponFired += HandleWeaponFired;
        }
        else
        {
            Debug.LogWarning("No PlayerStateEngine is available");
        }
    }

    void Update()
    {
        if (_currentTime >= _timeUntilExplosion && !_isReadyToExplode)
        {
            _isReadyToExplode = true;
            Explode();
            return;
        }

        _currentTime += Time.deltaTime;
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
        
        // Do this in the case of the ammo is not fired and it explodes to move on to next ammo
        if (!_isWeaponOnFlight)
        {
            Debug.LogWarning("Does this get called");
            _playerStateEngine.OnFinishedFiring();
        }

        Destroy(this.gameObject);
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
            _isWeaponOnFlight = true;
    }

}