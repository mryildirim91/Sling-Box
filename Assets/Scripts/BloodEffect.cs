using MyUtils;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(Destroy), 1.5f);
    }

    private void Destroy()
    {
        ObjectPool.Instance.ReturnGameObject(gameObject);
    }
}
