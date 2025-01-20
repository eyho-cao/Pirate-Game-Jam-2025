using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    // Privates
    [SerializeField] private GameObject _clickPointObject;
    [SerializeField] private Vector3 _maxVelocity = new Vector3(1, 1, 0);
    [SerializeField] private float _velocityMultiplier = 1;
    [SerializeField] private float _indicatorDampening = 1;

    private Camera _mainCamera;
    private Rigidbody _rigidbody;
    private GameObject _clone = null;
    private Vector3 _direction;
    [SerializeField] private LineRenderer _lineRenderer;

    public PlayerStateEngine playerStateEngine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody.freezeRotation = false;
    }

    private void _OnFirstMouseDown() {
        Vector3 clickPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _clone = (GameObject) Instantiate(_clickPointObject);
        _clone.transform.position = new Vector3(clickPos.x, clickPos.y, 0);
    }

    private void _OnMouseUp() {
        this._rigidbody.AddForce(_direction * _velocityMultiplier);
        Destroy(_clone);
        _clone = null;
        _lineRenderer.SetPosition(0, this.transform.position);
        _lineRenderer.SetPosition(1, this.transform.position);
        playerStateEngine.afterWeaponFired();
    }


    private void _OnMouseDown() {
        Vector3 clickPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _direction = _clone.transform.position - new Vector3(clickPos.x, clickPos.y, 0);
        _direction = new Vector3(Mathf.Clamp(_direction.x, -_maxVelocity.x, _maxVelocity.x), Mathf.Clamp(_direction.y, -_maxVelocity.y, _maxVelocity.y), 0);
        DrawLine();
    }

    void DrawLine() {
        if(_clone == null) {
            return;
        }
        _lineRenderer.SetPosition(0, this.transform.position);
        _lineRenderer.SetPosition(1, this.transform.position + (_direction / _indicatorDampening));
    }

    // Update is called once per frame
    void Update() {  
        if (Input.GetMouseButtonDown(0)) {
            _OnFirstMouseDown();
        }
        else if (Input.GetMouseButton(0)) {
            _OnMouseDown();
        }
        else if (Input.GetMouseButtonUp(0)) {
            _OnMouseUp();
        }
    }

   



}
