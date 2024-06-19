using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public void Push()
    {
        ObjectPoolMgr.Instance.Push(gameObject.name, this);
    }
}
