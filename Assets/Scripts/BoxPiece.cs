using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoxPiece : MonoBehaviour
{
    private bool _nameSet;
    private int _dataIndex;
    private int _boxNumber;
    private int _boxType;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxPieceData[] _boxPieceDatas;
    [SerializeField] private PowerUpData[] _powerUpDatas;

    public int DataIndex => _dataIndex;
    public int BoxNumber => _boxNumber;
    public int BoxType => _boxType;
    public BoxPieceData[] BoxPieceDatas => _boxPieceDatas;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        transform.DOScale(Vector3.one, 0.01f);
        SetBoxType();
    }

    private void OnEnable()
    {
        _boxNumber = PlayerPrefs.GetInt("Box Number");
    }

    private void Update()
    {
        if (!_nameSet)
        {
            gameObject.name = "Box" + _boxType;
            _nameSet = true;
        }
    }

    public void SetBoxNumber(int newNumber)
    {
        _boxNumber = newNumber;
    }

    private void SetBoxType()
    {
        int randomNumber = Random.Range(1, 101);

        foreach (var data in _boxPieceDatas)
        {
            if (IsBetween(randomNumber, data.Min, data.Max))
            {
                _dataIndex = Array.IndexOf(_boxPieceDatas, data);
                _spriteRenderer.sprite = data.Sprite;
                _boxType = data.BoxType;
                gameObject.name = "Box" + data.BoxType;
            }
        }
    }

    public void UpdateBoxType(BoxPieceData newData)
    {
        _dataIndex = Array.IndexOf(_boxPieceDatas, newData);
        _spriteRenderer.sprite = newData.Sprite;
        _boxType = newData.BoxType;
        gameObject.name = "Box" + newData.BoxType;
    }

    private bool IsBetween(int randomNumber, int min, int max)
    {
        if (min >= max)
        {
            return randomNumber >= max && randomNumber < min;
        }
        
        return randomNumber >= min && randomNumber < max;
    }

    public void ActivatePowerUp(int number)
    {
        gameObject.tag = "PowerUp";
        _spriteRenderer.sprite = _powerUpDatas[number - 1].Sprite;
        gameObject.name = _powerUpDatas[number - 1].Name;
    }

}
