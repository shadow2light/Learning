using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class WaterCpu:MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var vertices = _mesh.vertices;
                var dir = new Vector3(0, 0, 1);
                for (int i = 0; i < vertices.Length/10000; i++)
                {
                    var result=2*1*Mathf.Pow(Mathf.Sin(Vector3.Dot(dir, vertices[i])*2/20+Time.time*50*2/20+1)/2, 1);
                    Debug.Log(result);
                }
            }
        }
    }
}