using System.Collections;
using MyUtils;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool _enemiesAlive;
    private int _enemySpawnIndex;
    private int _enemyNum = 1;
    private int _numOfEnemies, _numOfEnemiesAlive;
    private float _spawnDelay = 30;
    private Vector2 _spawnPos;
    
    [SerializeField] private float _xOffset, _yOffset;
    [SerializeField] private GameObject _boss;
    [SerializeField] private GameObject[] _enemies;
    
    private void Awake()
    {
        _spawnPos = ScreenBoundaries.GetScreenBoundaries(1, _xOffset, -1, _yOffset);
        PlayerPrefs.DeleteKey("Enemy Number");
        
        PlayerPrefs.SetInt("NumberOfEnemies", 10);

        _numOfEnemies = PlayerPrefs.GetInt("NumberOfEnemies");

        InvokeRepeating(nameof(SpawnEnemies), _spawnDelay, 2);
    }

    private void OnEnable()
    {
        EventManager.OnEnemyMerge += MergeEnemy;
        EventManager.OnEnemyGone += CheckRemainingEnemies;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyMerge -= MergeEnemy; 
        EventManager.OnEnemyGone -= CheckRemainingEnemies;
    }

    private void SpawnEnemies()
    {
        if(GameManager.Instance.StopPlaying())
            return;

        if (_enemiesAlive)
            _enemiesAlive = false;
        
        if (_numOfEnemies > 0)
        {
            if (_enemySpawnIndex >= _enemies.Length - 1)
                _enemySpawnIndex = 0;
            
            int randomEnemy = Random.Range(0, 10);
            GameObject enemy;
                
            if(randomEnemy > 3)
                enemy = ObjectPool.Instance.GetObject(_enemies[_enemySpawnIndex]);
            else
                enemy = ObjectPool.Instance.GetObject(_enemies[_enemySpawnIndex + 1]);
            
            enemy.transform.position = _spawnPos;
            enemy.transform.parent = transform; 
            _numOfEnemies--;
            _numOfEnemiesAlive++;
            
            _enemyNum++;
            PlayerPrefs.SetInt("Enemy Number", _enemyNum);
        }
        else
        {
            _enemySpawnIndex++;
            CancelInvoke(nameof(SpawnEnemies));
        }
    }

    private void MergeEnemy(Transform enemyTransform, int index)
    {
        if (index < _enemies.Length - 1)
        {
            GameObject enemy = ObjectPool.Instance.GetObject(_enemies[index + 1]);
            enemy.transform.position = enemyTransform.position;
            enemy.transform.parent = transform;
        }
    }
    
    private void CheckRemainingEnemies(int number)
    {
        _numOfEnemiesAlive += number;

        if (_numOfEnemiesAlive <= 0 && !_enemiesAlive && _numOfEnemies <= 0)
        {
            _enemiesAlive = true;
            if (PlayerPrefs.GetInt("Wave") < 2)
            {
                EventManager.TriggerWaveComplete();
                GameManager.Instance.WavePassed();
                StartCoroutine(DelayEnemySpawn());
            }
            else
            {
                EventManager.TriggerSendBoss();
                StartCoroutine(DelayBossSpawn());
            }
        }
    }

    private IEnumerator DelayEnemySpawn()
    {
        yield return BetterWaitForSeconds.Wait(30);
        _numOfEnemies = PlayerPrefs.GetInt("NumberOfEnemies");
        InvokeRepeating(nameof(SpawnEnemies), 0, 2);
    }

    private void SpawnBoss()
    {
        GameObject boss = Instantiate(_boss);
        boss.transform.position = _spawnPos;
        boss.name = "Boss";
    }

    private IEnumerator DelayBossSpawn()
    {
        yield return BetterWaitForSeconds.Wait(30);
        SpawnBoss();
    }
}
