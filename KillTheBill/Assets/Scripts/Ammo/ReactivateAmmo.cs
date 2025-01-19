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
    private int _currentReactivation = 0;
    private bool _isReadyToReactivate = false;

    void Start ()
    {
        _currentReactivation = _amountOfReactivation;
        PlayerStateEngine instance = PlayerStateEngine.Instance;
        if (instance != null)
        {
            instance.OnWeaponFired += () => _isReadyToReactivate = true;
        }
        else
        {
            Debug.LogWarning("No PlayerStateEngine is available");
        }
    }

    void Update()
    {
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
        Debug.LogWarning("Reactivate");
        Vector3 parentPosition = gameObject.transform.position;
        Vector3 position = new Vector3(parentPosition.x + 50, parentPosition.y, parentPosition.z);
        Quaternion rotation = Quaternion.identity;
        List<GameObject> bullets = new List<GameObject>();

        if (_amountOfBullets > 0  && _amountOfBullets == 1)
        {
            Instantiate(_bulletPrefab, position, rotation);
            return;
        }

        float angleStep = _maxSpreadAngle / (_amountOfBullets - 1);
        float startAngle = -_maxSpreadAngle / 2;

        for (int i = 0; i < _amountOfBullets; i++)
        {
            rotation = Quaternion.Euler(0, 0, startAngle + (angleStep * i));
            bullets.Add(Instantiate(_bulletPrefab, position, rotation));
        }

        foreach (var b in bullets)
        {
            var rb = b.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(1000.0f, 0, 0));
        }
        
    }

    
}