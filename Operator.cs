public class Operator{
    string opName;
    string op;
    public Operator(string val)
    {
        this.op = val;
        // ~ not
        // && ..and..
        // || ..or..
        // -> implication
        // <-> biconditional
        if (val == "~")
        {
            this.opName ="not";
        }
        else if (val == "&&")
        {
            this.opName = "and";
        }
        else if (val == "||")
        {
            this.opName = "or";
        }
        else if (val == "->")
        {
            this.opName = "implication";
        }
        else if (val == "<->")
        {
            this.opName = "biconditional";
        }
        else
        {
            this.opName = "invalid";
        }
    }
    public string printString(){
        return op;
    }
}