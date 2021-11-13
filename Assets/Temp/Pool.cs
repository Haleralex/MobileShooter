using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private int _poolCount = 1;
    private Transform _holder;
    private T _objPrefab;

    private Queue<T> _pool = new Queue<T>();

    public bool IsPossibleToSpawn => _pool.Count > 0;

    public Pool(T prefab, int count, Transform holder)
    {
        _objPrefab = prefab;
        _poolCount = count;
        _holder = holder;
    }


    public void CreatePool()
    {
        if (!_objPrefab.TryGetComponent<T>(out var foo))
        {
            return;
        }

        for (int i = 0; i < _poolCount; i++)
        {
            var objGO = GameObject.Instantiate(_objPrefab);
            var obj = objGO.GetComponent<T>();
            objGO.gameObject.SetActive(false);
            objGO.transform.parent = _holder.transform;
            _pool.Enqueue(obj);
        }
    }

    public T GetObjFromPool()
    {
        var obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void ReturnObjToPool(T returned)
    {
        returned.gameObject.SetActive(false);
        _pool.Enqueue(returned);
    }
}
