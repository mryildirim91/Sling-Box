using System.Collections;
using MyUtils;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    private const float Distance = 3;
    private int _boxNumber = 1;
    private GameObject _clone;
    [SerializeField] private GameObject _box;
    [SerializeField] private Transform _spawnPos;

    private void Awake()
    {
        PlayerPrefs.DeleteKey("Box Number");
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        yield return null;
        
        while (!GameManager.Instance.StopPlaying())
        {
            yield return new WaitForSeconds(0.15f);

            if (_clone == null)
            {
                _clone = ObjectPool.Instance.GetObject(_box);
                _boxNumber = _boxNumber + 5;
                PlayerPrefs.SetInt("Box Number", _boxNumber);
                _clone.transform.position = _spawnPos.position;
            }
            
            if (Vector2.Distance(_clone.transform.position, _spawnPos.position) > Distance)
            {
                _clone = null;
            }
        }
    }
}
