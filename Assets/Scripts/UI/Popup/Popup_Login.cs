using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Popup_Login : MonoBehaviour
{
    public TMP_InputField InputId;
    public TextMeshProUGUI PlaceHolderId;
    public TMP_InputField InputPass;
    public TextMeshProUGUI PlaceHolderPassword;

    public Button ButtonLogin;
    public TextMeshProUGUI TextLogin;
    public Button ButtonCreate;
    public TextMeshProUGUI TextCreate;

    private void Awake()
    {
        SetButtonCallback();
        SetText();
    }

    private void SetButtonCallback()
    {
        ButtonLogin.onClick.RemoveAllListeners();
        ButtonLogin.onClick.AddListener(() =>
        {
            string id = InputId.text;
            string password = InputPass.text;

            FireAuth.Instance.TryLogin(id, password);
        });

        ButtonCreate.onClick.RemoveAllListeners();
        ButtonCreate.onClick.AddListener(() =>
        {
            string id = InputId.text;
            string password = InputPass.text;

            FireAuth.Instance.CreateNewEmail(id, password);
        });
    }

    private void SetText()
    {
        PlaceHolderId.text = TransMgr.GetText("아이디를 입력해주세요.");
        PlaceHolderPassword.text = TransMgr.GetText("비밀번호를 입력해주세요.");
        TextLogin.text = TransMgr.GetText("로그인");
        TextCreate.text = TransMgr.GetText("회원 가입");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var current = EventSystem.current.currentSelectedGameObject;
            if (current == null)
                return;

            if (InputId.gameObject == current)
            {
                InputPass.Select();
                InputPass.ActivateInputField();
            }
            else
            {
                InputId.Select();
                InputPass.ActivateInputField();
            }
        }
    }
}
