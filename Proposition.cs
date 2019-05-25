public class Proposition:Sentence{
    bool value;
    char name;
    public Proposition(char input):base()
    {
        this.name = input;
    }

    public char Name { get => name; set => name = value; }
    public bool Value { get => value; set => this.value = value; }

    public override string printString()
    {
        return this.Name.ToString();
    }

    public override bool getValue()
    {
       return this.Value;
    }
}