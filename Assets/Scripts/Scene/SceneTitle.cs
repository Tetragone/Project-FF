using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTitle : MonoBehaviour
{
    public UI_Loading Loading;
    public Button ButtonStart;

    private float _percent = 0f;
    private LoadingState _state = LoadingState.start;
    private int _frame = 0;

    private void Awake()
    {
        ButtonStart.gameObject.SetActive(false);
        ButtonStart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Lobby");
        });
    }

    private void Update()
    {
        Loading.SetPercent(_percent);
        _percent += Time.deltaTime;
        switch (_state)
        {
            case LoadingState.start:
                if (_percent > 0.1f)
                {
                    _percent = 0.1f;
                }

                if (TableMgr.IsReaded)
                {
                    _state = LoadingState.loading_data;
                    _frame = -1;
                }
                break;

            case LoadingState.loading_data:
                if (_percent < 0.7f)
                {
                    _percent += 0.1f;
                }
                else
                {
                    _percent = 0.7f;
                }

                if (_frame == 0)
                {
                    UserData.Instance.LoadData();
                }

                if (UserData.Instance.IsLoadingAll)
                {
                    _state = LoadingState.done;
                    _frame = 0;
                }
                break;

            case LoadingState.done:
                _percent = 1f;
                ButtonStart.gameObject.SetActive(true);
                Loading.gameObject.SetActive(false);
                break;
        }

        _frame++;
    }
}

public enum LoadingState
{
    start, loading_data, done,
}