using UnityEngine;

[CreateAssetMenu]
public class PatternTemplate : ScriptableObject
{
    // public Mesh mesh = null;
    public GameObject prefab = null;
    // public bool allowed = true;
    public Sprite sprite = null;

    // Neighbors allowed
    public PatternTemplate[] upNeighbors;
    public PatternTemplate[] rightNeighbors;
    public PatternTemplate[] downNeighbors;
    public PatternTemplate[] leftNeighbors;
}

