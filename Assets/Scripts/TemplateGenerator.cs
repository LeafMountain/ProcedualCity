using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Generator : MonoBehaviour
{
    public virtual Module[] GenerateTemplate() { return null; }
}

public class TemplateGenerator : Generator
{
    public Texture2D referenceImage;
    public int pixelsPerTile = 16;
    public bool debug = false;
    public Vector2Int middleCheckOffset = Vector2Int.zero;
    public Vector2Int horizontalOffset = Vector2Int.zero;
    public Vector2Int verticalOffest = Vector2Int.zero;

    public List<Module> templates = new List<Module>();

    void Start()
    {
        if(debug == true)
        {
            GenerateTemplate();
            VisualDebug();
        }
    }

    public override Module[] GenerateTemplate()
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
            Module pattern = Module.CreateInstance<Module>();
            pattern.name = i.ToString();
            Sprite tileSprite = tileSprites[i];
            pattern.sprite = tileSprite;

            templates.Add(pattern); 
        }

        // Create the identifiers
        for (int i = 0; i < templates.Count; i++)
        {
            Module currentTemplate = templates[i];

            // Up side
            Color upMiddlePixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.center.x + middleCheckOffset.x, (int)currentTemplate.sprite.textureRect.max.y - 1);
            Color upLeftPixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.min.x + verticalOffest.x, (int)currentTemplate.sprite.textureRect.max.y - 1);
            Color upRightPixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1 + verticalOffest.y, (int)currentTemplate.sprite.textureRect.max.y - 1);
            currentTemplate.upIdentifier = Animator.StringToHash(upMiddlePixel.ToString() + upLeftPixel.ToString() + upRightPixel.ToString());

            // Right side
            Color rightMiddlePixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.center.y + middleCheckOffset.y);
            Color rightTopPixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.max.y - 1 + horizontalOffset.x);
            Color rightBottomPixel = currentTemplate.sprite.texture.GetPixel((int)currentTemplate.sprite.textureRect.max.x - 1, (int)currentTemplate.sprite.textureRect.min.y + horizontalOffset.y);
            currentTemplate.rightIdentifier = Animator.StringToHash(rightMiddlePixel.ToString() + rightTopPixel.ToString() + rightBottomPixel.ToString());

            // Down side
            Color neighborDownMiddlePixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.center.x + middleCheckOffset.x, (int)templates[i].sprite.textureRect.min.y);
            Color neighborDownLeftPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x + verticalOffest.x, (int)templates[i].sprite.textureRect.min.y);
            Color neighborDownRightPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.max.x - 1 + verticalOffest.y, (int)templates[i].sprite.textureRect.min.y);
            templates[i].downIdentifier = Animator.StringToHash(neighborDownMiddlePixel.ToString() + neighborDownLeftPixel.ToString() + neighborDownRightPixel.ToString());

            // Left side
            Color neighborleftMiddlePixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.center.y + middleCheckOffset.y);
            Color neighborleftTopPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.max.y - 1 + horizontalOffset.x);
            Color neighborleftBottomPixel = templates[i].sprite.texture.GetPixel((int)templates[i].sprite.textureRect.min.x, (int)templates[i].sprite.textureRect.min.y + horizontalOffset.y);
            templates[i].leftIdentifier = Animator.StringToHash(neighborleftMiddlePixel.ToString() + neighborleftTopPixel.ToString() + neighborleftBottomPixel.ToString());
        }


        // Setup neighbors
        for (int j = 0; j < templates.Count; j++)
        {
            Module currentTemplate = templates[j];
            
            for (int i = 0; i < templates.Count; i++)
            {
                if(currentTemplate.upIdentifier == templates[i].downIdentifier)
                {
                    if(currentTemplate.upNeighbors.Contains(templates[i]) == false)
                    {
                        currentTemplate.upNeighbors.Add(templates[i]);
                        templates[i].downNeighbors.Add(currentTemplate);
                    }
                }

                if(currentTemplate.rightIdentifier == templates[i].leftIdentifier)
                {
                    currentTemplate.rightNeighbors.Add(templates[i]);
                    templates[i].leftNeighbors.Add(currentTemplate);
                }
            }
        }


        Debug.Log("Template done");
        return templates.ToArray();
    }

    private void VisualDebug()
    {
        for (int i = 0; i < templates.Count; i++)
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
}
