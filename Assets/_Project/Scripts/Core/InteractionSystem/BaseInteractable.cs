using UnityEngine;

namespace _Project.Scripts.Core
{
    public abstract class BaseInteractable : MonoBehaviour
    {
        public abstract bool Interact(GameObject interactionSource, out object interactionResultData);
    }
}