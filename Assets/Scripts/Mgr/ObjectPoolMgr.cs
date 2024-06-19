using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr : Singleton<ObjectPoolMgr>
{
    public Dictionary<string, Stack<PoolableObject>> Pools = new Dictionary<string, Stack<PoolableObject>>();
    [HideInInspector] public int MakePoolCount = 0;

    public void InitPools(string name, PoolableObject obj, int num = 100)
    {
        MakePoolCount++;
        StartCoroutine(CreatePool(name, obj, num));
    }

    private IEnumerator CreatePool(string name, PoolableObject obj, int num)
    {
        yield return null;

        int count = 0;
        Stack<PoolableObject> stackPool = new Stack<PoolableObject>();

        for (int i = 0; i < num; i++, count++)
        {
            var _obj = Instantiate<PoolableObject>(obj, transform);
            stackPool.Push(_obj);
            _obj.gameObject.SetActive(false);

            if (count >= 10)
            {
                count = 0;
                yield return null;
            }
        }

        Pools.Add(name, stackPool);
        MakePoolCount--;
        yield return null;
    }

    public PoolableObject Pop(string name)
    {
        if (Pools.ContainsKey(name))
        {
            return Pools[name].Pop();
        }
        else
        {
            Debug.LogError("You must increase pool count");
            return null;
        }
    }

    public bool Push(string name, PoolableObject obj)
    {
        if (!Pools.ContainsKey(name))
        {
            return false;
        }

        Pools[name].Push(obj);
        return true;
    }
}
