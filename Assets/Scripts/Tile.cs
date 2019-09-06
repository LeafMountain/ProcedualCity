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

        // Find the least used pattern
        // allowedPatterns.Sort(delegate(Pattern pat1, Pattern pat2){
        //     if(pat1.template.timesUsed < pat2.template.timesUsed)
        //     {
        //         return -1;
        //     }
        //     if(pat1.template.timesUsed > pat2.template.timesUsed)
        //     {
        //         return 1;
        //     }
        //     return 0;
        // });

        // int lastIndexOfLeastUsed = allowedPatterns.FindLastIndex(0, patternIndex => patternIndex == allowedPatterns[0]);
        int index = Random.Range(0, allowedPatterns.Count);

        finalPattern = allowedPatterns[index];
        finalPattern.template.timesUsed++;

        GameObject visual = new GameObject();
        visual.AddComponent<SpriteRenderer>().sprite = finalPattern.template.sprite;
        visual.transform.position = (Vector2)position;
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
                // Check if not contains
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
                // Check if not contains
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
                // Check if not contains
                if(patterns[i].template.leftNeighbors.Contains(neightborTile.finalPattern.template) == false)
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