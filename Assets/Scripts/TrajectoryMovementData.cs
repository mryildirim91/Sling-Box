using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "Box/New Box Movement")]
public class TrajectoryMovementData : ScriptableObject
{
    [SerializeField] private float _moveSpeed, _force, _maxDistance, _delayTime;
    [SerializeField] private int _steps;
    [SerializeField] private Ease _easeType;
    public float MoveSpeed => _moveSpeed;
    public float Force => _force;
    public float MaxDistance => _maxDistance;
    public float DelayTime => _delayTime;
    public int Steps => _steps;

    public Ease EaseType => _easeType;
}
