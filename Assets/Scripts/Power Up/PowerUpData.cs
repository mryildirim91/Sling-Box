using UnityEngine;

[CreateAssetMenu(fileName = "Power Up", menuName = "Power Up/New Power Up")]
public class PowerUpData : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;

    public Sprite Sprite => _sprite;
    public string Name => _name;
}
