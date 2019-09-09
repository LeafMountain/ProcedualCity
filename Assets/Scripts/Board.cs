using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector3Int size = Vector3Int.one;
    public Module[] patterns = null;
    public Generator templateGenerator = null;
    public Material defaultMaterial = null;

    [Header("Other")]

    public float timeBetweenCollapse = 0;

    [Header("First Tile")]
    public bool pickFirstTile = false;
    public Vector3Int position = Vector3Int.zero;
    public int templateIndex = 0;
    public bool startWithGround = false;

    private Slot[,,] slots = null;

    private void Start()
    {
        patterns = templateGenerator.GenerateTemplate();
        foreach(Module pattern in patterns)
        {
            pattern.timesUsed = 0;
        }
        
        // Initialize
        slots = new Slot[size.x, size.y, size.z];
        for (int z = 0; z < size.z; z++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    slots[x, y, z] = new Slot();
                    slots[x, y, z].position = new Vector3Int(x, y, z);
                    slots[x, y, z].AddPatternTemplates(patterns);
                }
            }
        }

        if(pickFirstTile == true)
        {
            slots[position.x, position.y, position.z].PickTileManually(templateIndex);
            Propagate(slots[position.x, position.y, position.z]);
        }

        if(startWithGround == true)
        {
            FillGround();
        }

        Go();
    }

    public void FillGround()
    {
        int maxJ = Random.Range(1, size.y / 2);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < Random.Range(1, size.y / 2); j++)
            {
                slots[i, j, 0].PickTileManually(0);
                Propagate(slots[i, j, 0]);
            }
        }
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
        yield return new WaitForSeconds(timeBetweenCollapse);
        DoYourStuff();
    }

    private void DoYourStuff()
    {
        Slot tile = GetLowestAllowedCount();
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

    private void Propagate(Slot slot)
    {
        if(slot.position.y + 1 < size.y)
        {
            Slot upSlot = slots[slot.position.x, slot.position.y + 1, slot.position.z];
            if(upSlot.GetAllowedPatternsCount() > 0)
                upSlot.SyncWithNeighbor(slot, Direction.down);
        }
        if(slot.position.x + 1 < size.x)
        {
            Slot rightSlot = slots[slot.position.x + 1, slot.position.y, slot.position.z];
            if(rightSlot.GetAllowedPatternsCount() > 0)
                rightSlot.SyncWithNeighbor(slot, Direction.left);
        }
        if(slot.position.y - 1 >= 0)
        {
            Slot downSlot = slots[slot.position.x, slot.position.y - 1, slot.position.z];
            if(downSlot.GetAllowedPatternsCount() > 0)
                downSlot.SyncWithNeighbor(slot, Direction.up);
        }
        if(slot.position.x - 1 >= 0)
        {
            Slot leftSlot = slots[slot.position.x - 1, slot.position.y, slot.position.z];
            if(leftSlot.GetAllowedPatternsCount() > 0)
                leftSlot.SyncWithNeighbor(slot, Direction.right);
        }
        if(slot.position.z + 1 < size.z)
        {
            Slot forwardSlot = slots[slot.position.x, slot.position.y, slot.position.z + 1];
            if(forwardSlot.GetAllowedPatternsCount() > 0)
                forwardSlot.SyncWithNeighbor(slot, Direction.back);
        }
        if(slot.position.z - 1 >= 0)
        {
            Slot backSlot = slots[slot.position.x, slot.position.y, slot.position.z - 1];
            if(backSlot.GetAllowedPatternsCount() > 0)
                backSlot.SyncWithNeighbor(slot, Direction.forward);
        }
    }

    // Get the tiles with the lowest non zero count
    private Slot GetLowestAllowedCount() {
        Slot lowestAllowedCountSlot = null;
        for (int z = 0; z < size.z; z++) {
            for (int y = 0; y < size.y; y++) {
                for (int x = 0; x < size.x; x++) {
                    if(lowestAllowedCountSlot == null) {
                        if(slots[x, y, z].GetAllowedPatternsCount() > 0) {
                            lowestAllowedCountSlot = slots[x, y, z];
                        }
                        else {
                            continue;
                        }
                    }
                    else {
                        int nextAllowedCount = slots[x, y, z].GetAllowedPatternsCount();
                        if(nextAllowedCount > 0 && lowestAllowedCountSlot.GetAllowedPatternsCount() > nextAllowedCount) {
                            lowestAllowedCountSlot = slots[x, y, z];
                        }
                    }
                }
            }
        }

        return lowestAllowedCountSlot;
    }
}
