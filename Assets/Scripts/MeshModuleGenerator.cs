using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshModuleGenerator : MonoBehaviour
{
    public Mesh mesh = null;    // Take multiple meshes instead?
    public Vector3 sizePerTile = Vector3.one;

    // Start is called before the first frame update
    private void Start()
    {
        GetViableVerts();
    }

    public void GenerateModules()
    {
        List<Vector3> verts = GetViableVerts();

    }

    private List<Vector3> GetViableVerts()
    {
        List<Vector3> verts = new List<Vector3>();
        mesh.GetVertices(verts);
        verts = verts.Distinct().ToList();
        List<Vector3> verts2 = new List<Vector3>();
        
        for (int i = verts.Count - 1; i >= 0; i--)
        {
            if((verts[i].x - mesh.bounds.min.x) % sizePerTile.x == 0 || (verts[i].z - mesh.bounds.min.z) % sizePerTile.z == 0)
            {
                // verts.RemoveAt(i);
                verts2.Add(verts[i]);
                // Gizmos.DrawSphere(verts[i], .05f);
            }
        }

        return verts2;
    }

    private void OnDrawGizmos()
    {
        List<Vector3> verts = GetViableVerts();
        for (int i = 0; i < verts.Count; i++)
        {
            // float test = verts[i].x - mesh.bounds.min.x;
            // if((test % sizePerTile.x) == 0 || (verts[i].z - mesh.bounds.min.x) % sizePerTile.z == 0)
            // {
                Gizmos.DrawSphere(verts[i], .05f);
            // }
        }
        

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireMesh(mesh);

        // Vector3 meshSize = mesh.bounds.size;
        // if (meshSize.x == 0) meshSize.x = 1;
        // if (meshSize.y == 0) meshSize.y = 1;
        // if (meshSize.z == 0) meshSize.z = 1;

        // Vector3Int gridSize = Vector3Int.zero;
        // gridSize.x = Mathf.CeilToInt(meshSize.x / sizePerTile.x);
        // gridSize.y = Mathf.CeilToInt(meshSize.y / sizePerTile.y);
        // gridSize.z = Mathf.CeilToInt(meshSize.z / sizePerTile.z);

    }
}
