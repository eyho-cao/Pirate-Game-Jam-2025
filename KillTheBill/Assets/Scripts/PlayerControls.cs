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
    private GameObject _clickPointObjClone = null;
    private Vector3 _direction;
    private bool _isReadyToLaunch = false;
    [SerializeField] private LineRenderer _lineRenderer;

    private PlayerStateEngine _playerStateEngine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody.freezeRotation = false;
        _playerStateEngine = PlayerStateEngine.Instance;
    }

    private void _OnFirstMouseDown() {
        Vector3 clickPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _clickPointObjClone = (GameObject) Instantiate(_clickPointObject);
        _clickPointObjClone.transform.position = new Vector3(clickPos.x, clickPos.y, 0);
        _isReadyToLaunch = true;
    }

    private void _OnMouseHeld() {
        Vector3 clickPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _direction = _clickPointObjClone.transform.position - new Vector3(clickPos.x, clickPos.y, 0);
        _direction = new Vector3(Mathf.Clamp(_direction.x, -_maxVelocity.x, _maxVelocity.x), Mathf.Clamp(_direction.y, -_maxVelocity.y, _maxVelocity.y), 0);
        DrawLine();
    }

    private void _OnMouseUp() {
        _isReadyToLaunch = false;
        Destroy(_clickPointObjClone);
        _clickPointObjClone = null;

        if (_direction == Vector3.zero)
        {
            return;
        }
        this._rigidbody.AddForce(_direction * _velocityMultiplier);
        _lineRenderer.SetPosition(0, this.transform.position);
        _lineRenderer.SetPosition(1, this.transform.position);
        _playerStateEngine.afterWeaponFired();
    }


    void DrawLine() {
        if(_clickPointObjClone == null) {
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

        if (_isReadyToLaunch)
        {
            if (Input.GetMouseButton(0)) {
                _OnMouseHeld();
            }
            else if (Input.GetMouseButtonUp(0)) {
                _OnMouseUp();
            }
        }
    }
}
