using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;
    public PatternTemplate[] patterns = null;
    public TemplateGenerator templateGenerator = null;
    public float timeBetweenCollapse = 0;

    [Header("First Tile")]
    public bool use = false;
    public Vector2Int position = Vector2Int.zero;
    public int templateIndex = 0;

    private Tile[,] tiles = null;

    private void Start()
    {
        patterns = templateGenerator.GenerateTemplate();
        foreach(PatternTemplate pattern in patterns)
        {
            pattern.timesUsed = 0;
        }

        // Initialize
        tiles = new Tile[size.x, size.y];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                tiles[x, y] = new Tile();
                tiles[x, y].position = new Vector2Int(x, y);
                tiles[x, y].AddPatternTemplates(patterns);
            }
        }

        // Pick first tile
        if(use == true)
        {
            tiles[0,0].PickTileManually(templateIndex);
            Propagate(tiles[position.x, position.y]);
        }
        // int maxJ = Random.Range(1, size.y / 2);
        // for (int i = 0; i < size.x; i++)
        // {
        //     for (int j = 0; j < Random.Range(1, size.y / 2); j++)
        //     {
        //         tiles[i, j].PickTileManually(0);
        //         Propagate(tiles[i, j]);
        //     }
        // }
        Go();
    }

    private void Go()
    {
        if(timeBetweenCollapse == 0)
        {
            DoYourStuff();
        }
        else
        {
            StartCoroutine(Go2());
        }
    }

    IEnumerator Go2()
    {
        // yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(timeBetweenCollapse);
        DoYourStuff();
    }

    private void DoYourStuff()
    {
        Tile tile = GetLowestAllowedCount();
        if(tile == null)
        {
            Debug.Log("COLLAPSE DONE");
        }
        else
        {
            tile.Collapse();
            Propagate(tile);
            Go();
        }
    }

    private void Propagate(Tile tile)
    {
        if(tile.position.y + 1 < size.y)
        {
            Tile upTile = tiles[tile.position.x, tile.position.y + 1];
            if(upTile.GetAllowedPatternsCount() > 0)
                upTile.SyncWithNeighbor(tile, Tile.Direction.down);
        }
        if(tile.position.x + 1 < size.x)
        {
            Tile rightTile = tiles[tile.position.x + 1, tile.position.y];
            if(rightTile.GetAllowedPatternsCount() > 0)
                rightTile.SyncWithNeighbor(tile, Tile.Direction.left);
        }
        if(tile.position.y - 1 >= 0)
        {
            Tile downTile = tiles[tile.position.x, tile.position.y - 1];
            if(downTile.GetAllowedPatternsCount() > 0)
                downTile.SyncWithNeighbor(tile, Tile.Direction.up);
        }
        if(tile.position.x - 1 >= 0)
        {
            Tile leftTile = tiles[tile.position.x - 1, tile.position.y];
            if(leftTile.GetAllowedPatternsCount() > 0)
                leftTile.SyncWithNeighbor(tile, Tile.Direction.right);
        }
    }

    // Get the tiles with the lowest non zero count
    private Tile GetLowestAllowedCount()
    {
        Tile lowestAllowedCountTile = null;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if(lowestAllowedCountTile == null)
                {
                    if(tiles[x, y].GetAllowedPatternsCount() > 0)
                    {
                        lowestAllowedCountTile = tiles[x, y];
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    int nextAllowedCount = tiles[x, y].GetAllowedPatternsCount();
                    if(nextAllowedCount > 0 && lowestAllowedCountTile.GetAllowedPatternsCount() > nextAllowedCount)
                    {
                        lowestAllowedCountTile = tiles[x, y];
                    }
                }
            }
        }

        return lowestAllowedCountTile;
    }

}
