[System.Serializable]
public class Pattern
{
    public PatternTemplate template = null;
    public bool allowed = true;

    public Pattern(PatternTemplate template)
    {
        this.template = template;
    }
}