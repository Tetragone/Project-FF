using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    public Button ButtonGameStart;

    private void Awake()
    {
        InitButtonCallback();
    }

    private void InitButtonCallback() 
    {
        ButtonGameStart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }
}
