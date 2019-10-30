using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up, down, left, right, forward, back
}

public class Slot
{
    public Pattern[] patterns = null;
    public Pattern finalPattern = null;
    public Vector3Int position = new Vector3Int(-1, -1, -1);

    private GameObject visual = null;

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
        // Gather all the allowed patterns
        List<Pattern> allowedPatterns = new List<Pattern>();
        for (int i = 0; i < patterns.Length; i++)
        {
            if(patterns[i].allowed == true)
            {
                allowedPatterns.Add(patterns[i]);
            }
        }

        if(allowedPatterns.Count <= 0)
        {
            Debug.LogError("No allowed patterns to pick from.");
            return false;
        }

        int index = Random.Range(0, allowedPatterns.Count);

        finalPattern = allowedPatterns[index];
        finalPattern.template.timesUsed++;

        GameObject visual = finalPattern.GetVisual();
        visual.transform.position = (Vector3)position + new Vector3(-15, 0, 0);
        visual.name = finalPattern.template.name;
        this.visual = visual;

        return true;
    }

    public void SyncWithNeighbor(Slot neightborTile, Direction neighborDirection)
    {
        if(neighborDirection == Direction.up)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if(patterns[i].template.upNeighbors.Contains(neightborTile.finalPattern.template) == false)
                {
                    patterns[i].allowed = false;
                }
            }
        }
        else if(neighborDirection == Direction.right)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if(patterns[i].template.rightNeighbors.Contains(neightborTile.finalPattern.template) == false)
                {
                    patterns[i].allowed = false;
                }
            }
        }
        else if(neighborDirection == Direction.down)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if(patterns[i].template.downNeighbors.Contains(neightborTile.finalPattern.template) == false)
                {
                    patterns[i].allowed = false;
                }
            }
        }
        else if(neighborDirection == Direction.left)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if(patterns[i].template.leftNeighbors.Contains(neightborTile.finalPattern.template) == false)
                {
                    patterns[i].allowed = false;
                }
            }
        }
        else if(neighborDirection == Direction.forward)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if(patterns[i].template.forwardNeighbors.Contains(neightborTile.finalPattern.template) == false)
                {
                    patterns[i].allowed = false;
                }
            }
        }
        else if(neighborDirection == Direction.back)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if(patterns[i].template.backNeighbors.Contains(neightborTile.finalPattern.template) == false)
                {
                    patterns[i].allowed = false;
                }
            }
        }
    }

    public void AddPatternTemplates(Module[] templates)
    {
        patterns = new Pattern[templates.Length];
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i] = new Pattern(templates[i]);
            patterns[i].allowed = true;
        }
    }

    public void Destroy()
    {
        if(visual != null)
        {
            GameObject.Destroy(visual);
        }
    }
}