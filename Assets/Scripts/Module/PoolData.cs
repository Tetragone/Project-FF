using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PoolData<T> where T : MonoBehaviour
{
    private Transform Pool;
    private List<T> NowObjects = new List<T>();
    private Queue<T> QueueValues = new Queue<T>();
    private T Res;
    private string ResKey;
    private bool IsLoading = false;

    public PoolData(string key)
    {
        ResKey = key;
        LoadRes();
    }

    private void LoadRes()
    {
        IsLoading = true;
        Addressables.LoadAssetAsync<T>(ResKey).Completed += (handele) =>
        {
            Res = handele.Result;
            IsLoading = false;
        };
    }

    public List<T> GetNowList()
    {
        return NowObjects;
    }

    public async Task<T> GetNew()
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
                if (!IsLoading)
                {
                    LoadRes();
                }

                while (IsLoading)
                {
                    await Task.Delay(100);
                }
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

    public void RemoveAll()
    {
        for (int i = 0; i < NowObjects.Count; i++)
        {
            NowObjects[i].gameObject.SetActive(false);
            QueueValues.Enqueue(NowObjects[i]);
        }

        NowObjects.Clear();
    }
}
