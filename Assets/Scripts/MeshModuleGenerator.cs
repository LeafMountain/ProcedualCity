using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshModuleGenerator
{
    // public Mesh[] meshes = null;
    public Vector3 sizePerTile = Vector3.one;
    public Material defaultMaterial = null;
    public GameObject test;

    public bool debug = false;

    // private void Start()
    // {
    //     MeshFilter[] filters = test.GetComponentsInChildren<MeshFilter>();
    //     meshes = new Mesh[filters.Length];
    //     for (int i = 0; i < filters.Length; i++)
    //     {
    //         meshes[i] = filters[i].sharedMesh;
    //     }

    //     // if(debug)
    //     // {
    //     //     VisualDebug(GenerateTemplate());
    //     // }
    // }

    static public Module[] GenerateTemplate(GameObject test, Material material)
    {
        Mesh[] meshes = null;

        MeshFilter[] filters = test.GetComponentsInChildren<MeshFilter>();
        meshes = new Mesh[filters.Length];
        for (int i = 0; i < filters.Length; i++)
        {
            meshes[i] = filters[i].sharedMesh;
        }

        List<Module> modules = new List<Module>();
        for (int i = 0; i < meshes.Length; i++)
        {
            modules.Add(CreateModule(meshes[i], material));
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

    static private int CreateHash(List<Vector3> verts)
    {
        // verts.Sort(Vector3Sort);


        string combined = string.Empty;
        for (int i = 0; i < verts.Count; i++)
        {
            combined += verts[i].ToString();
        }
        return Animator.StringToHash(combined);
    }

    private bool OnEdge(Vector3 position)
    {
        Vector3 halfSize = sizePerTile * .5f;
        return position.x == -halfSize.x || position.x == halfSize.x || position.y == -halfSize.y || position.y == halfSize.y || position.z == -halfSize.z || position.z == halfSize.z;
    }

    static private Module CreateModule(Mesh mesh, Material material)
    {
        Vector3 sizePerTile = Vector3.one;

        List<Vector3> verts = GetVertsAlogEdges(mesh);
        // mesh.GetVertices(verts);
        verts = verts.Distinct().ToList();
        for (int i = 0; i < verts.Count; i++)
        {
            Vector3 vert = verts[i];
            vert.x = (float)System.Math.Round(verts[i].x, 2);
            vert.y = (float)System.Math.Round(verts[i].y, 2);
            vert.z = (float)System.Math.Round(verts[i].z, 2);
            verts[i] = vert;
        }

        List<Vector3> left = new List<Vector3>();
        List<Vector3> right = new List<Vector3>();
        List<Vector3> back = new List<Vector3>();
        List<Vector3> forward = new List<Vector3>();
        List<Vector3> up = new List<Vector3>();
        List<Vector3> down = new List<Vector3>();      

        Vector3 halfSizePerTile = sizePerTile * .5f;

        for (int i = verts.Count - 1; i >= 0; i--)
        {
            // LEFT
            if(verts[i].x == (-halfSizePerTile.x))
            {
                Vector3 vert = verts[i];
                vert.x = 0;
                left.Add(vert);
            }
            // RIGHT
            else if(verts[i].x == (halfSizePerTile.x))
            {
                Vector3 vert = verts[i];
                vert.x = 0;
                right.Add(vert);
            }
            
            // DOWN
            if(verts[i].y == (-halfSizePerTile.y))
            {
                Vector3 vert = verts[i];
                vert.y = 0;
                down.Add(vert);
            } 
            // UP
            else if(verts[i].y == (halfSizePerTile.y))
            {
                Vector3 vert = verts[i];
                vert.y = 0;
                up.Add(vert);
            }

            // BACK
            if(verts[i].z == (-halfSizePerTile.z))
            {
                Vector3 vert = verts[i];
                vert.z = 0;
                back.Add(vert);
            }
            //FORWARD
            else if(verts[i].z == (halfSizePerTile.z))
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
        module.material = material;
        
        return module;
    }

    private static int Vector3Sort(Vector3 vert1, Vector3 vert2)
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

    // private void VisualDebug(Module[] modules)
    // {
    //     for (int i = 0; i < modules.Length; i++)
    //     {
    //         for (int j = 0; j < modules[i].forwardNeighbors.Count; j++)
    //         {
    //             GameObject go = new GameObject();
    //             go.AddComponent<MeshFilter>().mesh = modules[i].mesh;
    //             go.AddComponent<MeshRenderer>().material = defaultMaterial;
    //             go.transform.position = new Vector3(j, 0, i * 3);
    //             go.name = modules[i].name;

    //             GameObject go2 = new GameObject();
    //             go2.AddComponent<MeshFilter>().mesh = modules[i].forwardNeighbors[j].mesh;
    //             go2.AddComponent<MeshRenderer>().material = defaultMaterial;
    //             go2.transform.position = new Vector3(j, 0, 1 + i * 3);
    //             go2.name = modules[i].forwardNeighbors[j].name;
    //         }
    //     }
    // }

    public class Edge
    {
        public Vector3 first, second;
        public Edge(Vector3 first, Vector3 second)
        {
            if(Vector3Sort(first, second) == 1)
            {
                this.first = second;
                this.second = first;
            }
            else
            {
                this.first = first;
                this.second = second;
            }
        }

        public bool Compare(Edge other)
        {
            return (first == other.first && second == other.second);
        }
    }

    static public List<Edge> GetEdges(Mesh mesh)
    {
        List<Edge> edges = new List<Edge>(mesh.triangles.Length / 3);

        for (int i = 0; i < mesh.triangles.Length - 2; i += 3)
        {
            {
                Vector3 vert1 = mesh.vertices[mesh.triangles[i]];
                Vector3 vert2 = mesh.vertices[mesh.triangles[i + 1]];
                edges.Add(new Edge(vert1, vert2));
            }
            {
                Vector3 vert1 = mesh.vertices[mesh.triangles[i + 1]];
                Vector3 vert2 = mesh.vertices[mesh.triangles[i + 2]];
                edges.Add(new Edge(vert1, vert2));
            }
            {
                Vector3 vert1 = mesh.vertices[mesh.triangles[i + 2]];
                Vector3 vert2 = mesh.vertices[mesh.triangles[i]];
                edges.Add(new Edge(vert1, vert2));
            }
        }

        edges.Sort(delegate(Edge edge1, Edge edge2)
        {
            int firstSort = Vector3Sort(edge1.first, edge2.first);
            if(edge1.first == edge2.first)
            {
                return Vector3Sort(edge1.second, edge2.second);
            }
            else
            {
                return firstSort;
            }
        });

        for (int i = edges.Count - 1; i >= 1 ; i--)
        {
            if(edges[i].Compare(edges[i - 1]) == true)
            {
                edges.RemoveAt(i);
                edges.RemoveAt(i - 1);
                i -= 1;
            }
        }

        return edges;
    }

    static public List<Vector3> GetVertsAlogEdges(Mesh mesh)
    {
        List<Edge> edges = GetEdges(mesh);
        List<Vector3> edgeVerts = new List<Vector3>(edges.Count * 2);
        for (int i = 0; i < edges.Count; i++)
        {
            edgeVerts.Add(edges[i].first);
            edgeVerts.Add(edges[i].second);
        }
        return edgeVerts;
    }

    // private void OnDrawGizmosSelected()
    // {
    //     for (int j = 0; j < meshes.Length; j++)
    //     {
    //         Vector3 position = transform.position + new Vector3(1 * j * sizePerTile.x, 0, 0);
    //         Gizmos.color = Color.white;
    //         Gizmos.DrawWireMesh(meshes[j], position, Quaternion.identity);
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawWireCube(position, sizePerTile);

    //         // List<Vector3> verts = CreateModule(meshes[j]);
    //         // for (int i = 0; i < verts.Count; i++)
    //         // {
    //         //     Gizmos.DrawSphere(verts[i] + position, .05f);
    //         // }
    //     }

    //     Vector3 offset = new Vector3(0, 0, -5);
    //     for (int j = 0; j < meshes.Length; j++)
    //     {
    //         List<Edge> edges = GetEdges(meshes[j]);
    //         for (int i = 0; i < edges.Count; i++)
    //         {
    //             Gizmos.DrawLine(edges[i].first + offset + Vector3.right * (j + j), edges[i].second + offset + Vector3.right * (j + j));
    //         }      
    //     }
    // }
}