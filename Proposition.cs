public class Proposition:Sentence{
    bool value;
    string name;
    public Proposition(bool value, string input):base(input){
        this.value=value;
    }
    public override string printString(){
        return name;
    }
}