using UnityEngine;

public class CopyPositionWithOffset : MonoBehaviour
{
    // El objeto cuya posición se copiará.
    public Transform target;

    // La distancia de altura que queremos mantener.
    public float heightOffset = 1000f;

    // El objeto cuya rotación se copiará
    public Transform camera;

    void Update()
    {
        if (target != null)
        {
            // Copiar la posición del objeto objetivo con un offset en altura.
            transform.position = new Vector3(target.position.x, target.position.y + heightOffset, target.position.z);

            // Copiar la rotación del objeto objetivo.
            transform.rotation = camera.rotation;
        }
    }
}