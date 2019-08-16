using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;

    private Tile[,] tiles = null;

    private void Start()
    {
        // Initialize
        tiles = new Tile[size.x, size.y];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                tiles[x, y] = new Tile();
                tiles[x, y].position = new Vector2Int(x, y);
            }
        }

        // Collapse
        // Tile tile = GetLowestAllowedCount();
        // tile.Collapse();
        // Propagate(tile);
    }

    private void Go()
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
        }
    }

    private void Propagate(Tile tile)
    {
        if(tile.position.y + 1 < size.y)
        {
            Tile upTile = tiles[tile.position.x, tile.position.y + 1];
            upTile.SyncWithNeighbor(tile, Tile.Direction.down);
        }
        if(tile.position.x + 1 < size.x)
        {
            Tile upTile = tiles[tile.position.x + 1, tile.position.y];
            upTile.SyncWithNeighbor(tile, Tile.Direction.left);
        }
        if(tile.position.y - 1 >= 0)
        {
            Tile upTile = tiles[tile.position.x, tile.position.y - 1];
            upTile.SyncWithNeighbor(tile, Tile.Direction.up);
        }
        if(tile.position.x - 1 >= 0)
        {
            Tile upTile = tiles[tile.position.x - 1, tile.position.y];
            upTile.SyncWithNeighbor(tile, Tile.Direction.right);
        }
    }

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

                int nextAllowedCount = tiles[x, y].GetAllowedPatternsCount();
                if(nextAllowedCount > 0 && lowestAllowedCountTile.GetAllowedPatternsCount() > nextAllowedCount)
                {
                    lowestAllowedCountTile = tiles[x, y];
                }
            }
        }

        return lowestAllowedCountTile;
    }

}
