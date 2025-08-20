using UnityEngine;

public class SingletonAllSecen<T> : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<T>();
            DontDestroyOnLoad(this);
        }
    }
}
