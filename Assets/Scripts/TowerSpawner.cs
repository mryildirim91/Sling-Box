using System.Collections;
using System.Collections.Generic;
using MyUtils;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _boxSprite;
    private Dictionary<int, List<Transform>> _towerDict = new Dictionary<int, List<Transform>>();

    private void Start()
    {
        StartCoroutine(MergeBoxesInTower());
    }

    private void OnEnable()
    {
        EventManager.OnBoxTouchGround += BuildNewTower;
        EventManager.OnNewBoxToTower += AddNewBoxToTower;
        EventManager.OnDestroyTower += DestroyTower;
        EventManager.OnRemoveBoxFromTower += RemoveBoxFromTower;
    }

    private void OnDisable()
    {
        EventManager.OnBoxTouchGround -= BuildNewTower;
        EventManager.OnNewBoxToTower -= AddNewBoxToTower;
        EventManager.OnDestroyTower -= DestroyTower;
        EventManager.OnRemoveBoxFromTower -= RemoveBoxFromTower;
    }

    private void BuildNewTower(Transform box, int towerNumber)
    {
        GameObject newTower = new GameObject();
        newTower.transform.parent = transform;
        box.parent = newTower.transform;
        newTower.name = towerNumber.ToString();

        List<Transform> boxes = new List<Transform>();
        boxes.Add(box);

        if (!_towerDict.ContainsKey(towerNumber))
        {
            _towerDict.Add(towerNumber,boxes);
        }
    }

    private void AddNewBoxToTower(Transform otherBox, Transform newBox)
    {
        int towerNumber;
        
        if(otherBox.parent != null)
            towerNumber = int.Parse(otherBox.parent.name);
        else
            return;
        
        if (_towerDict.TryGetValue(towerNumber, out List<Transform> boxes))
        {
            int boxIndex = boxes.IndexOf(otherBox);
            
            if (boxIndex != boxes.Count - 1)
            {
                for (int i = boxes.Count - 1; i > boxIndex; i--)
                {
                    boxes[i].position += boxes[i].up * _boxSprite.bounds.size.y/1.1f;
                    boxes.Insert(i + 1 , boxes[i]);
                    boxes.RemoveAt(i);
                }
            }
            boxes.Insert(boxIndex + 1, newBox);
            newBox.parent = otherBox.parent;
  
            newBox.position = otherBox.position + otherBox.up  * _boxSprite.bounds.size.y/1.1f;
            newBox.rotation = otherBox.rotation;
            
            foreach (var t in boxes)
            {
                t.GetComponent<BoxPiece>().SetBoxNumber(boxes.IndexOf(t));
                t.GetComponentInChildren<SpriteRenderer>().sortingOrder = boxes.IndexOf(t) + 7;
            }
            
        }
    }

    private void RemoveBoxFromTower(Transform box)
    {
        int towerNumber;
        
        if(box.parent != null)
            towerNumber = int.Parse(box.parent.name);
        else
            return;
        
        if (_towerDict.TryGetValue(towerNumber, out List<Transform> boxes))
        {
            int boxIndex = boxes.IndexOf(box);
            
            if (boxIndex != boxes.Count - 1)
            {
                for (int i = boxIndex + 1; i < boxes.Count; i++)
                {
                    boxes[i].position -= boxes[i].up * _boxSprite.bounds.size.y/1.1f;
                }
            }
            boxes.RemoveAt(boxIndex);
            
            foreach (var t in boxes)
            {
                t.GetComponent<BoxPiece>().SetBoxNumber(boxes.IndexOf(t));
                t.GetComponentInChildren<SpriteRenderer>().sortingOrder = boxes.IndexOf(t) + 7;
            }
        }
    }
    
    private void DestroyTower(Transform box)
    {
        int towerNumber;
        
        if(box.parent != null)
            towerNumber = int.Parse(box.parent.name);
        else
            return;
        
        if (_towerDict.TryGetValue(towerNumber, out List<Transform> boxes))
        {
            foreach (var t in boxes)
            {
                t.parent = null;
                t.gameObject.layer = LayerMask.NameToLayer("Destroyed Box");
                t.tag = "Destroyed Box";
                Rigidbody2D rb2D = t.GetComponent<Rigidbody2D>(); 
                rb2D.constraints = RigidbodyConstraints2D.None;
                rb2D.AddForce(new Vector2(-1,1) * Random.Range(500,1000), ForceMode2D.Impulse);
                StartCoroutine(DelayDestroy(t.gameObject));
            }
            boxes.Clear();
            _towerDict.Remove(towerNumber);
        }
    }

    private IEnumerator DelayDestroy(GameObject obj)
    {
        yield return BetterWaitForSeconds.Wait(5);
        ObjectPool.Instance.ReturnGameObject(obj);
    }

    private IEnumerator MergeBoxesInTower()
    {
        while (!GameManager.Instance.StopPlaying())
        {
            yield return null;
            
            if (_towerDict.Count > 0)
            {
                foreach (var tower in _towerDict.Values)
                {
                    for (int i = 0; i < tower.Count - 1; i++)
                    {
                        if (tower[i].name == tower[i + 1].name && tower[i].CompareTag(tower[i + 1].tag))
                        {
                            yield return BetterWaitForSeconds.Wait(0.25f);
                            BoxPiece boxPiece = tower[i].GetComponent<BoxPiece>();
                            
                            if (boxPiece.DataIndex < boxPiece.BoxPieceDatas.Length - 1)
                            {
                                BoxPieceData data = boxPiece.BoxPieceDatas[boxPiece.DataIndex + 1];
                                boxPiece.UpdateBoxType(data);
                            }

                            tower[i + 1].parent = null;
                            ObjectPool.Instance.ReturnGameObject(tower[i + 1].gameObject);
                            tower.RemoveAt(i + 1);

                            for (int j = i + 1; j < tower.Count; j++)
                            {
                                tower[j].position -= tower[j].up * _boxSprite.bounds.size.y / 1.1f;
                            }

                            foreach (var t in tower)
                            {
                                t.GetComponent<BoxPiece>().SetBoxNumber(tower.IndexOf(t));
                            }

                            yield return BetterWaitForSeconds.Wait(0.25f);
                            break;
                        }
                    }
                }
            }
        }
    }
}
