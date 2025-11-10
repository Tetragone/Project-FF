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
        functions = FirebaseFunctions.GetInstance("asia-northeast3");
    }

    public void CallHttps(string name, Dictionary<string, object> data
        , UnityAction<Dictionary<object, object>> successAction, UnityAction failAction = null)
    {
        data.Add("uid", FireAuth.Instance.UID);
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
                    Debug.LogFormat("function {0} is success", name);
                    var data = task.Result.Data;
                    var result = (Dictionary<object, object>)data;
                    successAction.Invoke(result);
                } 
            });
    }
}
