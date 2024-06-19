using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameServices
{
    public class CoroutineService : IDisposable
    {
        private Dictionary<object, Dictionary<string, Coroutine>> _activeUniqueCoroutines = new Dictionary<object, Dictionary<string, Coroutine>>();

        private Dictionary<object, Dictionary<Guid, Coroutine>> _activeCoroutines = new Dictionary<object, Dictionary<Guid, Coroutine>>();

        private MonoBehaviour _monoBehaviour;

        public CoroutineService(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }

        /// <summary>
        /// Starts a coroutine
        /// </summary>
        /// <param name="owner">The class owner that started this coroutine</param>
        /// <param name="coroutine">The IEnumerator you want to evaluate</param>
        /// <param name="timeout">Optional, defines timeout to stop the coroutine after X seconds</param>
        /// <returns>The a unique Guid to identify the coroutine. Can be used to stop it</returns>
        public Guid StartCoroutine(object owner, IEnumerator coroutine, float timeout = 0f)
        {
            if(!_activeCoroutines.ContainsKey(owner))
            {
                _activeCoroutines.Add(owner, new Dictionary<Guid, Coroutine>());
            }

            var id = Guid.NewGuid();

            if(timeout > 0)
            {
                StartCoroutine(TryToStopCoroutineAfterTimeout(owner, timeout, id));
            }

            var notifyCoroutineFinishedMethod = NotifyCoroutineFinished(owner, coroutine, id);
            var coroutineWithFinishNotifier = StartCoroutine(notifyCoroutineFinishedMethod);
            _activeCoroutines[owner].Add(id, coroutineWithFinishNotifier);
            return id;
        }

        /// <summary>
        /// Starts a unique coroutine. If you try to start the same coroutine again while it's still active an error
        /// will be logged and the second coroutine won't start
        /// </summary>
        /// <param name="owner">The class owner that started this coroutine</param>
        /// <param name="coroutine">The IEnumerator you want to evaluate</param>
        /// <param name="name">The name of the coroutine method, this is needed to store it and make sure
        /// the same coroutine isn't triggered again. It's also used to stop it. Use nameof(NameOfYourIEnumerator)</param>
        /// <param name="timeout">Optional, defines timeout to stop the coroutine after X seconds</param>
        public void StartUniqueCoroutine(object owner, IEnumerator coroutine, string name, float timeout = 0f)
        {
            if(_activeUniqueCoroutines.TryGetValue(owner, out var coroutines))
            {
                if(coroutines.ContainsKey(name))
                {
                    Debug.LogError($"[CoroutineService] {coroutine} is already active with owner {owner}");
                    return;
                }
            }
            else
            {
                _activeUniqueCoroutines[owner] = new Dictionary<string, Coroutine>();
            }

            var notifyUniqueCoroutineFinishedMethod = NotifyUniqueCoroutineFinished(owner, coroutine, name);
            var coroutineWithFinishNotifier = StartCoroutine(notifyUniqueCoroutineFinishedMethod);
            _activeUniqueCoroutines[owner].Add(name, coroutineWithFinishNotifier);

            if(timeout > 0)
            {
                StartCoroutine(TryToStopUniqueCoroutineAfterTimeout(owner, timeout, name));
            }
        }

        public bool IsCoroutineActive(object owner, string name)
        {
            if(_activeUniqueCoroutines.TryGetValue(owner, out var coroutines))
            {
                return coroutines.ContainsKey(name);
            }
            return false;
        }

        private Coroutine StartCoroutine(IEnumerator routine)
        {
            if(_monoBehaviour != null && _monoBehaviour.isActiveAndEnabled)
            {
                return _monoBehaviour.StartCoroutine(routine);
            }

            Debug.LogError($"[CoroutineService] MonoBehaviour not ready! {_monoBehaviour}");
            return null;
        }

        private IEnumerator NotifyUniqueCoroutineFinished(object owner, IEnumerator originalCoroutine, string name)
        {
            yield return originalCoroutine;
            _activeUniqueCoroutines[owner].Remove(name);
        }

        private IEnumerator NotifyCoroutineFinished(object owner, IEnumerator originalCoroutine, Guid coroutineId)
        {
            yield return originalCoroutine;
            _activeCoroutines[owner].Remove(coroutineId);
        }

        private IEnumerator TryToStopCoroutineAfterTimeout(object owner, float seconds, Guid coroutineId)
        {
            yield return new WaitForSeconds(seconds);
            if(!_activeCoroutines.TryGetValue(owner, out var coroutines))
            {
                yield break;
            }
            if(coroutines.ContainsKey(coroutineId))
            {
                StopCoroutine(owner, coroutineId);
            }
        }

        private IEnumerator TryToStopUniqueCoroutineAfterTimeout(object owner, float seconds, string name)
        {
            yield return new WaitForSeconds(seconds);
            if(!_activeUniqueCoroutines.TryGetValue(owner, out var coroutines))
            {
                yield break;
            }
            if(coroutines.ContainsKey(name))
            {
                StopUniqueCoroutine(owner, name);
            }
        }

        /// <summary>
        /// Stops a coroutine. If the coroutine is no longer active or doesn't exist does nothing
        /// </summary>
        /// <param name="owner">The class owner that started this coroutine</param>
        /// <param name="coroutineId">The Guid provided when the coroutine was created</param>
        public void StopCoroutine(object owner, Guid coroutineId)
        {
            if(_activeCoroutines.ContainsKey(owner) &&
               _activeCoroutines[owner].TryGetValue(coroutineId, out var coroutine))
            {
                StopCoroutine(coroutine);
                _activeCoroutines[owner].Remove(coroutineId);
            }
        }

        /// <summary>
        /// Stops a unique coroutine. If the coroutine is no longer active or doesn't exist does nothing
        /// </summary>
        /// <param name="owner">The class owner that started this coroutine</param>
        /// <param name="name">The name of the coroutine you want to stop. Use nameof(NameOfYourIEnumerator)</param>
        public void StopUniqueCoroutine(object owner, string name)
        {
            if(_activeUniqueCoroutines.ContainsKey(owner) && _activeUniqueCoroutines.TryGetValue(owner, out var coroutines))
            {
                if(coroutines.TryGetValue(name, out var coroutineToStop))
                {
                    StopCoroutine(coroutineToStop);
                    _activeUniqueCoroutines[owner].Remove(name);
                }
            }
        }

        private void StopCoroutine(Coroutine coroutine)
        {
            if(_monoBehaviour != null && _monoBehaviour.isActiveAndEnabled)
            {
                _monoBehaviour.StopCoroutine(coroutine);
            }
            else
            {
                Debug.LogError($"[CoroutineService] MonoBehaviour not ready!");
            }
        }

        private void StopAllNonUniqueCoroutines(object owner)
        {
            if(_activeCoroutines.TryGetValue(owner, out var coroutines))
            {
                foreach(var coroutine in coroutines.Values)
                {
                    StopCoroutine(coroutine);
                }

                _activeCoroutines[owner].Clear();
            }
        }

        private void StopAllUniqueCoroutinesFromOneOwner(object owner)
        {
            if(_activeUniqueCoroutines.TryGetValue(owner, out var coroutines))
            {
                foreach(var coroutine in coroutines)
                {
                    StopCoroutine(coroutine.Value);
                }

                _activeUniqueCoroutines[owner].Clear();
            }
        }

        public void StopAllCoroutinesFromOneOwner(object owner)
        {
            StopAllUniqueCoroutinesFromOneOwner(owner);
            StopAllNonUniqueCoroutines(owner);
        }

        public void Dispose()
        {
            foreach(var owner in _activeUniqueCoroutines.Keys)
            {
                StopAllUniqueCoroutinesFromOneOwner(owner);
            }

            foreach(var owner in _activeCoroutines.Keys)
            {
                StopAllNonUniqueCoroutines(owner);
            }

            _activeUniqueCoroutines.Clear();
            _activeCoroutines.Clear();
        }
    }
}
