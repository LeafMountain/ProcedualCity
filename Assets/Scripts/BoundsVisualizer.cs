using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BoundsVisualizer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        MeshFilter mf = GetComponent<MeshFilter>();
        // var test = mf.sharedMesh.bone;
        // Gizmos.DrawWireCube(transform.position + mf.sharedMesh.GetAllBoneWeights[0], mf.sharedMesh.bounds.size);
        Gizmos.DrawSphere(transform.position + mf.sharedMesh.bounds.extents, .2f);
        // mf.sharedMesh.bounds.
    }
}
