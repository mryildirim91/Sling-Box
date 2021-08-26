using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Data", menuName = "Camera/New Shake Data")]
public class CameraShakeData : ScriptableObject
{
    [SerializeField] private int _vibrato;
    [SerializeField] private float _duration, _randomness;
    [SerializeField] private Vector3 _strength;

    public int Vibrato => _vibrato;
    public float Duration => _duration;
    public float Randomness => _randomness;
    public Vector3 Strength => _strength;
}
