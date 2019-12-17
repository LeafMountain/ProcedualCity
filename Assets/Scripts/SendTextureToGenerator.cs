using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTextureToGenerator : MonoBehaviour
{
    public Texture2D image = null;
    public int pixelsPerTile = 16;
    public bool startWithGround = false;
    public int groundTileIndex = 0;
    public Vector3Int size = new Vector3Int(30, 20, 1);

    public Vector2Int middleOffset;
    public Vector2Int horizontalOffset;
    public Vector2Int verticalOffset;

    public bool sendOnStart = false;
    
    private void Start()
    {
        if(sendOnStart == true)
        {
            Send();
        }
    }

    public void Send()
    {
        if(image != null)
        {
            Board.Instance.Regenerate(TemplateGenerator.GenerateTemplate(image, pixelsPerTile, middleOffset, horizontalOffset, verticalOffset), size, startWithGround, groundTileIndex);
        }
    }
}
