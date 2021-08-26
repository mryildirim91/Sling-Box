using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "Box/New Box Piece")]
public class BoxPieceData : ScriptableObject
{
    [SerializeField] private int _boxType;
    [SerializeField] private int _min, _max;
    [SerializeField] private Sprite _sprite;
    
    public int Min => _min;
    public int Max => _max;
    public int BoxType => _boxType;
    public Sprite Sprite => _sprite;
}
