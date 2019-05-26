
public class TruthElement
{
    bool value;
    string origin;
    bool[] propValues;
    public TruthElement(){}

    public bool Value { get => value; set => this.value = value; }
    public string Origin { get => origin; set => origin = value; }
    public bool[] PropValues { get => propValues; set => propValues = value; }
}