using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshModuleGenerator : Generator
{
    public Mesh[] meshes = null;    // Take multiple meshes instead?
    public Vector3 sizePerTile = Vector3.one;
    public Material defaultMaterial = null;

    public bool debug = false;

    private void Start()
    {
        if(debug)
        {
            Debug(GenerateTemplate());
        }
    }

    public override Module[] GenerateTemplate()
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
                    Vector3 vert = verts[i];
                    vert.x = 0;
                    left.Add(vert);
                }
                // RIGHT side of the shape
                else if(verts[i].x == topRightCorner.x)
                {
                    Vector3 vert = verts[i];
                    vert.x = 0;
                    right.Add(vert);
                }
                // BACK side of the shape
                else if(verts[i].z == bottomLeftCorner.z)
                {
                    Vector3 vert = verts[i];
                    vert.z = 0;
                    back.Add(vert);
                }
                // FORWARD side of the shape
                else if(verts[i].z == topRightCorner.z)
                {
                    Vector3 vert = verts[i];
                    vert.z = 0;
                    forward.Add(vert);
                }
            }

            // hash and save them into a new module
            Module module = new Module();
            module.mesh = meshes[i];
            module.forwardIdentifier = CreateHash(forward);
            module.rightIdentifier = CreateHash(right);
            module.backIdentifier = CreateHash(back);
            module.leftIdentifier = CreateHash(left);
            module.name = module.mesh.name;
            modules.Add(module);
        }

        for (int i = 0; i < modules.Count; i++)
        {
            Module currentModule = modules[i];

            for (int j = 0; j < modules.Count; j++)
            {
                if(currentModule.forwardIdentifier == modules[j].backIdentifier)
                {
                    if(currentModule.forwardNeighbors.Contains(modules[j]) == false)
                    {
                        currentModule.forwardNeighbors.Add(modules[j]);
                        modules[j].backNeighbors.Add(currentModule);
                    }
                }
                if(currentModule.rightIdentifier == modules[j].leftIdentifier)
                {
                    if(currentModule.rightNeighbors.Contains(modules[j]) == false)
                    {
                        currentModule.rightNeighbors.Add(modules[j]);
                        modules[j].leftNeighbors.Add(currentModule);
                    }
                }
            }
        }

        return modules.ToArray();
    }

    private int CreateHash(List<Vector3> verts)
    {
        string combined = string.Empty;
        for (int i = 0; i < verts.Count; i++)
        {
            combined += verts[i].ToString();
        }
        return Animator.StringToHash(combined);
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

    private void Debug(Module[] modules)
    {
        for (int i = 0; i < modules.Length; i++)
        {
            for (int j = 0; j < modules[i].forwardNeighbors.Count; j++)
            {
                GameObject go = new GameObject();
                go.AddComponent<MeshFilter>().mesh = modules[i].mesh;
                go.AddComponent<MeshRenderer>().material = defaultMaterial;
                go.transform.position = new Vector3(j, 0, i * 3);
                go.name = modules[i].name;

                GameObject go2 = new GameObject();
                go2.AddComponent<MeshFilter>().mesh = modules[i].forwardNeighbors[j].mesh;
                go2.AddComponent<MeshRenderer>().material = defaultMaterial;
                go2.transform.position = new Vector3(j, 0, 1 + i * 3);
                go2.name = modules[i].forwardNeighbors[j].name;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int j = 0; j < meshes.Length; j++)
        {
            Gizmos.color = Color.magenta;
            Vector3 position = transform.position + new Vector3(1 * j * sizePerTile.x, 0, 0);
            Gizmos.DrawWireMesh(meshes[j], position, Quaternion.identity);

            List<Vector3> verts = GetViableVerts(meshes[j]);
            for (int i = 0; i < verts.Count; i++)
            {
                Gizmos.DrawSphere(verts[i] + position, .05f);
            }
        }
    }
}