using System.Collections.Generic;
using UnityEngine;

namespace MyUtils
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }
        private Dictionary<string, Queue<GameObject>> _poolDict = new Dictionary<string, Queue<GameObject>>();

        private void Awake()
        {
            Instance = this;
        }

        public GameObject GetObject(GameObject clone)
        {
            if (_poolDict.TryGetValue(clone.name, out Queue<GameObject> objectList))
            {
                if (objectList.Count == 0)
                {
                    return CreateNewObject(clone);
                }
                
                GameObject obj = objectList.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            return CreateNewObject(clone);
        }

        public void ReturnGameObject(GameObject clone)
        {
            if (_poolDict.TryGetValue(clone.name, out Queue<GameObject> objectList))
            {
                objectList.Enqueue(clone);
            }
            else
            {
                Queue<GameObject> objectQueue = new Queue<GameObject>();
                objectQueue.Enqueue(clone);
                _poolDict.Add(clone.name, objectQueue);
            }
            clone.SetActive(false);
        }

        private GameObject CreateNewObject(GameObject clone)
        {
            GameObject obj = Instantiate(clone);
            return obj;
        }
    }
}
