using UnityEngine;

[CreateAssetMenu(fileName = "Sling", menuName = "Sling/New Sling")]
public class SlingData : ScriptableObject
{
    [SerializeField] private Sprite _rightSprite, _leftSprite, _handleSprite;

    public Sprite RightsSprite => _rightSprite;
    public Sprite LeftSprite => _leftSprite;
    public Sprite HandleSprite => _handleSprite;
}
