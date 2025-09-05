using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<T>();
            SetDataInAwake();
        }
    }

    // singleton 관련 코드가 awake()에 있는데 오버라이드하다가 문제 생기는것을 방지하기 위한 코드
    protected virtual void SetDataInAwake() { }
}
