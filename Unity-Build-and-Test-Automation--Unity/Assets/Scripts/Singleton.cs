using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;
    
    private void Awake()
    {
        if (!_instance)
        {
            _instance = this as T;
            DontDestroyOnLoad(_instance);
            OnAwake(true);
        }
        else
        {
            Destroy(gameObject);
            OnAwake(false);
        }
    }
    
    protected virtual void OnAwake(bool singletonInstance) {}
}
