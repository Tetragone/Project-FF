using Firebase.Auth;
using System;
using UnityEngine;

public class FireAuth : SingletonAllSecen<FireAuth>
{
    public bool IsLoginUser = false;
    private FirebaseAuth Auth = null;
    private FirebaseUser User = null;

    protected override void SetDataInAwake()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        Auth = FirebaseAuth.DefaultInstance;
        Auth.StateChanged += AuthStateChanged;
        IsLoginUser = Auth.CurrentUser != null;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (Auth.CurrentUser != User)
        {
            User = Auth.CurrentUser;
        }
    }

    void OnDestroy()
    {
        Auth.StateChanged -= AuthStateChanged;
        Auth = null;
    }

    // 옵져버 생성해서 해결하도록 하자.
    public void CreateNewEmail(string email, string password)
    {
        // 무조건 게임 씬에서 로그인이 될것이기 때문에 이렇게
        PopupMgr.ActiveLoadingPopup(true);
        Auth = FirebaseAuth.DefaultInstance;
        Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("Create Email was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Create Email encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            PopupMgr.ActiveLoadingPopup(false);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    // 옵져버 생성해서 해결하도록 하자.
    public void TryLogin(string email, string password)
    {
        // 무조건 게임 씬에서 로그인이 될것이기 때문에 이렇게
        PopupMgr.ActiveLoadingPopup(true);
        Auth = FirebaseAuth.DefaultInstance;
        Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("Sign in Email was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Sign in Email encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            PopupMgr.ActiveLoadingPopup(false);
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }
}
