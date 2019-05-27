using System;
using System.Collections.Generic;
using static GlobalProps;
public class Sentence{
    List<Sentence> subSentences = new List<Sentence>();
    List<Operator> joins = new List<Operator>();

    List<Proposition> propsInSentence = new List<Proposition>();
    bool not;

    bool isValid;
    
    bool nextNotVal = false;
    bool hasBrackets = false;
    string input;

    public bool Not { get => not; set => not = value; }
    public List<Proposition> PropsInSentence { get => propsInSentence; set => propsInSentence = value; }
    public bool IsValid { get => isValid; set => isValid = value; }

    public Sentence(string input, bool not = false)
    {
        this.input = input;
        this.Not = not;
        this.hasBrackets = false;
        IsValid = smartSort();
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
        if (PropsInSentence.Contains(newProp) == false)
        {
            PropsInSentence.Add(newProp);
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
                            i = operatorCheck(i);
                            if (i != -1)
                            {
                                joins.Add(new Operator(input.Substring(i,2)));
                                if (joins[joins.Count-1].OpName == "invalid")
                                {
                                    i = -1;
                                }
                                else 
                                {
                                    i = i + 2;
                                }
                            }
                            break;
                        // still need to manage negation!
                        case '~':
                            // not
                            i = negCheck(i);
                            if (i != -1)
                            {
                                nextNotVal = true;
                                i = i + 1;
                            }
                            break;
                        default:
                            i = -1;
                        break;
                    }
                }
                else
                {
                    i = propCheck(i);
                    if (i != -1)
                    {
                        // if char is a letter, add a subsetence which is a proposition
                        subSentences.Add(new Sentence(input[i],nextNotVal));
                        nextNotVal = false;
                        i = i + 1;
                    }
                }
                if (i == -1)
                {
                    Console.WriteLine("This sentence is invalid, please try again." + Environment.NewLine);
                    return false;
                }

            }
            pushPropsInSentence();
        }
        return true;
    }
    
    private int bracket(int start,bool nextNotVal)
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
                    Sentence bracketSentence = new Sentence(input.Substring(lFirst + 1, 
                    rLast - lFirst - 1), nextNotVal);
                    bracketSentence.hasBrackets = true; 
                    subSentences.Add(bracketSentence);
                    return (rLast + 1);
                }

            }

        }
        return -1;
    }

    private void pushPropsInSentence(){
        foreach (Sentence sent in subSentences)
        {
            foreach (Proposition prop in sent.propsInSentence)
            {
                if (propsInSentence.Contains(prop) == false)
                {
                    propsInSentence.Add(prop);
                }
            }
        }
    }
    private int operatorCheck(int i)
    {
        int orginalPos = i;
        // check before operator
        i--;
        if (i >= 0)
        {
            if ((input[i] == ')' || char.IsLetter(input[i])) == false)
            {
                i = -1;
                return i;
            }
        }
        else
        {
            // operator should never be evaluated on its own
            i = -1;
            return i;
        }
        
        // check behind the operator
        i = i + 3;
        if (input.Length > i)
        {
            if ((input[i] == '(' || input[i] == '~' || char.IsLetter(input[i])) == false)
            {
                i = -1;
                return i;
            }
        }
        else
        {
            // operator should never be evaluated on its own
            i = -1;
            return i;
        }

        i = orginalPos;
        return i;
    }
    private int negCheck(int i)
    {
        int orginalPos = i;
        i--;
        if (i >= 0)
        {
            // has to be an operator
            if ((input[i] == '|' || input[i] == '&' || input[i] == '>' || input[i] == '~') == false)
            {
                i = -1;
                return i;
            }
        }
        i = i + 2;
        if (input.Length > i)
        {
            if ((input[i] == '(' || input[i] == '~' || char.IsLetter(input[i])) == false)
            {
                i = -1;
                return i;
            }
        }
        else
        {
            // cannot finish a sentence w ~
            i = -1;
            return i;
        }
        i = orginalPos;
        return i;
    }

    private int propCheck(int i)
    {
        int orginalPos = i;
        i--;
        if (i >= 0)
        {
            // has to be an operator
            if (char.IsLetter(input[i]) == true)
            {
                i = -1;
                return i;
            }
        }
        i = i + 2;
        if (input.Length > i)
        {
            if (char.IsLetter(input[i]) == true)
            {
                i = -1;
                return i;
            }
        }
        i = orginalPos;
        return i;
    }
    public virtual string printString()
    {
        string sent = null;
        if (this.not == true)
        {
            sent = sent + "~";
        }
        if (this.hasBrackets==true){
            sent = sent + "(";
        }
        for (int i = 0; i < joins.Count; i ++)
        {
            string firstSub = subSentences[i].printString();
            sent = sent + firstSub + joins[i].printString();
        }
        string lastSub = subSentences[subSentences.Count - 1].printString();
        sent = sent + lastSub;
        if (this.hasBrackets == true)
        {
            sent = sent + ")";
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
    public void simplfy()
    {
        // convertImplication();
        pushNegativeInwards();
        Console.WriteLine(printString());
    }

    public void convertImplication()
    {
        for (int impIndex = 0; impIndex < joins.Count; impIndex++)
        {
            if (joins[impIndex].OpName == "implication")
            {
                joins[impIndex] = new Operator("||");
                subSentences[impIndex].not = flip(subSentences[impIndex].not);
                Console.WriteLine(this.printString());
                return;
            }
        }
    }
    public void pushNegativeInwards(){
        not = flip(not);
        for (int index = 0; index < subSentences.Count; index++)
        {
            subSentences[index].not = flip(subSentences[index].not);
        }
        
    }
    private bool flip(bool value)
    {
        if (value)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    // public static Sentence pushNegativeOutwards(Sentence sentence)
    // {

    // }
    // public static Sentence expandBiconditional(Sentence sentence)
    // {

    // }
    // public static Sentence convertToCNF(Sentence sentence)
    // {

    // }
}