using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    [SerializeField] private int _numLevels = 2;
    [SerializeField] private GameObject _levelSelectMenu;
    [SerializeField] private int _verticalGap = 250;
    [SerializeField] private int _horizontalGap = 250;
    [SerializeField] private Vector2 _startPos = new Vector2(0, 0);
    [SerializeField] private int _numPerRow = 6;
    [SerializeField] private GameObject _selectorObject;

    void Start(){
        for(int i = 0; i < _numLevels; i++){
            GameObject clone = (GameObject) GameObject.Instantiate(_selectorObject);
            clone.transform.SetParent(_levelSelectMenu.transform);
            int rowNum =  (i - (i % _numPerRow)) / _numPerRow;
            clone.transform.position = new Vector3(_startPos.x + (i % _numPerRow *_horizontalGap), _startPos.y - (rowNum * _verticalGap), 0);
        }
    }
}
