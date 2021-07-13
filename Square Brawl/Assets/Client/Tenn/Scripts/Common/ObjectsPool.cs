using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ObjectsPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectsPool Instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public PhotonView _pv;
    void Awake()
    {
        Instance = this;

        _pv=GetComponent<PhotonView>();

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            if (!_pv.IsMine)
            {
                return;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                //GameObject obj = Instantiate(pool.prefab, transform);
                
                GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", pool.tag), Vector3.zero, Quaternion.identity);
                objectPool.Enqueue(obj);
                
                //obj.transform.parent = transform;
                //obj.SetActive(false);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation,Transform transform)
    {
        if (!_pv.IsMine)
        {
            return null;
        }

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool with tag");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        //objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.parent = transform;

        IPoolObject pooledObj = objectToSpawn.GetComponent<IPoolObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
