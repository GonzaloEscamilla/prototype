using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Utilities.Math
{
    public class  RotationTween : MonoBehaviour
    {
        [SerializeField] private EasingFunction.EaseType type;
        [SerializeField] float duration = 1; // seconds, must be > 0.0f

        private EasingFunction.Function _function;
        private Quaternion targetRotation = Quaternion.identity;

        [ContextMenu("Test Simple Rotation")]
        public void TestRotation()
        {
            RotateObject(this.gameObject, Vector3.up, 90);
        }

        /// <summary>
        /// Rotates and object on a defined axis a desired amount of euler angles.
        /// </summary>
        /// <param name="objToRotate"></param>
        /// <param name="axis">Must be in the from of a vector 3 representing the axis e.g (1,0,0) for positive x axis, or (0,-1,0) for negative y axis. Other values might run in to undesired results.</param>
        /// <param name="anglesToRotate"></param>
        public void RotateObject(GameObject objToRotate, Vector3 axis, int anglesToRotate )
        {
            var initialRotation = objToRotate.transform.rotation;

            targetRotation = Quaternion.Euler(axis * anglesToRotate) * targetRotation;

            StopAllCoroutines();
            StartCoroutine(DoRotation(objToRotate, initialRotation, targetRotation));
        }

        private IEnumerator DoRotation(GameObject objToRotate, Quaternion from, Quaternion to)
        {
            if (duration <= 0f)
            {
                Debug.LogWarning("Time must be greater than 0.0f.", this.gameObject);
                StopAllCoroutines();
                yield break;
            }
            
            _function = EasingFunction.GetEasingFunction(type);
        
            float currentTime = 0;

            while (currentTime < 1)
            {
                currentTime += Time.deltaTime / duration;
                objToRotate.transform.rotation = QuaternionEasing(from, to, currentTime);

                yield return null;
            }
            
            objToRotate.transform.rotation = to;
        }

        private Quaternion QuaternionEasing(Quaternion from, Quaternion to, float amount)
        {
            var result = Quaternion.identity;

            result.x = _function(from.x, to.x, amount);
            result.y = _function(from.y, to.y, amount);
            result.z = _function(from.z, to.z, amount);
            result.w = _function(from.w, to.w, amount);
        
            return result;
        }
    }
}
