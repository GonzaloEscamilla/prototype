using UnityEngine;

namespace _Project.Scripts.Core.UI
{
    public class Billboard : MonoBehaviour
    {
        private Camera _main;

        private void Awake() => _main = Camera.main;

        protected virtual void LateUpdate() => transform.LookAt(transform.position + _main.transform.forward);
    }
}