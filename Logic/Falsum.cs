

public class Falsum:Proposition
{
    public Falsum(Sentence parent):base('F',parent)
    {
        this.Value = false;
    }
}