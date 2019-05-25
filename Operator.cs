public class Operator{
    string opName;

    int functionCall = -1;
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
            this.OpName = "not";
            this.functionCall = 0;
        }
        else if (val == "&&")
        {
            this.OpName = "and";
            this.functionCall = 1;
        }
        else if (val == "||")
        {
            this.OpName = "or";
            this.functionCall = 2;
        }
        else if (val == "->")
        {
            this.OpName = "implication";
            this.functionCall = 3;
        }
        else if (val == "<>")
        {
            this.OpName = "biconditional";
            this.functionCall = 4;
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

    public bool operate(bool x, bool y = false)
    {
        bool result; 
        switch (functionCall)
        {
            case 0:
                result = PropLogicRules.not(x);
            break;
            case 1:
                result = PropLogicRules.AND(x, y);
            break;
            case 2:
                result = PropLogicRules.OR(x, y);
            break;
            case 3:
                result = PropLogicRules.implication(x, y);
            break;
            case 4:
                result = PropLogicRules.biconditional(x, y);
            break;
            default:
                result = false;
            break;
        }
        return result;
    }
}