using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData<T> where T : MonoBehaviour
{
    private List<T> NowObjects = new List<T>();
    private Queue<T> QueueValues = new Queue<T>();
    private T Res;
    private string ResPath;

    public PoolData(string path)
    {
        ResPath = path;
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
            if (Res == null)
            {
                Res = Resources.Load<T>(ResPath);
            }
            return Object.Instantiate<T>(Res);
        }
    }

    public void Add(T obj)
    {
        NowObjects.Add(obj);
    }
}
