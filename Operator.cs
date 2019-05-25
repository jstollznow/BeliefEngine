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
        // <> biconditional
        if (val == "~")
        {
            this.OpName ="not";
        }
        else if (val == "&&")
        {
            this.OpName = "and";
        }
        else if (val == "||")
        {
            this.OpName = "or";
        }
        else if (val == "->")
        {
            this.OpName = "implication";
        }
        else if (val == "<>")
        {
            this.OpName = "biconditional";
        }
        else
        {
            this.OpName = "invalid";
        }
    }

    public string OpName { get => opName; set => opName = value; }

    public string printString()
    {
        return op;
    }
}