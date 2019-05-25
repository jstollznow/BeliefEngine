using System;
using System.Collections.Generic;
public class Sentence{
    List<Sentence> subSentences = new List<Sentence>();
    List<Operator> joins = new List<Operator>();
    string input;
    public Sentence(string input)
    {
        this.input = input;
        smartSort();
    }
    public Sentence(){}
    public void smartSort()
    {
        int i = 0;
        if (input != null)
        {
            while (i < input.Length && i != -1)
            {
                if (char.IsLetter(input[i]) == false)
                {
                    switch (input[i])
                    {
                        // declares everything in the brackets as a new sentence
                        case '(':
                            i = bracket(i);
                            break;
                        // handles operators (all two characters long)
                        case '&':
                        case '-':
                        case '<':
                        case '|':
                            joins.Add(new Operator(input.Substring(i,2)));
                            if (joins[joins.Count-1].OpName=="invalid")
                            {
                                i = -1;
                            }
                            else 
                            {
                                i = i + 2;
                            }
                            break;
                        // still need to manage negation!
                    }
                    if (i == -1)
                    {
                        Console.WriteLine("This sentence is invalid");
                        return;
                    }
                }
                else
                {
                    // if char is a letter, add a subsetence which is a proposition
                    subSentences.Add(new Proposition(input[i]));
                    i = i + 1;
                }

            }

        }
    }
    
    public int bracket(int start)
    // handles finding the subsentence where brackets are used or required
    {
        int lCount = 1;
        int lFirst = start;
        int rCount = 0;
        int rLast = -1;
        for (int i = start + 1; i < input.Length; i++)
        {
            if (char.IsLetter(input[i]) == false)
            {
                switch (input[i])
                {
                    case '(':
                        lCount++;
                        break;
                    case ')':
                        rCount++;
                        rLast=i;
                        break;

                }
                if (lCount == rCount)
                {
                    // make sub-sentence
                    subSentences.Add(new Sentence(
                        input.Substring(lFirst + 1,rLast - lFirst - 1)));
                    return (rLast + 1);
                }

            }

        }
        return -1;
    }

    public virtual string printString()
    {
        string sent = null;
        for (int i =0; i < joins.Count; i ++)
        {
            sent = sent + "(" + subSentences[i].printString() + ")" + joins[i].printString();
        }
        sent = sent + "(" + subSentences[subSentences.Count-1].printString() + ")";
        return sent;
    }
}