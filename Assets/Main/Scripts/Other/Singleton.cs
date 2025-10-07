using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private static bool _dontDestroyOnLoad;
    private static T _instance;
    private static bool _isQuitting;
    public static T Instance
    {
        get
        {
            if (_isQuitting) return null;
            if (_instance == null) CreateAndAssign();
            return _instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoCreate()
    {
        if (_instance == null && !_isQuitting) CreateAndAssign();
    }

    private static void CreateAndAssign()
    {
        var go = new GameObject(typeof(T).Name);
        _instance = go.AddComponent<T>();
        if (_dontDestroyOnLoad)
        { DontDestroyOnLoad(go); }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}