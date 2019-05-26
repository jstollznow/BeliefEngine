using System.Collections.Generic;
using System;
public class TruthTable
{
    TruthElement[,] table;
    List<Proposition> invovledProps;
    public TruthTable(List<Sentence> sentences)
    {
        
    }
    
    public void GenerateTable(List<Sentence> sentences)
    {
        updateInvolvedProps(sentences);
        int rows = invovledProps.Count - 1;
        int cols = sentences.Count - 1;
        table = new TruthElement[rows,cols];
        // generate condition states


    }

    public void generateBoolValues (int numProps)
    {
        // like a bit counter
        List<bool[]> values = new List<bool[]>(); 
        values[0] = new bool[]{false, false, false};
        for (int i = 1; i < Math.Pow(2,numProps); i++)
        {
            bool[] newEntry = new bool[numProps];
            newEntry = values[i-1];
            if (newEntry[0])
            {
                newEntry[0] = false;
                for (int j = 1; j < numProps; j++)
                {
                    if (newEntry[j] == false)
                    {
                        newEntry[j] = true;
                        for (int carry = j; carry >= 0; carry--)
                        {
                            newEntry[carry] = false;
                        }
                        break;
                    }
                } 
            }
            else
            {
                newEntry[0] = true;
            }
            values[i] = newEntry;
        }
    }
    public void updateInvolvedProps(List<Sentence> sentences)
    {
        for (int i = 0; i < sentences.Count; i++)
        {
            // for each proposition in each sentence
            for (int j = 0; j < sentences[i].PropsInSentence.Count; j++)
            {
                // check if it is already in the propositions for the truth table
                if (invovledProps.Contains(sentences[i].PropsInSentence[j]) == false)
                {
                    // if not, add it
                    invovledProps.Add(sentences[i].PropsInSentence[j]);
                }
            }
        }
    }
}