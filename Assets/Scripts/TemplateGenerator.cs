using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemplateGenerator : MonoBehaviour
{
    public Texture2D referenceImage;
    public Vector2Int gridSize = Vector2Int.one;
    public int pixelsPerTile = 16;
    public bool debug = false;

    public List<PatternTemplate> templates = new List<PatternTemplate>();

    void Start()
    {
        if(debug == true)
        {
            GenerateTemplate();
        }
    }

    public PatternTemplate[] GenerateTemplate()
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

                // if(debug)
                // {
                //     GameObject tileGO = new GameObject();
                //     tileGO.AddComponent<SpriteRenderer>().sprite = tileSprite;
                //     tileGO.transform.position = new Vector2(x, y);
                // }
            }
        }

        // Create the pattern templates
        for (int i = 0; i < tileSprites.Count; i++)
        {
            PatternTemplate pattern = PatternTemplate.CreateInstance<PatternTemplate>();
            pattern.name = i.ToString();
            Sprite tileSprite = tileSprites[i];
            pattern.sprite = tileSprite;

            templates.Add(pattern); 
        }


        // Setup neighbors
        for (int j = 0; j < templates.Count; j++)
        {
            PatternTemplate currentTemplate = templates[j];
            Vector2Int currentUpMiddlePixelPosition = new Vector2Int((int)currentTemplate.sprite.textureRect.center.x, (int)currentTemplate.sprite.textureRect.max.y - 1);
            Vector2Int currentUpLeftPixelPosition = new Vector2Int((int)currentTemplate.sprite.textureRect.min.x, (int)currentTemplate.sprite.textureRect.max.y - 1);
            Vector2Int currentUpRightPixelPosition = new Vector2Int((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.max.y - 1);
            Color upMiddlePixel = currentTemplate.sprite.texture.GetPixel(currentUpMiddlePixelPosition.x, currentUpMiddlePixelPosition.y);
            Color upLeftPixel = currentTemplate.sprite.texture.GetPixel(currentUpLeftPixelPosition.x, currentUpLeftPixelPosition.y);
            Color upRightPixel = currentTemplate.sprite.texture.GetPixel(currentUpRightPixelPosition.x, currentUpRightPixelPosition.y);

            // currentTemplate.sprite.texture.SetPixel(currentUpRightPixelPosition.x, currentUpRightPixelPosition.y, Color.red);
            // currentTemplate.sprite.texture.SetPixel(currentUpMiddlePixelPosition.x, currentUpMiddlePixelPosition.y, Color.red);
            // currentTemplate.sprite.texture.SetPixel(currentUpLeftPixelPosition.x, currentUpLeftPixelPosition.y, Color.red);

            for (int i = 0; i < templates.Count; i++)
            {
                // Check if top middle pixel fits
                {
                    Color neighborDownMiddlePixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.center.x, (int)templates[i].sprite.textureRect.min.y);
                    // currentTemplate.sprite.texture.SetPixel((int)templates[i].sprite.textureRect.center.x, (int)templates[i].sprite.textureRect.min.y, Color.red);

                    Color neighborDownLeftPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.min.y);
                    // currentTemplate.sprite.texture.SetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.min.y, Color.red);

                    Color neighborDownRightPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.max.x - 1, (int)templates[i].sprite.textureRect.min.y);
                    // currentTemplate.sprite.texture.SetPixel((int)templates[i].sprite.textureRect.max.x - 1, (int)templates[i].sprite.textureRect.min.y, Color.red);

                    // currentTemplate.sprite.texture.Apply();

                    // You only need to check up and right since you are filling in the neighbors at the same time

                    if(upMiddlePixel == neighborDownMiddlePixel 
                    && upLeftPixel == neighborDownLeftPixel 
                    && upRightPixel == neighborDownRightPixel
                    )
                    {
                        if(currentTemplate.upNeighbors.Contains(templates[i]) == false)
                        {
                            currentTemplate.upNeighbors.Add(templates[i]);
                            templates[i].downNeighbors.Add(currentTemplate);
                        }
                    }
                }

                // Check if right middle pixel fits
                {
                    Color rightMiddlePixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.center.y);
                    Color neighborleftMiddlePixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.center.y);
                    // currentTemplate.sprite.texture.SetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.center.y, Color.magenta);
                    // currentTemplate.sprite.texture.SetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.center.y, Color.magenta);


                    Color rightTopPixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.max.y - 1);
                    Color neighborleftTopPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.max.y - 1);
                    // currentTemplate.sprite.texture.SetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.max.y - 1, Color.magenta);
                    // currentTemplate.sprite.texture.SetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.max.y - 1, Color.magenta);



                    Color rightBottomPixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.min.y);
                    Color neighborleftBottomPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.min.y);
                    // currentTemplate.sprite.texture.SetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.min.y, Color.magenta);
                    // currentTemplate.sprite.texture.SetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.min.y, Color.magenta);


                    // You only need to check up and right since you are filling in the neighbors at the same time

                    // BUGG: Checking the whole texture not just the middle of the sprite
                    if(rightMiddlePixel == neighborleftMiddlePixel 
                    &&
                    rightTopPixel == neighborleftTopPixel &&
                    rightBottomPixel == neighborleftBottomPixel
                    )    // Check more pixels, not just the middle
                    {
                        currentTemplate.rightNeighbors.Add(templates[i]);
                        templates[i].leftNeighbors.Add(currentTemplate);
                    }
                }
            }
        }

        if(debug)
        {
            for (int i = 0; i < templates.Count; i++)       //// THIS IS NOT SPAWNING CORRECTLY
            {
                // GameObject go = new GameObject();
                // go.AddComponent<SpriteRenderer>().sprite = templates[i].sprite;
                // go.transform.position = new Vector2(0, i * 2);

                for (int j = 0; j < templates[i].upNeighbors.Count; j++)
                {
                    GameObject go = new GameObject();
                    go.AddComponent<SpriteRenderer>().sprite = templates[i].sprite;
                    go.transform.position = new Vector2(j, i * 3);

                    GameObject go2 = new GameObject();
                    go2.AddComponent<SpriteRenderer>().sprite = templates[i].sprite;
                    go2.transform.position = new Vector2(j, 1 + i * 3);
                }
            }
        }

        return templates.ToArray();
    }
}
