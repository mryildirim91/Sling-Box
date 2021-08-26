using System;
using System.Collections;
using UnityEngine;
using MyUtils;

public class BoxCollision : MonoBehaviour
{
    private bool _isStationary, _isMerged;
    private float _boxHeight;
    private BoxPiece _boxPiece;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _boxPiece = GetComponent<BoxPiece>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxHeight = _spriteRenderer.bounds.size.y;
    }
    private void Update()
    {
        SetBoxRotation();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Invoke(nameof(BeStationary), 0.15f);
        
        if (gameObject.CompareTag("Unlaunched Box"))
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(CareAreaCollision());
            
            if(other.gameObject.CompareTag("Ground"))
                EventManager.TriggerBoxTouchGround(transform, _boxPiece.BoxNumber);
            
            else if(other.gameObject.CompareTag("Launched Box"))
                BoxCollide(other);
            
            gameObject.tag = "Launched Box";
            gameObject.layer = LayerMask.NameToLayer("Launched Box");
        }
    }
    private void SetBoxRotation()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _boxHeight, _groundLayer);
        
        if (hit)
        {
            if (!_isStationary)
            {
                float slopeAngle = Vector2.SignedAngle(hit.normal, Vector2.up);
                transform.rotation = Quaternion.Euler(0,0,-slopeAngle);
            }
        }
    }
    
    private void BeStationary()
    {
        _isStationary = true;
    }
    
    private void BoxCollide(Collision2D other)
    {
        BoxPiece boxPiece = other.gameObject.GetComponent<BoxPiece>();
            
            if (_boxPiece.BoxType == boxPiece.BoxType)
            {
                if (_boxPiece.BoxNumber > boxPiece.BoxNumber)
                {
                    if (boxPiece.DataIndex < boxPiece.BoxPieceDatas.Length - 1)
                    {
                        transform.parent = null;
                        ObjectPool.Instance.ReturnGameObject(gameObject);
                    
                        BoxPieceData newData = boxPiece.BoxPieceDatas[_boxPiece.DataIndex + 1];
                
                        boxPiece.UpdateBoxType(newData);
                    }
                }
            }
            else
            {
                StartCoroutine(CollisionOnOff());
                EventManager.TriggerNewBoxToTower(other.transform, transform);
            }
    }
    private IEnumerator CollisionOnOff()
    {
        _collider2D.isTrigger = true;
        yield return BetterWaitForSeconds.Wait(2);
        _collider2D.isTrigger = false;
    }

    private IEnumerator CareAreaCollision()
    {
        yield return BetterWaitForSeconds.Wait(1);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Area"), false);
    }
}
