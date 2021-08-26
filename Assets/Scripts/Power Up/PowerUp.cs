using MyUtils;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject.CompareTag("PowerUp") && other.gameObject.CompareTag("Launched Box"))
        {
            BoxPiece boxPiece = other.gameObject.GetComponent<BoxPiece>();
            
            if (gameObject.name == "2x")
            {
                boxPiece.UpdateBoxType(boxPiece.BoxPieceDatas[boxPiece.DataIndex + 1]);
            }
            else if (gameObject.name == "4x")
            {
                boxPiece.UpdateBoxType(boxPiece.BoxPieceDatas[boxPiece.DataIndex + 2]);
            }
            else
            {
                EventManager.TriggerRemoveBoxFromTower(other.transform);
                ObjectPool.Instance.ReturnGameObject(other.gameObject);
            }
        }

        if (gameObject.CompareTag("PowerUp"))
        {
            PowerUpManager.Instance.ActivatePowerUp();
            ObjectPool.Instance.ReturnGameObject(gameObject);
        }
    }

    private void Update()
    {
        if (transform.position.y <= -15 && gameObject.CompareTag("PowerUp"))
        {
            PowerUpManager.Instance.ActivatePowerUp();
            ObjectPool.Instance.ReturnGameObject(gameObject);
        }
    }
}
