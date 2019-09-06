using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PatternTemplate : ScriptableObject
{
    // Temp
    // private GameObject go;
    // public GameObject prefab {
    //     get{
    //         if(go == null)
    //         {
    //             go = new Prefa();
    //             go.AddComponent<SpriteRenderer>().sprite = sprite;
    //         }
    //         return go;
    //     }
    // }
    // public bool allowed = true;
    public Sprite sprite = null;
    public int timesUsed = 0;

    public int upIdentifier = 0;
    public int rightIdentifier = 0;
    public int downIdentifier = 0;
    public int leftIdentifier = 0;


    // Neighbors allowed
    public List<PatternTemplate> upNeighbors = new List<PatternTemplate>();
    public List<PatternTemplate> rightNeighbors = new List<PatternTemplate>();
    public List<PatternTemplate> downNeighbors = new List<PatternTemplate>();
    public List<PatternTemplate> leftNeighbors = new List<PatternTemplate>();
}

