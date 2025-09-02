using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

public static class AddressableMgr
{
    private static List<object> LoadedAssets = new List<object>();
    private static List<GameObject> InstantiatedObjects = new List<GameObject>();

    public static async Task<T> LoadAsset<T>(string key) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;
        LoadedAssets.Add(handle.Result);
        return handle.Result;
    }

    public static async Task<GameObject> LoadAndInstantiate(string key, Transform parent = null, bool isWorld = false)
    {
        var handle = Addressables.InstantiateAsync(key, parent, isWorld);
        await handle.Task;
        var go = handle.Result;
        InstantiatedObjects.Add(go);
        return go;
    }

    public static async void ReleaseAfterMS(GameObject obj, int ms = 100)
    {
        if (!InstantiatedObjects.Contains(obj))
        {
            return;
        }

        // 중복으로 호출될 수 도 있기 때문에 먼저 리스트에서 제거
        InstantiatedObjects.Remove(obj);
        await Task.Delay(ms);
        Addressables.ReleaseInstance(obj);
    }

    // 로드한 것 모두 Release
    public static void ReleaseAll()
    {
        foreach (var asset in LoadedAssets)
        {
            Addressables.Release(asset);
        }
        LoadedAssets.Clear();

        foreach (var asset in InstantiatedObjects)
        {
            Addressables.ReleaseInstance(asset);
        }
        InstantiatedObjects.Clear();
    }
}