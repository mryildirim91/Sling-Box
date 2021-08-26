using MyUtils;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject _explosion; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            GameObject explosion = ObjectPool.Instance.GetObject(_explosion);
            explosion.transform.position = transform.position;
            ObjectPool.Instance.ReturnGameObject(gameObject);
            PowerUpManager.Instance.ActivatePowerUp();
        }
    }
}
