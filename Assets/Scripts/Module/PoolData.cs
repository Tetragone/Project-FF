using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData<T> where T : MonoBehaviour
{
    private Transform Pool;
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
            T result = QueueValues.Dequeue();
            result.gameObject.SetActive(true);

            return result;
        }
        else
        {
            if (Res == null)
            {
                Res = Resources.Load<T>(ResPath);
            }

            if (Pool == null)
            {
                GameObject trans = new GameObject();
                trans.name = string.Format("pool parent {0}", Res.gameObject.name);
                Pool = trans.transform;
            }

            T result = Object.Instantiate<T>(Res, Pool);

            return result;
        }
    }

    public void Add(T obj)
    {
        NowObjects.Add(obj);
    }

    public void Remove(T obj)
    {
        obj.gameObject.SetActive(false);
        QueueValues.Enqueue(obj);
        NowObjects.Remove(obj);
    }
}
