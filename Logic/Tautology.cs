

public class Tautology:Proposition
{
    public Tautology(Sentence parent):base('T',parent)
    {
        this.Value = true;
    }
}