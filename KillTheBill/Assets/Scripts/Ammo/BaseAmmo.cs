using UnityEngine;

public class BaseAmmo : MonoBehaviour 
{
    [SerializeField]
    private float _weight = 1.0f;
    protected Rigidbody _rigidBody;

    void Start ()
    {
        _rigidBody = GetComponent<Rigidbody>();

        if (_weight > 0)
            _rigidBody.mass = _weight;
    }

    void Update()
    {

    }
}