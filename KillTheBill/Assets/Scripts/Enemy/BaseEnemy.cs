using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] public float _health = 1.0f;
    [SerializeField] public float _weight = 0.5f;
    private Rigidbody _rigidBody;

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
        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        // Only collide with "Player" (6), "Bullet" (7), "Wall" (9)
        int layerMask = (1 << 6) | (1 << 7) | (1 << 9);

        if ((layerMask & (1 << other.gameObject.layer)) != 0)
        {
            float objectWeight = other.gameObject.GetComponent<Rigidbody>().mass;
            if (objectWeight > _weight)
            {
                _health -= objectWeight;
            }
        }
    }
}