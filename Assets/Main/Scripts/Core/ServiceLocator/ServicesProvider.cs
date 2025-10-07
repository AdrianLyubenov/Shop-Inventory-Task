using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ServicesProvider : MonoBehaviour
{
    protected Dictionary<Type, object> _services;

    protected List<Type> _servicesToOverride = null;
    protected List<Type> _servicesToRemoveOnSceneChange = null;

    private void Awake()
    {
        if (ServiceLocator.Instance == null)
        {
            GameObject go = new(nameof(ServiceLocator));
            ServiceLocator newServiceLocator = go.AddComponent<ServiceLocator>();
            newServiceLocator.Initialize();
        }

        PrepareServicesToAdd();

        foreach (Type t in _services.Keys)
        {
            if (!ServiceLocator.Instance.Contains(t)) continue;

            // Destroying any services that already exist
            MonoBehaviour mono = Convert.ChangeType(_services[t], t) as MonoBehaviour;
            if (mono != null) Destroy(mono.gameObject);
            _services.Remove(t);
        }

        ServiceLocator.Instance.Add(_services, _servicesToOverride);
    }

    private void Start()
    {
        ServiceLocator.InitializeServices(_services);
    }

    private void OnDestroy()
    {
        PrepareServicesToRemoveOnSceneChange();

        if (_servicesToRemoveOnSceneChange == null) return;

        ServiceLocator.Instance.Remove(_servicesToRemoveOnSceneChange);
        // foreach (Type t in _servicesToRemoveOnSceneChange) 
        // ServiceLocator.Instance.Remove(_servicesToRemoveOnSceneChange);
    }

    protected virtual void PrepareServicesToAdd()
    {
        // _services = new Dictionary<Type, object>();

        /*
            Examples:
            monoDependencies.Add(typeof(GameManager), gameManager);
            container.Add(new BonusesData());
        */

        foreach (KeyValuePair<Type, object> monoDependency in _services
                     .Where(monoDependency => monoDependency.Value == null))
            Debug.LogError($"Dependency is missing: {monoDependency.Key.Name}", this);
    }

    protected virtual void PrepareServicesToRemoveOnSceneChange()
    {
        _servicesToRemoveOnSceneChange = new List<Type>();

        /*
            Examples:
            monoDependencies.Add(typeof(GameManager), gameManager);
            container.Add(new BonusesData());
        */

        foreach (Type _ in _servicesToRemoveOnSceneChange.Where(service => service == null))
        {
            Debug.LogError("Dependency is missing.", this);
        }
    }
}