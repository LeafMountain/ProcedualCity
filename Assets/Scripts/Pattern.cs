using UnityEngine;

[CreateAssetMenu]
public class PatternTemplate : ScriptableObject
{
    public Mesh mesh = null;
    // public bool allowed = true;

    // Neighbors allowed
    public PatternTemplate[] upNeighbors;
    public PatternTemplate[] rightNeighbors;
    public PatternTemplate[] downNeighbors;
    public PatternTemplate[] leftNeighbors;
}

[System.Serializable]
public class Pattern 
{
    public PatternTemplate template = null;
    public bool allowed = true;

    public Pattern(PatternTemplate template)
    {
        this.template = template;
    }
}