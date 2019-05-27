static class PropLogicRules
{
    public static bool not(bool x)
    {
        return (!x);
    }
    public static bool AND(bool x, bool y)
    {
        return(x && y);
    }
    public static bool OR(bool x, bool y)
    {
        return(x || y);
    }
    public static bool implication(bool x, bool y)
    {
        return (not(x) || y);
    }
    public static bool biconditional(bool x, bool y)
    {
        return (implication(x,y) && implication(y,x));
    }
    
}