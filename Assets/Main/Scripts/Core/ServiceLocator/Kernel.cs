using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is the container, responsible for keeping the type to instance collection, which is used by the DependenciesProvider.
/// </summary>
public class Kernel
{
    private readonly Dictionary<Type, object> _container = new();

    public void Add<T>(T obj) where T : class
    {
        if (_container.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Type {typeof(T).Name} already exists.");
            return;
        }

        _container.Add(typeof(T), obj);
    }

    public void AddOrUpdate(Type t, object obj)
    {
        if (_container.ContainsKey(t))
        {
            Debug.Log($"Type {t.Name} already exists. Replacing....");
            _container[t] = obj;
            return;
        }

        _container.Add(t, obj);
    }

    internal bool Has<T>() where T : class => _container.ContainsKey(typeof(T));

    public void Add(Type t, object obj)
    {
        if (_container.ContainsKey(t))
        {
            Debug.LogWarning($"Type {t.Name} already exists.");
            return;
        }

        _container.Add(t, obj);
    }

    public List<T> GetAll<T>() where T : class => _container.Values.OfType<T>().ToList();

    public T Get<T>() where T : class
    {
        if (_container.ContainsKey(typeof(T)))
            return _container[typeof(T)] as T;

        return null;
    }

    public bool Contains<T>() => _container.ContainsKey(typeof(T));
    public bool Contains(Type t) => _container.ContainsKey(t);

    public bool Remove<T>() => _container.Remove(typeof(T));
    public void Remove(Type t) => _container.Remove(t);

    public void Clear() => _container.Clear();
}