using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemplateGenerator : MonoBehaviour
{
    public Texture2D referenceImage;
    public Vector2Int gridSize = Vector2Int.one;
    public int pixelsPerTile = 16;

    private List<Color[]> Tiles = null;
    private List<PatternTemplate> templates = new List<PatternTemplate>();

    void Start()
    {
        GenerateTemplate();
    }

    private void GenerateTemplate()
    {
        // Separate the tiles
        List<Sprite> tileSprites = new List<Sprite>();
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector2Int currentPosition = new Vector2Int(x * pixelsPerTile, y * pixelsPerTile);
                Sprite tileSprite = Sprite.Create(referenceImage, new Rect(currentPosition.x, currentPosition.y, pixelsPerTile, pixelsPerTile), new Vector2(0.5f, 0.5f), pixelsPerTile);
                tileSprites.Add(tileSprite);

                GameObject tileGO = new GameObject();
                tileGO.AddComponent<SpriteRenderer>().sprite = tileSprite;
                tileGO.transform.position = new Vector2(x, y);
            }
        }

        // Create the pattern templates
        for (int i = 0; i < tileSprites.Count; i++)
        {
            PatternTemplate pattern = PatternTemplate.CreateInstance<PatternTemplate>();
            Sprite tileSprite = tileSprites[i];
            pattern.sprite = tileSprite;

            templates.Add(pattern); 
        }


        // Setup neighbors
        for (int j = 0; j < templates.Count; j++)
        {
            PatternTemplate currentTemplate = templates[j];

            for (int i = 0; i < templates.Count; i++)
            {
                // Check if top middle pixel fits
                Color upMiddlePixel = currentTemplate.sprite.texture.GetPixel(currentTemplate.sprite.texture.width / 2, currentTemplate.sprite.texture.height);
                Color neighborDownMiddlePixel = templates[i].sprite.texture.GetPixel(templates[i].sprite.texture.width / 2, 0);

                // You only need to check up and right since you are filling in the neighbors at the same time
            
                if(upMiddlePixel == neighborDownMiddlePixel)    // Check more pixels, not just the middle
                {
                    // DONT USE 0!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    currentTemplate.upNeighbors[0] = templates[i];
                    templates[i].downNeighbors[0] = currentTemplate;
                }
            }
        }
    }
}
