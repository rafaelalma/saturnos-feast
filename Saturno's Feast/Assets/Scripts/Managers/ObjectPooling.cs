using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        [SerializeField] private string name;

        [SerializeField] private GameObject prefabObject;
        public GameObject PrefabObject { get { return prefabObject; } private set { prefabObject = value; } }

        [SerializeField] private int poolDepth;
        public int PoolDepth { get { return poolDepth; } private set { poolDepth = value; } }

        [SerializeField] private bool canGrow;
        public bool CanGrow { get { return canGrow; } private set { canGrow = value; } }
    }

    public class ObjectPooling : MonoBehaviour
    {
        private static ObjectPooling instance;
        public static ObjectPooling Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("The ObjectPooling is NULL.");
                }

                return instance;
            }
        }

        [SerializeField] private List<ObjectPoolItem> itemsToPool = new List<ObjectPoolItem>();

        private readonly List<GameObject> pool = new List<GameObject>();

        private void Awake()
        {
            instance = this;

            foreach (var item in itemsToPool)
            {
                for (int i = 0; i < item.PoolDepth; i++)
                {
                    GameObject pooledObject = Instantiate(item.PrefabObject, transform);
                    pooledObject.SetActive(false);
                    pool.Add(pooledObject);
                }
            }
        }

        public GameObject GetAvailableObject(string tag)
        {
            foreach (var pooledObject in pool)
            {
                if (pooledObject.activeInHierarchy == false && pooledObject.CompareTag(tag))
                {
                    return pooledObject;
                }
            }

            foreach (var item in itemsToPool)
            {
                if (item.PrefabObject.CompareTag(tag) && item.CanGrow)
                {
                    GameObject pooledObject = Instantiate(item.PrefabObject, transform);
                    pooledObject.SetActive(false);
                    pool.Add(pooledObject);

                    return pooledObject;
                }
            }
            return null;
        }
    }
}