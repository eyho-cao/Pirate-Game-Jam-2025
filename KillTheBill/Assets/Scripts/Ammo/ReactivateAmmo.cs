using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReactivateAmmo : BaseAmmo 
{
    [SerializeField]
    private int _amountOfReactivation = 1;
    [SerializeField]
    private int _amountOfBullets = 1;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _maxSpreadAngle = 60.0f;
    [SerializeField]
    private float _bulletSpeed = 500.0f;
    private int _currentReactivation = 0;
    private bool _isReadyToReactivate = false;
    private GameObject _nozzle;

    protected override void Awake ()
    {
        base.Awake();
        _nozzle = transform.Find("Nozzle").gameObject;
        _nozzle.transform.forward = Vector3.right;
        _currentReactivation = _amountOfReactivation;
        PlayerStateEngine instance = PlayerStateEngine.Instance;
        if (instance != null)
        {
            instance.OnWeaponFired += (GameObject currentAmmo) =>
            {
                if (gameObject == currentAmmo.gameObject)
                    _isReadyToReactivate = true;
            };
        }
        else
        {
            Debug.LogWarning("No PlayerStateEngine is available");
        }
    }

    void Update()
    {
        Debug.Log(_isReadyToReactivate);
        if (_isReadyToReactivate)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_currentReactivation > 0)
                {
                    _currentReactivation--;
                    Reactivate();
                }
                _isReadyToReactivate = false;
            }

        }
    }

    private void Reactivate()
    {
        Vector3 parentPosition = gameObject.transform.position;
        Vector3 position = new Vector3(parentPosition.x + 1, parentPosition.y, parentPosition.z);
        Quaternion rotation = Quaternion.identity;
        List<GameObject> bullets = new List<GameObject>();

        if (_amountOfBullets > 0  && _amountOfBullets == 1)
        {
            bullets.Add(Instantiate(_bulletPrefab, position, rotation));
        }
        else
        {
            float angleStep = _maxSpreadAngle / (_amountOfBullets - 1);
            float startAngle = -_maxSpreadAngle / 2;

            for (int i = 0; i < _amountOfBullets; i++)
            {
                rotation = Quaternion.Euler(0, 90, startAngle + (angleStep * i));
                // rotation = Quaternion.Euler(0, startAngle + (angleStep * i), 0);
                // rotation = Quaternion.Euler(startAngle + (angleStep * i), 0, 0);

                bullets.Add(Instantiate(_bulletPrefab, position, rotation));
            }
        }


        foreach (var b in bullets)
        {
            var rb = b.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.AddForce(_nozzle.transform.forward * _bulletSpeed, ForceMode.VelocityChange);
        }

        // Kickback
        // _rigidBody.AddForce(-transform.forward * 100.0f, ForceMode.Impulse);

        
    }

    
}