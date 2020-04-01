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
    public static void setPropositions(bool[] values,List<Proposition> selectedProps)
    {
        for (int i = 0; i < selectedProps.Count; i++)
        {
            selectedProps[i].Value = values[i];
        }
    }
    // these functions and this class is used for the letters in the sentences
    
}