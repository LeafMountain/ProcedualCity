using UnityEngine;

[System.Serializable]
public class Pattern
{
    public Module template = null;
    public bool allowed = true;

    public Pattern(Module template)
    {
        this.template = template;
    }

    public GameObject GetVisual()
    {
        GameObject visual = new GameObject();
        if(template.sprite != null)
        {
            visual.AddComponent<SpriteRenderer>().sprite = template.sprite;
        }
        else
        {
            visual.AddComponent<MeshFilter>().mesh = template.mesh;
            visual.AddComponent<MeshRenderer>().material = null;
        }

        visual.name = template.name;
        return visual;
    }
}