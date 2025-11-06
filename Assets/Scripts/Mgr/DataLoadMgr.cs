using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadMgr : MonoBehaviour
{
    public static bool IsLoaded = false;
    private static bool IsLoadFromServer = false;

    // Awake()에서 하면 Singleton이 활성화가 안되어 있을 수 도 있기 때문에 Start()에서 해준다.
    private void Start()
    {
        IsLoaded = false;
        StartCoroutine(StartLoad());
    }

    private IEnumerator StartLoad()
    {
        yield return GameOptionData.Instance.LoadData();
        LoadData();
    }

    public static void SaveLocalData()
    {
        if (IsLoadFromServer)
        {

        }
        else
        {
            UserDataMgr.Instance.SaveData();
            UserDataMgr.Instance.SaveData();
        }
    }

    private void LoadData()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            LoadLocalData();
        }
        else
        {
            if (FireAuth.Instance.IsLoginUser)
            {
                LoadServerData();
            }
            else
            {
                LoadLocalData();
            }
        }
    }

    public static void LoadLocalData()
    {
        UserDataMgr.Instance.LoadData();
        UserDataMgr.Instance.LoadData();
        IsLoaded = true;
    }

    public static void LoadServerData()
    {

    }
}

public class DataLoad
{
    public bool IsSaveServer = false;
    private static string KeySaveServer = "is_save_server";

    public static void SaveLocal(DataLoad data)
    {
        PlayerPrefs.SetInt(KeySaveServer, data.IsSaveServer ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static DataLoad LoadLocal()
    {
        DataLoad data = new DataLoad();
        data.IsSaveServer = PlayerPrefs.GetInt(KeySaveServer) == 1;
        return data;
    }
}