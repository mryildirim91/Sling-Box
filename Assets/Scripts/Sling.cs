using UnityEngine;
using UnityEngine.UI;

public class Sling : MonoBehaviour
{
    private bool _collided;
    private Vector3 _initialSlingHandlePos;
    [SerializeField] private Transform _rightRubber, _leftRubber, _slingHandle;
    [SerializeField] private LineRenderer _rightLR, _leftLR;
    [SerializeField] private Image _tickImage;
    [SerializeField] private SpriteRenderer _rightSprite, _leftSprite, _handleSprite;
    [SerializeField] private SlingData[] _slingDatas;
    [SerializeField] private RectTransform[] _rectTransforms;

    private void Awake()
    {
        SetSlingType(PlayerPrefs.GetInt("SlingType"));
        
        _initialSlingHandlePos = _slingHandle.position;
        _rightLR.SetPosition(0,_rightRubber.position);
        _leftLR.SetPosition(0,_leftRubber.position);
    }

    private void OnEnable()
    {
        EventManager.ONBoxReachToSling += UpdateRubberPosition;
        EventManager.OnBoxLeaveSling += UpdateRubberPosition;
    }

    private void OnDisable()
    {
        EventManager.ONBoxReachToSling -= UpdateRubberPosition;
        EventManager.OnBoxLeaveSling -= UpdateRubberPosition;
    }

    private void Update()
    {
        _rightLR.SetPosition(1, _slingHandle.position);
        _leftLR.SetPosition(1, _slingHandle.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !_collided)
        {
            _collided = true;
            EventManager.TriggerGameOver();
            GameManager.Instance.GameOver();
        }
    }

    private void UpdateRubberPosition(Transform transform)
    {
        _slingHandle.parent = transform;

        if (_slingHandle.transform.parent == null)
        {
            _slingHandle.position = _initialSlingHandlePos;
        }
    }

    public void SetSlingType(int type)
    {
        PlayerPrefs.SetInt("SlingType", type);
        int slingType = PlayerPrefs.GetInt("SlingType");
        
        _rightSprite.sprite = _slingDatas[slingType].RightsSprite;
        _leftSprite.sprite = _slingDatas[slingType].LeftSprite;
        _handleSprite.sprite = _slingDatas[slingType].HandleSprite;
        _tickImage.rectTransform.position = _rectTransforms[slingType].position;
    }
}
