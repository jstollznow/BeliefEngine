class Operator{
    string operation;
    Operator(string val)
    {
        // ~ not
        // && ..and..
        // || ..or..
        // -> implication
        // <-> biconditional
        if (val == "~")
        {
            this.operation="not";
        }
        else if (val == "&&")
        {
            this.operation = "and";
        }
        else if (val == "||")
        {
            this.operation = "or";
        }
        else if (val == "->")
        {
            this.operation = "implication";
        }
        else if (val == "<->")
        {
            this.operation = "biconditional";
        }
        else
        {
            this.operation = "invalid";
        }
    }
}