using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadMgr : MonoBehaviour
{
    private bool IsLoadFromServer = false;

    private void Awake()
    {
        DataLoad data = DataLoad.LoadLocal();
        IsLoadFromServer = data.IsSaveServer;
    }

    public void SaveLocalData()
    {
        if (IsLoadFromServer)
        {

        }
        else
        {

        }
    }

    public void LoadLocalData()
    {
        if (IsLoadFromServer)
        {

        }
        else
        {

        }
    }
}

public class DataLoad
{
    public bool IsSaveServer = false;
    private static string KeySaveServer = "is_save_server";

    public void SaveLocal(DataLoad data)
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