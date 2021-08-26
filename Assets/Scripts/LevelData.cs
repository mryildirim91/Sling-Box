using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/New Level")]
public class LevelData : ScriptableObject
{
    [SerializeField] private Sprite _bgImage,_hillSprite, _bridgeSprite;
    [SerializeField] private Sprite[] _flatSprites;

    public Sprite BgImage => _bgImage;
    public Sprite HillSprite => _hillSprite;
    public Sprite BridgeSprite => _bridgeSprite;
    public Sprite[] FlatSprites => _flatSprites;
}
