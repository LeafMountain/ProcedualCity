using UnityEngine;

[CreateAssetMenu]
public class Pattern : ScriptableObject
{
    public Mesh mesh = null;
    public bool allowed = true;

    // Neighbors allowed
    public Pattern[] upNeighbors;
    public Pattern[] rightNeighbors;
    public Pattern[] downNeighbors;
    public Pattern[] leftNeighbors;
}