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

    public bool Collapse()
    {
        int tries = 0;
        int defititePatter = Random.Range(0, patterns.Length);
        while(patterns[defititePatter].allowed == false && tries < 100)
        {
            defititePatter = Random.Range(0, patterns.Length);
            tries++;
        }
        if(patterns[defititePatter].allowed == false)
        {
            Debug.LogWarning("No solution found");
            return false;
        }

        GameObject visual = new GameObject();
        visual.AddComponent<MeshFilter>().sharedMesh = patterns[defititePatter].template.mesh;
        visual.AddComponent<MeshRenderer>();
        visual.transform.position = new Vector3(position.x, 0, position.y);

        return true;
    }

    public void SyncWithNeighbor(Tile neightborTile, Direction neighborDirection)
    {
        if(neighborDirection == Direction.up)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                // Check if not contains
                if(System.Array.IndexOf(patterns[i].template.upNeighbors, neightborTile.finalPattern) == -1)
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
                if(System.Array.IndexOf(patterns[i].template.rightNeighbors, neightborTile.finalPattern) == -1)
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
                if(System.Array.IndexOf(patterns[i].template.downNeighbors, neightborTile.finalPattern) == -1)
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
                if(System.Array.IndexOf(patterns[i].template.leftNeighbors, neightborTile.finalPattern) == -1)
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