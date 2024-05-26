using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameServices
{
    internal class ServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly Dictionary<Type, List<Action<object>>> _actionsWaitingForServices = new Dictionary<Type, List<Action<object>>>();

        public void AddService<T>(T service) where T : class
        {
            _services.Add(typeof(T), service);
            TriggerActionsWaitingForService(service);
        }

        public T GetService<T>() where T : class
        {
            if (!Contains<T>())
            {
                 Debug.LogWarning($"{typeof(T)} do not exist as a service right now.");               
            }
            return (T) _services[typeof(T)];
        }
        
        public void WaitFor<T>(Action<T> onServiceAvailable) where T : class
        {
            Type type = typeof(T);
            if(_services.ContainsKey(type))
            {
                onServiceAvailable.Invoke(_services[type] as T);
            }
            else
            {
                Action<object> action = ConvertAction(onServiceAvailable);
                AddWaitingAction(type, action);
            }
        }

        public void Clear()
        {
            foreach(object service in _services.Values)
            {
                if(service is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            _services.Clear();
            _actionsWaitingForServices.Clear();
        }

        public bool Contains<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }

        private void TriggerActionsWaitingForService<T>(T service) where T : class
        {
            Type type = typeof(T);

            if(_actionsWaitingForServices.ContainsKey(type))
            {
                List<Action<object>> actions = _actionsWaitingForServices[type];

                if(actions != null)
                {
                    foreach(Action<object> action in actions)
                    {
                        Action<T> actionToTrigger = ConvertAction<T>(action);
                        actionToTrigger.Invoke(service);
                    }
                    actions.Clear();
                }
            }
        }

        private void AddWaitingAction(Type type, Action<object> action)
        {
            if(!_actionsWaitingForServices.ContainsKey(type))
            {
                _actionsWaitingForServices[type] = new List<Action<object>>();
            }
            _actionsWaitingForServices[type].Add(action);
        }

        private Action<object> ConvertAction<T>(Action<T> action) where T : class
        {
            return (o) => { action(o as T); };
        }

        private Action<T> ConvertAction<T>(Action<object> action)
        {
            return (o) => { action(o); };
        }
    }
}