using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Utilities
{
    [System.Serializable]
    public class Detector<T> where T : Component
    {
        [Header("Settings")] 
        private Transform center;
        private LayerMask detectionMask;
        private float radius;
        private float frameRate = 0.25f;
        private int maxDetectableColliders = 10;
        private bool sortByDistance;
        private float elapsedTime = 0f;
        
        [Header("Status")]
        [SerializeField] private List<T> detectedComponents;

        public event Action<T> OnTargetDetected;
        public event Action<T> OnTargetLost;

        private Collider[] allocatedColliders;

        public Detector(Transform center, DetectorSettings settings)
        {
            this.center = center;
            detectionMask = settings.detectionMask;
            radius = settings.radius;
            frameRate = settings.frameRate;
            maxDetectableColliders = settings.maxDetectableColliders;
            sortByDistance = settings.sortByDistance;
            elapsedTime = 0f;
            
            allocatedColliders = new Collider[maxDetectableColliders];
            detectedComponents = new List<T>();
        }

        public void Update()
        {
            Detect();
        }

        public List<T> GetCurrentDetectedComponents()
        {
            return detectedComponents;
        }
        
        public static void DetectColliders(Vector3 center, float radius, out Collider[] hits, LayerMask detectionLayer)
        {
            Collider[] colliders = new Collider[10];
            int collidersAmount = Physics.OverlapSphereNonAlloc(center, radius, colliders, detectionLayer);
            hits = new Collider[collidersAmount];

            for (int i = 0; i < hits.Length; i++)
            {
                hits[i] = colliders[i];
            }
        }

        public List<T> Detect(float range)
        {
            detectedComponents = new();
            
            int collidersAmount = Physics.OverlapSphereNonAlloc(center.position, range, allocatedColliders, detectionMask);
            
            for (int i = 0; i < collidersAmount; i++)
            {
                var component = allocatedColliders[i].GetComponent<T>();
                if (component != null)
                {
                    if (!detectedComponents.Contains(component))
                    {
                        detectedComponents.Add(component);
                    }
                }
            }

            return detectedComponents;
        }
        
        public void SortByDistance(List<T> currentColl)
        {
            if (currentColl.Count > 0)
            {
                currentColl.Sort((a, b) =>
                {
                    return Vector3.SqrMagnitude(a.transform.position - center.position).CompareTo(
                        Vector3.SqrMagnitude(b.transform.position - center.position));
                });
            }
        }

        public void Reset()
        {
            detectedComponents = new List<T>();
            allocatedColliders = new Collider[maxDetectableColliders];
        }

        private void Detect()
        {
            elapsedTime += Time.deltaTime;
            if (!(elapsedTime > frameRate)) 
                return;
            
            int amount  = Physics.OverlapSphereNonAlloc(center.position, radius, allocatedColliders, detectionMask);

            elapsedTime = 0;

            for (int i = 0; i < amount; i++)
            {
                var detectedComponent = allocatedColliders[i].GetComponent<T>();

                if (detectedComponent == null)
                    continue;
                                    
                if (detectedComponents.Contains(detectedComponent)) 
                    continue;
                
                detectedComponents.Add(detectedComponent);

                // Invoke the detected objetive event.
                OnTargetDetected?.Invoke(detectedComponent);
            }

            var componentsToRemove = new List<T>();
            foreach (var component in detectedComponents)
            {
                if (Vector3.Distance(component.transform.position, center.position) > radius)
                {
                    componentsToRemove.Add(component);
                }
            }

            foreach (var component in componentsToRemove)
            {
                detectedComponents.Remove(component);
                OnTargetLost?.Invoke(component);
            }

            if (sortByDistance)
                SortByDistance(detectedComponents);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(center.position, radius);

            for (int i = 0; i < detectedComponents.Count; i++)
            {
                Gizmos.DrawLine(center.position, detectedComponents[i].transform.position);
            }
        }
    }
}