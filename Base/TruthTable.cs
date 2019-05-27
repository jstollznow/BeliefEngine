using System.Collections.Generic;
using System;
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
    
    public void GenerateTable(List<Sentence> sentences)
    {
        if (sentences.Count != 0)
        {
            updateInvolvedProps(sentences);
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

    public List<bool[]> valuesToMatch()
    {
        List<bool[]> criticalValues = new List<bool[]>();
        for (int row = 0; row < table.GetLength(0); row++)
        {
            bool criticalRow = true;
            for (int i = 0; i < table.GetLength(1); i++)
            {
                if (table[row,i].Value == false)
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
        // for (int i = 0; i < values.Count; i++)
        // {
        //     for (int j = 0; j < numProps; j++)
        //     {
        //         Console.Write(values[i][j] + " ");
        //     }
        //     Console.Write(Environment.NewLine);
        // }
        return values;
    }

    private void updateInvolvedProps(List<Sentence> sentences)
    {
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
    }
}