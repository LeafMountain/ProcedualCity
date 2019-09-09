using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshModuleGenerator : MonoBehaviour
{
    public Mesh[] meshes = null;    // Take multiple meshes instead?
    public Vector3 sizePerTile = Vector3.one;

    private void Start()
    {
        GenerateModules();
    }

    public List<Module> GenerateModules()
    {
        List<Module> modules = new List<Module>();
        for (int i = 0; i < meshes.Length; i++)
        {
            List<Vector3> verts = GetViableVerts(meshes[i]);
            Vector3 topRightCorner = meshes[i].bounds.min + sizePerTile;
            Vector3 bottomLeftCorner = meshes[i].bounds.min;
            
            List<Vector3> left = new List<Vector3>();
            List<Vector3> right = new List<Vector3>();
            List<Vector3> back = new List<Vector3>();
            List<Vector3> forward = new List<Vector3>();

            // get all the sides verts
            for (int j = 0; j < verts.Count; j++)
            {
                // LEFT side of the shape
                if(verts[i].x == bottomLeftCorner.x)
                {
                    left.Add(verts[i]);
                }
                // RIGHT side of the shape
                else if(verts[i].x == topRightCorner.x)
                {
                    right.Add(verts[i]);
                }
                // BACK side of the shape
                else if(verts[i].z == bottomLeftCorner.z)
                {
                    back.Add(verts[i]);
                }
                // FORWARD side of the shape
                else if(verts[i].z == topRightCorner.z)
                {
                    forward.Add(verts[i]);
                }
            }

            Module module = new Module();
            module.mesh = meshes[i];
            // module.leftIdentifier = Animator.StringToHash(left.ForEach(delegate string (Vector3 v) { return string.Empty; }))
            // hash and save them into a new module
            // check for neighbors
        }
        return modules;
    }

    private List<Vector3> GetViableVerts(Mesh mesh)
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
            }
        }

        return verts2;
    }

    private void OnDrawGizmos()
    {
        for (int j = 0; j < meshes.Length; j++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireMesh(meshes[j]);

            List<Vector3> verts = GetViableVerts(meshes[j]);
            for (int i = 0; i < verts.Count; i++)
            {
                Gizmos.DrawSphere(verts[i], .05f);
            }
        }
    }
}

public class Module
{
    public Mesh mesh = null;
    
    public int upIdentifier = 0;
    public int rightIdentifier = 0;
    public int downIdentifier = 0;
    public int leftIdentifier = 0;
    public int forwardIdentifier = 0;
    public int backIdentifier = 0; 
}