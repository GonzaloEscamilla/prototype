using UnityEngine;

namespace _Project.Scripts.Core
{
    public abstract class BaseInteractable : MonoBehaviour
    {
        public abstract void Interact(GameObject interactionSource);
    }
}