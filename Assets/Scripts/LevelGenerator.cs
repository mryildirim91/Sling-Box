using MyUtils;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _numOfPlatforms;
    [SerializeField] private Transform _engine, _sling;
    [SerializeField] private Image _bgImage;
    [SerializeField] private GameObject[] _flatGrounds;
    [SerializeField] private GameObject[] _unevenGrounds;
    [SerializeField] private LevelData[] _levelDatas;
    private void Awake()
    {
        SpriteRenderer engineSprite = _engine.GetComponent<SpriteRenderer>();
        float engineOffsetX = engineSprite.bounds.size.x;
        float engineOffsetY = engineSprite.bounds.size.y;
        _engine.localPosition = ScreenBoundaries.GetScreenBoundaries(-1, engineOffsetX / 2, -1,
            engineOffsetY / 2 + 5);
        _sling.localPosition = _engine.position + new Vector3(4,-0.4f);
        
        GenerateLevel();
    }
    
    private void GenerateLevel()
    {
        int level = PlayerPrefs.GetInt("Level") + 1;
        int levelIndex;
        
        if (level < 5)
            levelIndex = 0;
        else if (level > 5 && level < 10)
            levelIndex = 1;
        else
            levelIndex = _levelDatas.Length - 1;

        _bgImage.sprite = _levelDatas[levelIndex].BgImage;
        
        GameObject[] grounds = new GameObject[_numOfPlatforms];
        GameObject groundParent = new GameObject();
        groundParent.name = "Grounds";
        groundParent.transform.parent = transform;
        
        float xSize = _flatGrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;

        for (int i = 0; i < _numOfPlatforms; i++)
        {
            if (i < 2)
            {
                grounds[i] = Instantiate(_flatGrounds[Random.Range(0, 4)]);
            }
            else
            {
                if (i % 2 == 0)
                {
                    grounds[i] = Instantiate(_unevenGrounds[Random.Range(0, _unevenGrounds.Length)]);
                }
                else
                {
                    grounds[i] = Instantiate(_flatGrounds[Random.Range(0, _flatGrounds.Length)]);
                }
            }

            grounds[i].transform.position = GetPlatformPosition(i, xSize);

            grounds[i].transform.parent = groundParent.transform;

            SpriteRenderer renderer = grounds[i].GetComponent<SpriteRenderer>();

            if (grounds[i].name == "Bridge(Clone)")
            {
                renderer.sprite = _levelDatas[levelIndex].BridgeSprite;
            }
            else if(grounds[i].name == "Hill(Clone)")
            {
                renderer.sprite = _levelDatas[levelIndex].HillSprite;
            }
            else
            {
                renderer.sprite = _levelDatas[levelIndex].FlatSprites[Random.Range(0, _levelDatas[levelIndex].FlatSprites.Length)];
            }
        }
    }

    private Vector2 GetPlatformPosition(int x, float xSize)
    {
        Vector2 initialPos = ScreenBoundaries.GetScreenBoundaries(-1, 0, -1, 4);
        Vector2 pos = new Vector2(initialPos.x + x * xSize, initialPos.y);
        return pos;
    }
}
