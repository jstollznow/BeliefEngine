using System;
using System.Collections.Generic;
using static GlobalProps;
public class Sentence{
    List<Sentence> subSentences = new List<Sentence>();
    List<Operator> joins = new List<Operator>();

    bool not;
    bool nextNotVal = false;
    string input;

    public bool Not { get => not; set => not = value; }

    public Sentence(string input, bool not = false)
    {
        this.input = input;
        this.Not = not;
        smartSort();
    }
    public Sentence(){}
    public Sentence(char propName, bool not)
    {
        this.Not = not;
        Proposition newProp = null;
        for (int j = 0; j < props.Count; j++)
        {
            if (props[j].Name == propName)
            {
                newProp = props[j];
            }
        }
        if (newProp == null)
        {
            newProp = new Proposition(propName);
            nextNotVal = false;
            props.Add(newProp);
        }
        subSentences.Add(newProp);


    }
    public bool smartSort()
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
                            i = bracket(i,nextNotVal);
                            nextNotVal = false;
                            break;
                        // handles operators (all two characters long)
                        case '&':
                        case '-':
                        case '<':
                        case '|':
                            joins.Add(new Operator(input.Substring(i,2)));
                            if (joins[joins.Count-1].OpName == "invalid")
                            {
                                i = -1;
                            }
                            else 
                            {
                                i = i + 2;
                            }
                            break;
                        // still need to manage negation!
                        case '~':
                            // not
                            nextNotVal = true;
                            i ++;
                            break;
                    }
                    if (i == -1)
                    {
                        Console.WriteLine("This sentence is invalid");
                        return false;
                    }
                }
                else
                {
                    // if char is a letter, add a subsetence which is a proposition
                    subSentences.Add(new Sentence(input[i],nextNotVal));
                    nextNotVal = false;
                    i = i + 1;
                }

            }

        }
        return true;
    }
    
    public int bracket(int start,bool nextNotVal)
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
                        lCount ++;
                        break;
                    case ')':
                        rCount ++;
                        rLast = i;
                        break;

                }
                if (lCount == rCount)
                {
                    // make sub-sentence
                    subSentences.Add(new Sentence(
                        input.Substring(lFirst + 1,rLast - lFirst - 1),nextNotVal));
                    return (rLast + 1);
                }

            }

        }
        return -1;
    }

    public virtual string printString()
    {
        string sent = null;
        if (this.not == true)
        {
            sent = sent + "~";
        }
        for (int i = 0; i < joins.Count; i ++)
        {
            string firstSub = subSentences[i].printString();
            if (firstSub.Length <= 2)
            {
                sent = sent + firstSub + joins[i].printString();
            }
            else
            {
                sent = sent + "(" + firstSub + ")" + joins[i].printString();
            }
        }
        string lastSub = subSentences[subSentences.Count - 1].printString();
        if (lastSub.Length <= 2)
        {
            sent = sent + lastSub;
        }
        else
        {
            sent = sent + "(" + lastSub + ")";
        }
        return sent;
    }
    public virtual bool getValue(){
        bool carry = subSentences[0].getValue();
        for (int i = 0; i < joins.Count; i++)
        {
            carry = joins[i].operate(carry ,subSentences[i+1].getValue());
        }
        if (this.not == true)
        {
            return !carry;
        }
        else 
        {
            return carry;
        }
    }
}