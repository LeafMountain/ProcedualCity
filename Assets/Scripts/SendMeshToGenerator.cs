using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMeshToGenerator : MonoBehaviour
{
    public GameObject mesh = null;
    public Material material = null;
    public Vector3Int size = new Vector3Int(10, 10, 10);

    public bool sendOnStart = false;
    
    private void Start()
    {
        if(sendOnStart == true)
        {
            Send();
        }
    }

    public void Send()
    {
        if(mesh != null)
        {
            Board.Instance.Regenerate(MeshModuleGenerator.GenerateTemplate(mesh, material), size);
        }
    }
}
