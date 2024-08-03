using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData<T> where T : MonoBehaviour
{
    private List<T> NowObjects = new List<T>();
    private Queue<T> QueueValues = new Queue<T>();
    private T Res;

    public PoolData(string path)
    {
        Res = Resources.Load<T>(path);
    }

    public List<T> GetNow()
    {
        return NowObjects;
    }

    public T GetNew()
    {
        if (QueueValues.Count > 0)
        {
            return QueueValues.Dequeue();
        }
        else
        {
            return Object.Instantiate<T>(Res);
        }
    }

    public void Add(T obj)
    {
        NowObjects.Add(obj);
    }
}
