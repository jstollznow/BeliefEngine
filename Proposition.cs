public class Proposition:Sentence{
    bool value;
    char name;
    public Proposition(char input):base()
    {
        this.name = input;
    }
    public override string printString()
    {
        return name.ToString();
    }
}