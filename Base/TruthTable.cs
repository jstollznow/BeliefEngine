using System.Collections.Generic;
using System;
/* Generates a truth table with the propositions in the knowledge base and operations involved */
public class TruthTable
{
    TruthElement[,] table;
    List<Proposition> invovledProps;

    public TruthElement[,] Table { get => table; set => table = value; }
    public List<Proposition> InvovledProps { get => invovledProps; set => invovledProps = value; }

    public TruthTable(List<Sentence> sentences)
    {
        invovledProps = new List<Proposition>();
        GenerateTable(sentences);

    }
    
    public void GenerateTable(List<Sentence> sentences, Sentence newSentence = null)
    {
        if (sentences.Count != 0)
        {
            updateInvolvedProps(sentences, newSentence);
            List<bool[]> boolValues = new List<bool[]>();
            boolValues = generateBoolValues(InvovledProps.Count);
            int rows = boolValues.Count;
            int cols = sentences.Count;
            table = new TruthElement[rows,cols];
            // generate condition states
            for (int i = 0; i < boolValues.Count; i++)
            {
                GlobalProps.setPropositions(boolValues[i],InvovledProps);
                for (int j = 0; j < sentences.Count; j++)
                {
                    Table[i,j] = new TruthElement();
                    Table[i,j].Value = sentences[j].getValue();
                    Table[i,j].Origin = makeOriginString(sentences[j]);
                    Table[i,j].PropValues = boolValues[i];
                }
            }
        }
    }

    public List<bool[]> valuesToMatch(bool value)
    {
        List<bool[]> criticalValues = new List<bool[]>();
        for (int row = 0; row < table.GetLength(0); row++)
        {
            bool criticalRow = true;
            for (int i = 0; i < table.GetLength(1); i++)
            {
                if (table[row,i].Value != value)
                {
                    criticalRow = false;
                    break;
                }
            }
            if (criticalRow)
            {
                criticalValues.Add(table[row,0].PropValues);
            }
        }
        return criticalValues;
    }
    
    public int sentenceCheck ()
    {
        bool taut = true;
        bool falsum = true;
        List<bool[]> criticalValues = new List<bool[]>();
        for (int col = 0; col < table.GetLength(1); col++)
        {
            for (int row = 0; row < table.GetLength(0); row++)
            {
                if (table[row, col].Value == false)
                {
                    taut = false;
                }
                else 
                {
                    falsum = false;
                }
            }
        }
        if (!taut && !falsum)
        {
            return -1;
        }
        else if (taut)
        {
            return 1;
        }
        else if (falsum)
        {
            return 0;
        }
        else
        {
            return -2;
        }
    }
    public List<bool[]> sentTaut()
    {
        List<bool[]> criticalValues = new List<bool[]>();
        for (int row = 0; row < table.GetLength(0); row++)
        {
            bool criticalRow = true;
            for (int i = 0; i < table.GetLength(1); i++)
            {
                if (table[row, i].Value == false)
                {
                    criticalRow = false;
                    break;
                }
            }
            if (criticalRow)
            {
                criticalValues.Add(table[row, 0].PropValues);
            }
        }
        return criticalValues;
    }
    private string makeOriginString(Sentence sentence)
    {
        string orgStr = "";

        foreach (Proposition prop in InvovledProps)
        {
            orgStr = orgStr + prop.Name + ": ";
            if (prop.Value)
            {
                orgStr = orgStr + "T, ";
            }
            else
            {
                orgStr = orgStr + "F, ";
            }
        }
        orgStr = orgStr + "Sentence: " + sentence.printString();
        return orgStr;
    }

    private List<bool[]> generateBoolValues(int numProps)
    {
        // like a bit counter
        List<bool[]> values = new List<bool[]>();
        bool[] firstEntry = new bool[numProps];
        for (int i = 0; i < numProps; i++)
        {
            firstEntry[i] = false;
        }
        values.Add(firstEntry);
        for (int i = 1; i < Math.Pow(2, numProps); i++)
        {
            bool[] newEntry = new bool[numProps];
            Array.Copy(values[i - 1], newEntry, numProps);
            // newEntry = values[i - 1];
            if (newEntry[numProps - 1])
            {
                newEntry[numProps - 1] = false;
                for (int j = numProps - 2; j >= 0; j--)
                {
                    if (newEntry[j] == false)
                    {
                        newEntry[j] = true;
                        for (int carry = j + 1; carry < numProps; carry++)
                        {
                            newEntry[carry] = false;
                        }
                        break;
                    }
                }
            }
            else
            {
                newEntry[numProps - 1] = true;
            }
            values.Add(newEntry);
        }
        return values;
    }

    private void updateInvolvedProps(List<Sentence> sentences, Sentence newSentence)
    {
        if (newSentence != null)
        {
            sentences.Add(newSentence);
        }
        for (int i = 0; i < sentences.Count; i++)
        {
            // for each proposition in each sentence
            for (int j = 0; j < sentences[i].PropsInSentence.Count; j++)
            {
                // check if it is already in the propositions for the truth table
                if (InvovledProps.Contains(sentences[i].PropsInSentence[j]) == false)
                {
                    // if not, add it
                    InvovledProps.Add(sentences[i].PropsInSentence[j]);
                }
            }
        }
        if (newSentence != null)
        {
            sentences.Remove(newSentence);
        }
    }
}