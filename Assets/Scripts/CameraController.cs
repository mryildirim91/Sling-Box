using System;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private bool _boxTouched;
    private Vector2 _startPos, _endPos, _initialPos;
    [SerializeField] private CameraShakeData _shakeData;

    private void Awake()
    {
        _initialPos = transform.position;
    }

    private void OnEnable()
    {
        EventManager.OnDestroyTower += Shake;
    }

    private void OnDisable()
    {
        EventManager.OnDestroyTower -= Shake;
    }
    
    private void Shake(Transform none)
    {
        transform.DOShakePosition(_shakeData.Duration, _shakeData.Strength, _shakeData.Vibrato, _shakeData.Randomness);
    }
}
