using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class SelfDestroy: MonoBehaviour
    {
        private void Awake()
        {
            Destroy(this.gameObject);
        }
    }
}