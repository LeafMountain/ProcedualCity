using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Pattern[] patterns = null;
    public Pattern finalPattern = null;
    public Vector2Int position = new Vector2Int(-1, -1);

    public void Init (Pattern[] patterns)
    {
        this.patterns = patterns;
    }

    public int GetAllowedPatternsCount()
    {
        if(finalPattern != null)
        {
            return 0;
        }

        int count = 0;
        for (int i = 0; i < patterns.Length; i++)
        {
            if(patterns[i].allowed == true)
            {
                ++count;
            }
        }

        return count;
    }

    public void PickTileManually(int index)
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            if(i != index)
            {
                patterns[i].allowed = false;
            }
        }
        Collapse();
    }

    public bool Collapse()
    {
        List<int> allowedPatterns = new List<int>();
        for (int i = 0; i < patterns.Length; i++)
        {
            if(patterns[i].allowed == true)
            {
                allowedPatterns.Add(i);
            }
        }

        if(allowedPatterns.Count <= 0)
        {
            Debug.LogError("No allowed patterns to pick from.");
            return false;
        }

        int index = Random.Range(0, allowedPatterns.Count);
        finalPattern = patterns[allowedPatterns[index]];

        GameObject visual = GameObject.Instantiate(finalPattern.template.prefab, (Vector2)position, Quaternion.identity);
        // visual.AddComponent<MeshFilter>().sharedMesh = finalPattern.template.mesh;
        // visual.AddComponent<MeshRenderer>();
        // visual.transform.position = new Vector3(position.x, 0, position.y);
        visual.name = finalPattern.template.name;

        return true;
    }

    public void SyncWithNeighbor(Tile neightborTile, Direction neighborDirection)
    {
        if(neighborDirection == Direction.up)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                // Check if not contains
                if(System.Array.IndexOf(patterns[i].template.upNeighbors, neightborTile.finalPattern.template) == -1)
                {
                    patterns[i].allowed = false;
                }
            }
        }
        else if(neighborDirection == Direction.right)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                // Check if not contains
                if(System.Array.IndexOf(patterns[i].template.rightNeighbors, neightborTile.finalPattern.template) == -1)
                {
                    patterns[i].allowed = false;
                }
            }
        }
        else if(neighborDirection == Direction.down)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                // Check if not contains
                if(System.Array.IndexOf(patterns[i].template.downNeighbors, neightborTile.finalPattern.template) == -1)
                {
                    patterns[i].allowed = false;
                }
            }
        }
        else if(neighborDirection == Direction.left)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                // Check if not contains
                if(System.Array.IndexOf(patterns[i].template.leftNeighbors, neightborTile.finalPattern.template) == -1)
                {
                    patterns[i].allowed = false;
                }
            }
        }
    }

    public void AddPatternTemplates(PatternTemplate[] templates)
    {
        patterns = new Pattern[templates.Length];
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i] = new Pattern(templates[i]);
            patterns[i].allowed = true;
        }
    }

    public enum Direction
    {
        up,
        right,
        down,
        left
    }
}