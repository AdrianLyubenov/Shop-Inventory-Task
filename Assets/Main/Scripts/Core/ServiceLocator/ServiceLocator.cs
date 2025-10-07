using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance = null;

    private Kernel _container = null;
    private bool _isInit = false;
    private bool _shuttingDown = false;

    public void Initialize()
    {
        Instance = this;
        _isInit = true;
        _container = new Kernel();

        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        if (_isInit) return;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Initialize();
    }

    public void OnDestroy()
    {
        _shuttingDown = true;
        DoDispose();
        Clear();
    }

    private void DoDispose()
    {
        foreach (IIsDisposable disposable in _container.GetAll<IIsDisposable>())
            disposable.OnDispose();
    }

    public void Clear() => _container.Clear();

    public T Get<T>() where T : class => _shuttingDown ? null : _container.Get<T>();

    public bool TryGet<T>(out T value) where T : class
    {
        value = null;
        bool hasValidInstance = _container.Has<T>() && !_shuttingDown;
        if (hasValidInstance) value = _container.Get<T>();
        return hasValidInstance;
    }

    public void Add(Dictionary<Type, object> services, List<Type> overrides = null)
    {
        if (_shuttingDown) return;

        foreach (KeyValuePair<Type, object> item in services)
        {
            if (overrides != null && overrides.Contains(item.Key))
                _container.AddOrUpdate(item.Key, item.Value);
            else
                _container.Add(item.Key, item.Value);
        }
    }

    public static void InitializeServices(Dictionary<Type, object> services)
    {
        foreach (IService initializable in services.Values.OfType<IService>())
            initializable.Initialize(Instance);
    }

    public bool Contains(Type t) => !_shuttingDown && _container.Contains(t);

    public void Remove(List<Type> services)
    {
        if (_shuttingDown) return;

        foreach (Type serviceType in services)
            _container.Remove(serviceType);
    }
}

public interface IService
{
    void Initialize(ServiceLocator serviceLocator);
}

public interface IEnableable
{
    void OnEnable();
}

public interface IIsDisposable
{
    void OnDispose();
}