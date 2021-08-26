using UnityEngine;

[CreateAssetMenu(fileName = "Engine", menuName = "Engine/New Engine")]
public class EngineData : ScriptableObject
{
    [SerializeField] private Sprite _sprite;

    public Sprite Sprite => _sprite;
}