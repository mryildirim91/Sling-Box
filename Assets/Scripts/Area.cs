using System;
using System.Collections;
using MyUtils;
using UnityEngine;
public class Area : MonoBehaviour
{
    [SerializeField] private int _areaValue;
    [SerializeField] private GameObject _explosion;
    private void Awake()
    {
        StartCoroutine(ToggleCollider());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Launched Box"))
        {
            if (other.GetComponent<BoxPiece>().BoxType >= _areaValue)
            {
                GameObject explosion = ObjectPool.Instance.GetObject(_explosion);
                explosion.transform.position = other.gameObject.transform.position;
                EventManager.TriggerDestroyTower(other.transform);
                ObjectPool.Instance.ReturnGameObject(other.gameObject);
            }
        }
    }

    private IEnumerator ToggleCollider()
    {
        while (!GameManager.Instance.StopPlaying())
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Launched Box"),true);
            yield return BetterWaitForSeconds.Wait(2);
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Launched Box"),false);
            yield return BetterWaitForSeconds.Wait(1);
        }
    }
}
