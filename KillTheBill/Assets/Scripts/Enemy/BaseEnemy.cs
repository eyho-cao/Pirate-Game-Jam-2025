using System;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float _health = 1.0f;
    [SerializeField] private float _weight = 0.5f;
    [SerializeField] private int _score = 100;
    private Rigidbody _rigidBody;
    private bool _isDying = false;
    public Action<int> OnEnemyDied;

    public void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        if (_weight >= 0)
        {
            _rigidBody.mass = _weight;
        }
    }

    public void Update()
    {
        if (_health <= 0 && !_isDying)
        {
            HandleEnemyDead();
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        // Only collide with "Player" (6), "Bullet" (7), "Wall" (9)
        int wallLayerMask = (1 << 9);
        int collidableLayerMask = (1 << 6) | (1 << 7) | wallLayerMask;
        
        GameObject otherGameObject = other.gameObject;

        if ((collidableLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            // If the enemy is placed on a "Wall" object, it shouldn't deal damage to itself
            // Only when fallen on top of it, would it die!
            if ((wallLayerMask & (1 << other.gameObject.layer)) != 0)
            {
                if (otherGameObject.transform.position.y <= this.transform.position.y)
                {
                    return;
                }
            }

            float objectWeight = other.gameObject.GetComponent<Rigidbody>().mass;
            if (objectWeight > _weight)
            {
                _health -= objectWeight;
            }
        }
    }

    private void HandleEnemyDead()
    {
        _isDying = true;
        OnEnemyDied?.Invoke(_score);
        Destroy(this.gameObject);
    }
}