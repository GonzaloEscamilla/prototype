using UnityEngine;

public class CopyPositionWithOffset : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float heightOffset = 1000f;
    [SerializeField] private Transform camera;

    private void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y + heightOffset, target.position.z);
            transform.rotation = camera.rotation;
        }
    }
}