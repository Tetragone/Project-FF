using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Functions;
using Firebase.Extensions;
using UnityEngine.Events;

public class FireCloudFunction : SingletonAllSecen<FireCloudFunction>
{
    FirebaseFunctions functions;

    private void Start()
    {
        functions = FirebaseFunctions.DefaultInstance;
    }

    public void CallHttps(string name, Dictionary<string, object> data
        , UnityAction<Dictionary<string, object>> successAction, UnityAction failAction = null)
    {
        functions.GetHttpsCallable(name).CallAsync(data)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    foreach (var e in task.Exception.Flatten().InnerExceptions)
                    {
                        if (e is FunctionsException fe)
                        {
                            Debug.LogError($"Firebase 함수 에러: {fe.ErrorCode} - {fe.Message}");
                        }
                    }

                    if (failAction != null)
                    {
                        failAction.Invoke();
                    }
                }
                else
                {
                    var result = task.Result.Data as Dictionary<string, object>;
                    successAction.Invoke(result);
                } 
            });
    }
}
