using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Utilities
{
    [System.Serializable]
    public class Detector<T>
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
        [SerializeField] private List<Collider> detectedColliders;

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
            detectedColliders = new List<Collider>();
        }

        public List<Collider> DetectColliders(LayerMask whatToDetected, float sphereRadius, out int amount, bool sorted)
        {
            amount = Physics.OverlapSphereNonAlloc(center.position, sphereRadius, allocatedColliders, whatToDetected);

            List<Collider> collList = new List<Collider>();

            for (int i = 0; i < amount; i++)
            {
                collList.Add(allocatedColliders[i]);
            }

            if (sorted) SortCollidersByDistance(collList);

            return collList;
        }

        public void Update()
        {
            Detect();
        }

        public List<Collider> GetCurrentColliders()
        {
            return detectedColliders;
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

        public void SortCollidersByDistance(List<Collider> currentColl)
        {
            if (currentColl.Count > 0)
            {
                currentColl.Sort(delegate (Collider a, Collider b)
                {   
                    return Vector3.SqrMagnitude(a.transform.position - center.position).CompareTo(
                     Vector3.SqrMagnitude(b.transform.position - center.position));
                });
            }
        }

        public void Reset()
        {
            detectedColliders = new List<Collider>();
            allocatedColliders = new Collider[maxDetectableColliders];
        }

        private void Detect()
        {
            elapsedTime += Time.deltaTime;
            if (!(elapsedTime > frameRate)) return;
            
            int amount  = Physics.OverlapSphereNonAlloc(center.position, radius, allocatedColliders, detectionMask);

            elapsedTime = 0;

            for (int i = 0; i < amount; i++)
            {
                if (detectedColliders.Contains(allocatedColliders[i])) continue;
                
                detectedColliders.Add(allocatedColliders[i]);

                // Invoke the detected objetive event.
                OnTargetDetected?.Invoke(allocatedColliders[i].GetComponent<T>());
            }

            // I lost at least one collider.
            if (amount < detectedColliders.Count)
            {
                for (int i = 0; i < detectedColliders.Count; i++)
                {
                    var isInside = false;

                    for (int j = 0; j < amount && !isInside; j++)
                    {
                        if (detectedColliders[i].Equals(allocatedColliders[j]))
                        {
                            // The collider is still inside the range.
                            isInside = true;
                        }
                    }

                    if (isInside) continue;

                    if (detectedColliders[i] == null)
                    {
                        detectedColliders.Remove(detectedColliders[i]);
                        continue;
                    }
                    OnTargetLost?.Invoke(detectedColliders[i].GetComponent<T>());
                    detectedColliders.Remove(detectedColliders[i]);
                }
            }

            if (sortByDistance)
                SortCollidersByDistance(detectedColliders);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(center.position, radius);

            for (int i = 0; i < detectedColliders.Count; i++)
            {
                Gizmos.DrawLine(center.position, detectedColliders[i].transform.position);
            }
        }
    }
}