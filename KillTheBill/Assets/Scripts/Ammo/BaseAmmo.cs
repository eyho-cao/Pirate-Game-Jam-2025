using UnityEngine;

public class BaseAmmo : MonoBehaviour 
{
    [SerializeField]
    private float _weight = 1.0f;
    protected Rigidbody _rigidBody;

    protected virtual void Awake() 
    {
        _rigidBody = GetComponent<Rigidbody>();

        if (_weight > 0)
            _rigidBody.mass = _weight;

        // FreezeObject();

    }

    // Try not to use Start because the script is deactivated by default when starting
    void Start ()
    {
    }

    void Update()
    {

    }

    public virtual void FreezeObject()
    {
        _rigidBody.constraints = RigidbodyConstraints.FreezePosition;
        _rigidBody.useGravity = false;
    }

    public virtual void UnfreezeObject()
    {
        _rigidBody.constraints = RigidbodyConstraints.None;
        _rigidBody.useGravity = true;
    }
}