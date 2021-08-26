using System;
using System.Collections;
using MyUtils;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject _bloodEffect;

    private float _animTime;
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _animTime = _animator.GetCurrentAnimatorStateInfo(0).length;

        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EventManager.TriggerExplode();
            
            if(other.name != "Boss")
                EventManager.TriggerEnemyGone(-1);
            
            else if (other.name == "Boss")
            {
                EventManager.TriggerLevelComplete();
                GameManager.Instance.LevelComplete();
            }
            
            GameObject effect = Instantiate(_bloodEffect);
            effect.transform.position = other.transform.position;
            ObjectPool.Instance.ReturnGameObject(other.gameObject);
        }
    }

    private IEnumerator Destroy()
    {
        yield return BetterWaitForSeconds.Wait(_animTime);
        ObjectPool.Instance.ReturnGameObject(gameObject);
    }
}
