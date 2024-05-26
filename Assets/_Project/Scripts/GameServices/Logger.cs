using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.GameServices
{
    [AddComponentMenu("ND/Services/Logger")]
    public class Logger : MonoBehaviour, IDebug
    {
        [Header("Settings")] 
        [SerializeField] private bool _showLogs;

        [SerializeField] private string _prefix;

        [SerializeField] private Color _prefixColor;

        private string _hexColor;

        private void OnValidate() 
        {
            _hexColor = "#" + ColorUtility.ToHtmlStringRGB(_prefixColor);
        }

        public void Log(object message, Object sender = null)
        {
            InternalLog(message,sender);
        }

        [System.Diagnostics.Conditional("ENABLE_LOGS")]
        private void InternalLog(object message, Object sender = null)
        {
            if (!_showLogs) return;

            Debug.Log($"<color={_hexColor}>{_prefix}: {message}</color>",sender);
        }
        
        public void LogWarning(object message, Object sender = null)
        {
            InternalLogWarning(message, sender);
        }

        public void LogError(object message, Object sender = null)
        {
            InternalLogError(message, sender);
        }

        [System.Diagnostics.Conditional("ENABLE_LOGS")]
        private void InternalLogWarning(object message, Object sender = null)
        {
            if (!_showLogs) return;

            Debug.LogWarning($"<color={_hexColor}>{_prefix}: {message}</color>",sender);
        }
        
        [System.Diagnostics.Conditional("ENABLE_LOGS")]
        private void InternalLogError(object message, Object sender = null)
        {
            if (!_showLogs) return;

            Debug.LogError($"<color={_hexColor}>{_prefix}: {message}</color>",sender);
        }
        
        public string Prefix
        {
            get => _prefix;
            set => _prefix = value;
        }
        public Color PrefixColor
        {
            get => _prefixColor;
            set
            {
                _prefixColor = value;
                _hexColor = "#" + ColorUtility.ToHtmlStringRGB(_prefixColor);
            }
        }
    }
}
