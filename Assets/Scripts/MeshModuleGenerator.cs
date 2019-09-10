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
            VisualDebug(GenerateTemplate());
        }
    }

    public override Module[] GenerateTemplate()
    {
        List<Module> modules = new List<Module>();
        for (int i = 0; i < meshes.Length; i++)
        {
            List<Vector3> verts = GetViableVerts(meshes[i]);            
            List<Vector3> left = new List<Vector3>();
            List<Vector3> right = new List<Vector3>();
            List<Vector3> back = new List<Vector3>();
            List<Vector3> forward = new List<Vector3>();
            List<Vector3> up = new List<Vector3>();
            List<Vector3> down = new List<Vector3>();

            // get all the sides verts
            for (int j = 0; j < verts.Count; j++)
            {
                // LEFT side of the shape
                if(verts[j].x == -(sizePerTile.x * .5f))
                {
                    Vector3 vert = verts[j];
                    vert.x = 0;
                    left.Add(vert);
                }
                // RIGHT side of the shape
                else if(verts[j].x == sizePerTile.x * .5f)
                {
                    Vector3 vert = verts[j];
                    vert.x = 0;
                    right.Add(vert);
                }

                // DOWN side of the shape
                if(verts[j].y == -sizePerTile.y * .5f)
                {
                    Vector3 vert = verts[j];
                    vert.y = 0;
                    down.Add(vert);
                }
                // UP side of the shape
                else if(verts[j].y == sizePerTile.y * .5f)
                {
                    Vector3 vert = verts[j];
                    vert.y = 0;
                    up.Add(vert);
                }

                // BACK side of the shape
                if(verts[j].z == -sizePerTile.z * .5f)
                {
                    Vector3 vert = verts[j];
                    vert.z = 0;
                    back.Add(vert);
                }
                // FORWARD side of the shape
                else if(verts[j].z == sizePerTile.z * .5f)
                {
                    Vector3 vert = verts[j];
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
            module.downIdentifier = CreateHash(down);
            module.upIdentifier = CreateHash(up);
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
        verts.Sort(delegate (Vector3 vert1, Vector3 vert2) {
            if(vert1.x == vert2.x)
            {
                if(vert1.y == vert2.y)
                {
                    if(vert1.z == vert2.z)
                    {
                        return 0;
                    }
                    else if(vert1.z < vert2.z)
                    {
                        return -1;
                    }
                    else if(vert1.z > vert2.z)
                    {
                        return 1;
                    }
                }
                else if(vert1.y < vert2.y)
                {
                    return -1;
                }
                else if(vert1.y > vert2.y)
                {
                    return 1;
                }
            }
            else if(vert1.x < vert2.x)
            {
                return -1;
            }
            else if(vert1.x > vert2.x)
            {
                return 1;
            }

            return 0;
        });
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
        Vector3 halfSizePerTile = sizePerTile * .5f;
        float allowedDiff = 0.1f;

        List<Vector3> left = new List<Vector3>();
        List<Vector3> right = new List<Vector3>();
        List<Vector3> back = new List<Vector3>();
        List<Vector3> forward = new List<Vector3>();
        List<Vector3> up = new List<Vector3>();
        List<Vector3> down = new List<Vector3>();

        for (int i = verts.Count - 1; i >= 0; i--)
        {
            if(Mathf.Abs(verts[i].x - (mesh.bounds.center - halfSizePerTile).x) < allowedDiff || Mathf.Abs(verts[i].x - (mesh.bounds.center + halfSizePerTile).x) < allowedDiff
            || Mathf.Abs(verts[i].y - (mesh.bounds.center - halfSizePerTile).y) < allowedDiff || Mathf.Abs(verts[i].y - (mesh.bounds.center + halfSizePerTile).y) < allowedDiff
            || Mathf.Abs(verts[i].z - (mesh.bounds.center - halfSizePerTile).z) < allowedDiff || Mathf.Abs(verts[i].z - (mesh.bounds.center + halfSizePerTile).z) < allowedDiff)
            {
                // verts.RemoveAt(i);
                verts2.Add(verts[i]);
            }
                        // if(verts[i].x == (mesh.bounds.center - halfSizePerTile).x || verts[i].x == (mesh.bounds.center + halfSizePerTile).x
            // || verts[i].y == (mesh.bounds.center - halfSizePerTile).y || verts[i].y == (mesh.bounds.center + halfSizePerTile).y
            // || verts[i].z == (mesh.bounds.center - halfSizePerTile).z || verts[i].z == (mesh.bounds.center + halfSizePerTile).z)
            // {
            //     // verts.RemoveAt(i);
            //     verts2.Add(verts[i]);
            // }
        }
        
        // for (int i = verts.Count - 1; i >= 0; i--)
        // {
        //     // if((verts[i].x - mesh.bounds.min.x) % sizePerTile.x == 0 || (verts[i].z - mesh.bounds.min.z) % sizePerTile.z == 0)
        //     // {
        //     //     // verts.RemoveAt(i);
        //     //     verts2.Add(verts[i]);
        //     // }
        // }

        return verts2;
    }

    private void VisualDebug(Module[] modules)
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
            Vector3 position = transform.position + new Vector3(1 * j * sizePerTile.x, 0, 0);
            Gizmos.color = Color.white;
            Gizmos.DrawWireMesh(meshes[j], position, Quaternion.identity);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(position, sizePerTile);

            List<Vector3> verts = GetViableVerts(meshes[j]);
            for (int i = 0; i < verts.Count; i++)
            {
                Gizmos.DrawSphere(verts[i] + position, .05f);
            }
        }
    }
}