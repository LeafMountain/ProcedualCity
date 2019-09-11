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
            modules.Add(CreateModule(meshes[i]));
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
                if(currentModule.upIdentifier == modules[j].downIdentifier)
                {
                    if(currentModule.upNeighbors.Contains(modules[j]) == false)
                    {
                        currentModule.upNeighbors.Add(modules[j]);
                        modules[j].downNeighbors.Add(currentModule);
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
        // verts.Sort(Vector3Sort);


        string combined = string.Empty;
        for (int i = 0; i < verts.Count; i++)
        {
            combined += verts[i].ToString();
        }
        return Animator.StringToHash(combined);
    }

    private Module CreateModule(Mesh mesh)
    {
        List<Vector3> verts = new List<Vector3>();
        mesh.GetVertices(verts);
        verts = verts.Distinct().ToList();
        for (int i = 0; i < verts.Count; i++)
        {
            Vector3 vert = verts[i];
            vert.x = (float)System.Math.Round(verts[i].x, 2);
            vert.y = (float)System.Math.Round(verts[i].y, 2);
            vert.z = (float)System.Math.Round(verts[i].z, 2);
            verts[i] = vert;
        }

        float allowedDiff = 0f;

        List<Vector3> left = new List<Vector3>();
        List<Vector3> right = new List<Vector3>();
        List<Vector3> back = new List<Vector3>();
        List<Vector3> forward = new List<Vector3>();
        List<Vector3> up = new List<Vector3>();
        List<Vector3> down = new List<Vector3>();        

        for (int i = verts.Count - 1; i >= 0; i--)
        {
            // LEFT
            if(Mathf.Abs(verts[i].x - (mesh.bounds.min).x) <= allowedDiff)
            {
                Vector3 vert = verts[i];
                vert.x = 0;
                left.Add(vert);
            }
            // RIGHT
            else if(Mathf.Abs(verts[i].x - (mesh.bounds.min + sizePerTile).x) <= allowedDiff)
            {
                Vector3 vert = verts[i];
                vert.x = 0;
                right.Add(vert);
            }
            
            // DOWN
            if(Mathf.Abs(verts[i].y - (mesh.bounds.min).y) <= allowedDiff)
            {
                Vector3 vert = verts[i];
                vert.y = 0;
                down.Add(vert);
            } 
            // UP
            else if(Mathf.Abs(verts[i].y - (mesh.bounds.min + sizePerTile).y) <= allowedDiff)
            {
                Vector3 vert = verts[i];
                vert.y = 0;
                up.Add(vert);
            }

            // float test = (mesh.bounds.min).z + ((mesh.bounds.min).z - 0.5f);

            // BACK
            if(Mathf.Abs(verts[i].z - (mesh.bounds.min).z) <= allowedDiff)
            {
                Vector3 vert = verts[i];
                vert.z = 0;
                back.Add(vert);
            }
            //FORWARD
            else if(Mathf.Abs(verts[i].z - (mesh.bounds.min + sizePerTile).z) <= allowedDiff)
            {
                Vector3 vert = verts[i];
                vert.z = 0;
                forward.Add(vert);
            }
        }

        Module module = new Module();
        module.mesh = mesh;

        forward.Sort(Vector3Sort);
        back.Sort(Vector3Sort);
        right.Sort(Vector3Sort);
        left.Sort(Vector3Sort);        
        up.Sort(Vector3Sort);
        down.Sort(Vector3Sort);

        module.forwardIdentifier = CreateHash(forward);
        module.backIdentifier = CreateHash(back);
        module.rightIdentifier = CreateHash(right);
        module.leftIdentifier = CreateHash(left);
        module.upIdentifier = CreateHash(up);
        module.downIdentifier = CreateHash(down);
        module.name = module.mesh.name;
        module.material = defaultMaterial;
        
        return module;
    }

    private int Vector3Sort(Vector3 vert1, Vector3 vert2)
    {
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

            // List<Vector3> verts = CreateModule(meshes[j]);
            // for (int i = 0; i < verts.Count; i++)
            // {
            //     Gizmos.DrawSphere(verts[i] + position, .05f);
            // }
        }
    }
}