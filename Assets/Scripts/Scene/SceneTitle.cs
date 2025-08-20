using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTitle : MonoBehaviour
{
    private LoadingState _state = LoadingState.start;
    private int _frame = 0;

    private void Update()
    {
        switch (_state)
        {
            case LoadingState.start:
                if (TableMgr.IsReaded)
                {
                    _state = LoadingState.loading_data;
                    _frame = -1;
                }
                break;

            case LoadingState.loading_data:
                if (DataLoadMgr.IsLoaded)
                {
                    _state = LoadingState.done;
                    _frame = -1;
                }

                break;
            case LoadingState.done:
                SceneManager.LoadScene("Game");
                break;
        }

        _frame++;
    }
}

public enum LoadingState
{
    start, loading_data, done,
}