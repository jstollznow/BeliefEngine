using System.Collections.Generic;
public static class GlobalProps
{
    public static List<Proposition> props;

    public static string propositions()
    {
        string arguements = string.Empty;
        for (int i = 0; i < props.Count; i++)
        {
            arguements = arguements + props[i].Name + " ";
        }
        return arguements;
    }
    public static void setPropositions(bool[] values)
    {
        for (int i = 0; i < props.Count; i++)
        {
            props[i].Value = values[i];
        }
    }
}