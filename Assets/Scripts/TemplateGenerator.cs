using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemplateGenerator : MonoBehaviour
{
    public Texture2D referenceImage;
    public int pixelsPerTile = 16;
    public bool debug = false;
    public Vector2Int middleCheckOffset = Vector2Int.zero;
    public Vector2Int horizontalOffset = Vector2Int.zero;
    public Vector2Int verticalOffest = Vector2Int.zero;

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
        Vector2Int gridSize = Vector2Int.zero;
        gridSize.x = referenceImage.width / pixelsPerTile;
        gridSize.y = referenceImage.height / pixelsPerTile;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector2Int currentPosition = new Vector2Int(x * pixelsPerTile, y * pixelsPerTile);
                Sprite tileSprite = Sprite.Create(referenceImage, new Rect(currentPosition.x, currentPosition.y, pixelsPerTile, pixelsPerTile), new Vector2(0.5f, 0.5f), pixelsPerTile);
                tileSprites.Add(tileSprite);
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
            Vector2Int currentUpMiddlePixelPosition = new Vector2Int((int)currentTemplate.sprite.textureRect.center.x + middleCheckOffset.x, (int)currentTemplate.sprite.textureRect.max.y - 1);
            Vector2Int currentUpLeftPixelPosition = new Vector2Int((int)currentTemplate.sprite.textureRect.min.x + verticalOffest.x, (int)currentTemplate.sprite.textureRect.max.y - 1);
            Vector2Int currentUpRightPixelPosition = new Vector2Int((int)currentTemplate.sprite.textureRect.max.x - 1 + verticalOffest.y, (int)currentTemplate.sprite.textureRect.max.y - 1);
            Color upMiddlePixel = currentTemplate.sprite.texture.GetPixel(currentUpMiddlePixelPosition.x, currentUpMiddlePixelPosition.y);
            Color upLeftPixel = currentTemplate.sprite.texture.GetPixel(currentUpLeftPixelPosition.x, currentUpLeftPixelPosition.y);
            Color upRightPixel = currentTemplate.sprite.texture.GetPixel(currentUpRightPixelPosition.x, currentUpRightPixelPosition.y);
            
            currentTemplate.upIdentifier = Animator.StringToHash(upMiddlePixel.ToString() + upLeftPixel.ToString() + upRightPixel.ToString());

            Color rightMiddlePixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.center.y + middleCheckOffset.y);
            Color rightTopPixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.max.y - 1 + horizontalOffset.x);
            Color rightBottomPixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.min.y + horizontalOffset.y);

            currentTemplate.rightIdentifier = Animator.StringToHash(rightMiddlePixel.ToString() + rightTopPixel.ToString() + rightBottomPixel.ToString());
            
            for (int i = 0; i < templates.Count; i++)
            {
                {
                    if(templates[i].downIdentifier == 0)
                    {
                        Color neighborDownMiddlePixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.center.x + middleCheckOffset.x, (int)templates[i].sprite.textureRect.min.y);
                        Color neighborDownLeftPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x + verticalOffest.x, (int)templates[i].sprite.textureRect.min.y);
                        Color neighborDownRightPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.max.x - 1 + verticalOffest.y, (int)templates[i].sprite.textureRect.min.y);
                        templates[i].downIdentifier = Animator.StringToHash(neighborDownMiddlePixel.ToString() + neighborDownLeftPixel.ToString() + neighborDownRightPixel.ToString());
                    }
                    

                    if(currentTemplate.upIdentifier == templates[i].downIdentifier)
                    {
                        if(currentTemplate.upNeighbors.Contains(templates[i]) == false)
                        {
                            currentTemplate.upNeighbors.Add(templates[i]);
                            templates[i].downNeighbors.Add(currentTemplate);
                        }
                    }
                }

                {
                    if(templates[i].leftIdentifier == 0)
                    {
                        Color neighborleftMiddlePixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.center.y + middleCheckOffset.y);
                        Color neighborleftTopPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.max.y - 1 + horizontalOffset.x);
                        Color neighborleftBottomPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.min.y + horizontalOffset.y);
                        templates[i].leftIdentifier = Animator.StringToHash(neighborleftMiddlePixel.ToString() + neighborleftTopPixel.ToString() + neighborleftBottomPixel.ToString());
                    }

                    // You only need to check up and right since you are filling in the neighbors at the same time

                    if(currentTemplate.rightIdentifier == templates[i].leftIdentifier)
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
                    go2.AddComponent<SpriteRenderer>().sprite = templates[i].upNeighbors[j].sprite;
                    go2.transform.position = new Vector2(j, 1 + i * 3);
                }
            }
        }

        Debug.Log("Template done");
        return templates.ToArray();
    }
}
