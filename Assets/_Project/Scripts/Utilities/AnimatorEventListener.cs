using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Utilities
{
    public class AnimatorEventListener : MonoBehaviour
    {
        private Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();

        public UnityEvent GetEvent(string eventName)
        {
            eventName = eventName.ToLower();

            if (events.ContainsKey(eventName))
            {
                return events[eventName];
            }
            events.Add(eventName, new UnityEvent());
            return events[eventName];
        }

        public void ClearEvents()
        {
            events.Clear();
        }

        public void TriggerEvent(string eventName)
        {
            GetEvent(eventName)?.Invoke();
        }
    }
}