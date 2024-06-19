using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameRoot : MonoBehaviour
{
    public GameObject ObjLoading;

    private void Awake()
    {
        ObjLoading.SetActive(true);
    }

    private void Update()
    {
        if (GameMgr.Instance.IsLoadingObjectPool)
        {
            ObjLoading.SetActive(false);
            GameMgr.Instance.IsLoadingObjectPool = false;
        }
    }
}
