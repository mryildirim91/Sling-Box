using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Animator _animator;
    [SerializeField]private Image _tickImage;
    [SerializeField]private EngineData[] _datas;
    [SerializeField] private RectTransform[] _rectTransforms;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        SetEngineType(PlayerPrefs.GetInt("EngineType"));
    }
    
    public void SetEngineType(int type)
    {
        if (type == 1)
        {
            _animator.enabled = false;
        }
        else if(type == 2)
        {
            _animator.enabled = true;
            _animator.SetTrigger("Engine2");
        }
        else
        {
            _animator.enabled = true;
            _animator.SetTrigger("Engine1");
        }
        
        PlayerPrefs.SetInt("EngineType", type);
        int engineType = PlayerPrefs.GetInt("EngineType");
        _renderer.sprite = _datas[engineType].Sprite;
        _tickImage.rectTransform.position = _rectTransforms[type].position;
    }
}
