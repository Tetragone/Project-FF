using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadMgr : MonoBehaviour
{
    public static bool IsLoaded = false;
    private static bool IsLoadFromServer = false;

    protected void Awake()
    {
        DataLoad data = DataLoad.LoadLocal();
        IsLoadFromServer = data.IsSaveServer;
        IsLoaded = false;
        StartCoroutine(StartLoad());
    }

    private IEnumerator StartLoad()
    {
        while (!IsVaildLoadStart())
        {
            yield return null;
        }

        LoadLocalData();
        IsLoaded = true;
    }

    private bool IsVaildLoadStart()
    {
        return UserDataMgr.Instance != null && UpgradeMgr.Instance != null;
    }

    public static void SaveLocalData()
    {
        if (IsLoadFromServer)
        {

        }
        else
        {
            UserDataMgr.Instance.SaveData();
            UpgradeMgr.Instance.SaveData();
        }
    }

    public void LoadLocalData()
    {
        if (IsLoadFromServer)
        {

        }
        else
        {
            UserDataMgr.Instance.LoadData();
            UpgradeMgr.Instance.LoadData();
        }
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