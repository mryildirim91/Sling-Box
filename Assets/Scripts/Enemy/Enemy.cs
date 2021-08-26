using System.Collections;
using MyUtils;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    private bool _towerDestroyed;
    private float _initialSpeed;
    private float _rayDistance;
    private Rigidbody2D _rb2D;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _slopeNormalPerp;
    private Animator _animator;

    [SerializeField] private float _speed;
    [SerializeField] private int _enemySpawnIndex, _enemyType;
    [SerializeField]private LayerMask _groundLayer, _boxLayer;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Walk = Animator.StringToHash("Walk");

    public int EnemyNumber { get; private set; }
    public int EnemyType => _enemyType;

    private void Awake()
    {
        _initialSpeed = _speed;
        _slopeNormalPerp = Vector2.zero;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        _rayDistance = _spriteRenderer.bounds.size.y;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Unlaunched Box"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Destroyed Box"), true);
    }

    private void Start()
    {
        EnemyNumber = PlayerPrefs.GetInt("Enemy Number");
    }
    
    private void FixedUpdate()
    {
        Move();
        DetectBox();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            
            if (EnemyNumber > enemy.EnemyNumber)
            {
                if (_enemyType == enemy.EnemyType)
                {
                    EventManager.TriggerEnemyMerge(transform, _enemySpawnIndex);
                    EventManager.TriggerEnemyGone(-1);
                    ObjectPool.Instance.ReturnGameObject(gameObject);
                    ObjectPool.Instance.ReturnGameObject(other.gameObject);
                }
            }
        }
    }

    private void Move()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(transform.position - new Vector3(_spriteRenderer.bounds.size.x / 2,0,0), Vector2.down, _rayDistance, _groundLayer);

        if (hit)
        {
            _slopeNormalPerp = Vector2.Perpendicular(hit.normal);
        }

        _rb2D.velocity = _slopeNormalPerp * (Time.fixedDeltaTime * _speed);
        transform.rotation = Quaternion.Euler(_slopeNormalPerp);
    }

    private void DetectBox()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(transform.position - new Vector3(0,_spriteRenderer.bounds.size.y / 3.65f,0), _slopeNormalPerp, _rayDistance, _boxLayer);

        if (hit)
        {
            if (Vector2.Distance(transform.position, hit.transform.position) < 2f)
            {
                _speed = 0;
                
                _animator.SetTrigger(Attack);

                if (_enemyType > hit.transform.GetComponent<BoxPiece>().BoxType && !_towerDestroyed)
                {
                    _towerDestroyed = true;
                    StartCoroutine(DelayDestroyTower(hit.transform));
                }
            }
            else if(_speed <= 0)
            {
                _towerDestroyed = false;
                Invoke(nameof(SpeedDelay), 2);
            }
        }
        else
        {
            if (_speed <= 0)
            {
                _towerDestroyed = false;
                Invoke(nameof(SpeedDelay), 2);
            }
        }
    }

    private void SpeedDelay()
    {
        _speed = _initialSpeed;
        _animator.SetTrigger(Walk);
    }

    private IEnumerator DelayDestroyTower(Transform transform)
    {
        yield return BetterWaitForSeconds.Wait(1);
        EventManager.TriggerDestroyTower(transform);
    }
}
