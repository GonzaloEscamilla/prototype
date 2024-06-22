using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] colors = mesh.colors;

        for (int i = 0; i < colors.Length; i++)
        {
            Debug.Log("Vertex " + i + ": " + colors[i]);
        }
    }
}
