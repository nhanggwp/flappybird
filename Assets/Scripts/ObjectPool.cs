using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject Prefab;
    private Stack<GameObject> _objectPool = new Stack<GameObject>();

    [SerializeField] private int _maxPoolSize = 5;

    public void InitializePool()
    {
        for (int i = 0; i < _maxPoolSize; i++)
        {
            var obj = Instantiate(Prefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            _objectPool.Push(obj);
        }
    }

    void Start()
    {
        InitializePool();
    }

    public GameObject GetObject()
    {
        if (_objectPool.Count > 0)
        {
            var obj = _objectPool.Pop();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            Debug.Log("There are no objects to get!");
            return null;
        }
    }

    public void ReturnBack(GameObject obj)
    {
        obj.SetActive(false);
        _objectPool.Push(obj);
    }
}
