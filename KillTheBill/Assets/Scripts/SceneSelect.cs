using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    [SerializeField] private int _numLevels = 2;
    [SerializeField] private GameObject _levelSelectMenu;
    [SerializeField] private int _verticalGap = 250;
    [SerializeField] private int _horizontalGap = 250;
    [SerializeField] private int _numPerRow = 6;
    [SerializeField] private GameObject _selectorObject;

    void Start(){
        for(int i = 0; i < _numLevels; i++){

        }
    }
}
