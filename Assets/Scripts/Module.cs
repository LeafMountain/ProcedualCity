using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Module : ScriptableObject
{
    public Sprite sprite = null;
    public Mesh mesh = null;
    public Material material = null;

    public int timesUsed = 0;

    public int upIdentifier = 0;
    public int downIdentifier = 0;
    public int rightIdentifier = 0;
    public int leftIdentifier = 0;
    public int forwardIdentifier = 0;
    public int backIdentifier = 0;

    // Neighbors allowed
    public List<Module> upNeighbors = new List<Module>();
    public List<Module> rightNeighbors = new List<Module>();
    public List<Module> downNeighbors = new List<Module>();
    public List<Module> leftNeighbors = new List<Module>();
    public List<Module> forwardNeighbors = new List<Module>();
    public List<Module> backNeighbors = new List<Module>();

}

