public class Proposition:Sentence{
    bool value;
    string name;
    public Proposition(string input):base(){
        this.name = input;
        // this.value=value;
    }
    public override string printString(){
        return name;
    }
}