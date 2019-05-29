using System;
using System.Collections.Generic;
using static GlobalProps;

/** Class to structure sentences and ensure that the input is valid */
public class Sentence
{
    List<Sentence> subSentences = new List<Sentence>();
    List<Operator> joins = new List<Operator>();
    Sentence parent;
    List<Proposition> propsInSentence = new List<Proposition>();
    bool not;

    bool isValid;
    bool nextNotVal = false;
    bool hasBrackets = false;
    string input;

    public bool Not { get => not; set => not = value; }
    public List<Proposition> PropsInSentence { get => propsInSentence; set => propsInSentence = value; }
    public bool IsValid { get => isValid; set => isValid = value; }

    //Define sentence structure
    public Sentence(string input, bool not = false, Sentence parent = null)
    {
        this.input = input;
        this.Not = not;
        this.hasBrackets = false;
        this.parent = parent;
        IsValid = smartSort();
    }
    protected Sentence(Sentence parent)
    {
        this.parent = parent;
    }


    private Sentence(char propName, bool not, Sentence parent)
    {
        this.Not = not;
        Proposition newProp = null;
        this.parent = parent;
        for (int j = 0; j < props.Count; j++)
        {
            if (props[j].Name == propName)
            {
                newProp = props[j];
            }
        }
        if (newProp == null)
        {
            newProp = new Proposition(propName, this);
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
                            i = bracket(i, nextNotVal);
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
                                joins.Add(new Operator(input.Substring(i, 2)));
                                if (joins[joins.Count - 1].OpName == "invalid")
                                {
                                    i = -1;
                                }
                                else
                                {
                                    i = i + 2;
                                }
                            }
                            break;
                        case '~':
                            // not
                            i = negCheck(i);
                            if (i != -1)
                            {
                                nextNotVal = !nextNotVal;
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
                        if (input[i] == 'F')
                        {
                            subSentences.Add(new Falsum(this));
                        }
                        else if (input[i] == 'T')
                        {
                            subSentences.Add(new Tautology(this));
                        }
                        // if char is a letter, add a subsetence which is a proposition
                        subSentences.Add(new Sentence(input[i], nextNotVal, this));
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
            pushSubSentence();
        }
        return true;
    }

    private int bracket(int start, bool nextNotVal)
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
                        rLast = i;
                        break;

                }
                if (lCount == rCount)
                {
                    // make sub-sentence
                    Sentence bracketSentence = new Sentence(input.Substring(lFirst + 1,
                    rLast - lFirst - 1), nextNotVal, this);
                    // this.hasBrackets = true;
                    bracketSentence.hasBrackets = true;
                    subSentences.Add(bracketSentence);
                    return (rLast + 1);
                }

            }

        }
        return -1;
    }

    private void pushPropsInSentence()
    // Move sentences into brackets
    {
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
    private void pushSubSentence()
    {
        if (subSentences.Count == 1 && subSentences[0].GetType() != typeof(Proposition))
        {
            Sentence sent = subSentences[0];
            this.hasBrackets = sent.hasBrackets;
            joins = sent.joins;
            subSentences = sent.subSentences;
            this.Not = !(PropLogicRules.biconditional(this.Not, sent.Not));
        }
    }
    private int operatorCheck(int i)
    //Verify location of operators
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
    //Check is a proposition is negative
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
    /* Checks for a proposition and returns the position */
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
    // Outputs the propositional sentence as a string so it can be printed
    {
        string sent = null;
        if (this.not == true)
        {
            sent = sent + "~";
        }
        if (this.hasBrackets == true)
        {
            sent = sent + "(";
        }
        for (int i = 0; i < joins.Count; i++)
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

    public virtual bool getValue()
    // Returns if a value is true or false
    {
        bool carry = subSentences[0].getValue();
        for (int i = 0; i < joins.Count; i++)
        {
            carry = joins[i].operate(carry, subSentences[i + 1].getValue());
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
    //Converts a sentence to conjunctive normal form
    {
        bicondLoop();
        Console.WriteLine("After Biconditional:" + printString());
        impLoop();
        Console.WriteLine("After Implies:" + printString());
        deMorganLoop();
        Console.WriteLine("After deMorgan:" + printString());
        distLoop();
        Console.WriteLine("After dist:" + printString());
        bracketLoop();
        Console.WriteLine("After brackets:" + printString());
        logicLoop();
        Console.WriteLine("After logic:" + printString());
        knownLoop();
        // pushSubSentence();
        // shorteningLoop();
        Console.WriteLine("After knowns:" + printString());
    }
    
    /*Main biconditional elimination function */
    private void bicondLoop()
    {
        foreach (Sentence sub in subSentences)
        {
            sub.bicondLoop();
        }
        if (joins.Count != 0)
        {
            Operator[] staticJoins = new Operator[joins.Count];
            joins.CopyTo(staticJoins);
            foreach (Operator join in staticJoins)
            {
                if (join.Op == "<>")
                {
                    convertBiconditional();
                }
            }
        }
    }
    /*Main Implication elimination function */
    private void impLoop()
    {
        foreach (Sentence sub in subSentences)
        {
            sub.impLoop();
        }
        if (joins.Count != 0)
        {
            Operator[] staticJoins = new Operator[joins.Count];
            joins.CopyTo(staticJoins);
            foreach (Operator join in staticJoins)
            {
                if (join.Op == "->")
                {
                    convertImplication();
                }
            }
        }
    }
    /* Main De Morgan's function */
    private void deMorganLoop()
    {
        //checks if De Morgan's needs to be applied (negation outside a bracket)
        if (applyDeMorgans())
        // push negatives inwards
        {
            DeMorgans();
        }
        //recursively apply de morgans to each subsentence
        foreach (Sentence sub in subSentences)
        {
            sub.deMorganLoop();
        }
    }
    /* Distributes and over or */
    private void distLoop()
    {
        foreach (Sentence sub in subSentences)
        {
            sub.distLoop();
        }
        if (joins.Count != 0)
        {
            Operator[] staticJoins = new Operator[joins.Count];
            joins.CopyTo(staticJoins);
            foreach (Operator join in staticJoins)
            {
                // commutativity();
                bool noChange = false;
                if (join.Op == "||")
                {
                    noChange = distributivity("||","&&");
                }
                //if first operation does not change, flip and run again
                if(noChange)
                {
                    commutativity();
                    if (join.Op == "||")
                    {
                        noChange = distributivity("||", "&&");
                    }
                }
            }
        }
        //recursively apply distribution to each subsentence
        foreach (Sentence sub in subSentences)
        {
            sub.distLoop();
        }
    }

    /*APply main logic loop to all sentences and subsentences */
    private void logicLoop()
    {
        foreach (Sentence sub in subSentences)
        {
            sub.logicLoop();
        }

        if (this.GetType() != typeof(Proposition))
        {
            applyBasicLogic();
        }
    }
    //Determines if a setence is a tautology or falsum 
    private void knownLoop()
    {
        foreach (Sentence sub in subSentences)
        {
            sub.knownLoop();
        }
        bool noAction = false;
        while (!noAction)
        {
            if (this.GetType() != typeof(Proposition) &&
        this.GetType() != typeof(Tautology) &&
        this.GetType() != typeof(Falsum))
            {
                noAction = applyKnowns();
            }
            else 
            {
                noAction = true;
            }
        }
    }
    //Checks to see if brackets 
    private void bracketLoop()
    {
        foreach (Sentence sub in subSentences)
        {
            sub.bracketLoop();
        }
        if (joins.Count >= 1)
        {
            fixBrackets();
        }
    }
    //recursively apply simplification to all subsentences
    private void cycle()
    {
        for (int sIndex = 0; sIndex < subSentences.Count; sIndex++)
        {
            subSentences[sIndex].simplfy();
        }
    }
    //Eliminates implication operators
    private bool convertImplication()
    {
        for (int impIndex = 0; impIndex < joins.Count; impIndex++)
        {
            if (joins[impIndex].OpName == "implication")
            {
                joins[impIndex] = new Operator("||");
                subSentences[impIndex].not = flip(subSentences[impIndex].not);
                // Console.WriteLine(this.printString());
                return false;
            }
        }
        return true;
    }
    //De Morgans function
    private void DeMorgans()
    {
        //Applies negation
        not = flip(not);
        hasBrackets = (parent != null);
        int index;
        for (index = 0; index < joins.Count; index++)
        {
            //Switch operator
            subSentences[index].not = flip(subSentences[index].not);
            if (joins[index].OpName == "or")
            {
                joins[index] = new Operator("&&");
            }
            else if (joins[index].OpName == "and")
            {
                joins[index] = new Operator("||");
            }
        }
        if (subSentences[index].GetType() != typeof(Proposition))
        {
            subSentences[index].not = flip(subSentences[index].not);
        }
        else
        {
            not = flip(not);
            hasBrackets = not;
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
    //Eliminates bi-conditional operators
    private bool convertBiconditional()
    {
        for (int biIndex = 0; biIndex < joins.Count; biIndex++)
        {
            if (joins[biIndex].OpName == "biconditional")
            {
                string aSentence = subSentences[biIndex].printString();
                string bSentence = subSentences[biIndex + 1].printString();

                subSentences[biIndex] = new Sentence(aSentence + "->" + bSentence, false, this);
                subSentences[biIndex].hasBrackets = true;

                subSentences[biIndex + 1] = new Sentence(bSentence + "->" + aSentence, false, this);
                subSentences[biIndex + 1].hasBrackets = true;

                joins[biIndex] = new Operator("&&");

                return false;
            }
        }
        return true;
    }
    //Distributes AND over OR 
    private bool distributivity(string op, string overOp)
    {
        // Console.WriteLine("Before: " + printString());
        if (joins.Count == 1)
        {
            if (joins[0].Op == op)
            {
                if (subSentences[1].joins.Count >= 1)
                {
                    if (subSentences[1].joins[0].Op == overOp)
                    {
                        Sentence subSent1 = subSentences[1];
                        List<Sentence> newSentences = new List<Sentence>();
                        List<Operator> newJoins = new List<Operator>();
                        foreach (Sentence subSubSent in subSent1.subSentences)
                        {
                            Sentence a = new Sentence(subSentences[0].printString() +
                            op + subSubSent.printString(), false, this);
                            a.hasBrackets = true;
                            newSentences.Add(a);
                        }
                        for (int i = 0; i < (newSentences.Count - 1); i++)
                        {
                            newJoins.Add(new Operator(overOp));
                        }

                        subSentences = newSentences;
                        joins = newJoins;

                        // Console.WriteLine("After: " + printString());
                        return false;
                    }
                }
            }
        }
        // Console.WriteLine("After: " + printString());
        return true;
    }
    // flip arguements if join is OR or AND

    /* Main function to simplify sentences into CNF form */
    private bool applyBasicLogic()
    // assumes all joins in the same sentence are the same operators, else they are seperated by brackets
    {
        for (int i = 0; i < subSentences.Count; i++)
        {
            for (int j = 0; j < subSentences.Count; j++)
            {
                if (i != j)
                {
                    if (subSentences[i].printString() == subSentences[j].printString())
                    {
                        joins.RemoveAt(0);
                        subSentences.RemoveAt(j);
                        return false;
                    }
                    else if (subSentences[i].printString() == "~(" + subSentences[j].printString() + ")"
                    || subSentences[i].printString() == "~" + subSentences[j].printString())
                    {
                        string checkOp = joins[0].OpName;
                        if (j > i)
                        {
                            subSentences.RemoveAt(j);
                            subSentences.RemoveAt(i);
                        }
                        else
                        {
                            subSentences.RemoveAt(i);
                            subSentences.RemoveAt(j);
                        }
                        joins.RemoveAt(0);
                        // hasBrackets = false;
                        if (checkOp == "or")
                        {
                            subSentences.Clear();
                            joins.Clear();
                            subSentences.Add(new Tautology(this));
                        }
                        else
                        {
                            subSentences.Add(new Falsum(this));
                        }

                        return false;
                        // pushSubSentence();
                    }
                    else
                    {
                        //CNF form 
                        if (subSentences[i].applyDeMorgans())
                        {
                            subSentences[i].DeMorgans();
                            if (subSentences[i].printString() == subSentences[j].printString())
                            {
                                joins.RemoveAt(0);
                                subSentences.RemoveAt(j);
                                return false;
                            }
                            else if (subSentences[i].printString() == "~(" + subSentences[j].printString() + ")"
                            || subSentences[i].printString() == "~" + subSentences[j].printString())
                            {
                                string checkOp = joins[0].OpName;
                                if (j > i)
                                {
                                    subSentences.RemoveAt(j);
                                    subSentences.RemoveAt(i);
                                }
                                else
                                {
                                    subSentences.RemoveAt(i);
                                    subSentences.RemoveAt(j);
                                }
                                joins.RemoveAt(0);
                                // hasBrackets = false;
                                if (checkOp == "or")
                                {
                                    subSentences.Clear();
                                    joins.Clear();
                                    subSentences.Add(new Tautology(this));
                                }
                                else
                                {
                                    subSentences.Add(new Falsum(this));
                                }
                                return false;
                            }
                            else
                            {
                                subSentences[i].DeMorgans();
                            }
                        }
                    }
                }
            }

        }
        return true;
    }
    /*If a proposition is only true or only false, remove the sentence */
    private bool applyKnowns()
    {
        for (int i = 0; i < subSentences.Count; i++)
        {
            if (subSentences[i].subSentences.Count == 1)
            {
                if (subSentences[i].subSentences[0].GetType() == typeof(Tautology))
                {
                    string checkOp = joins[0].OpName;
                    // joins.RemoveAt(0);
                    hasBrackets = false;
                    if (checkOp == "and")
                    // T and x
                    {
                        subSentences.RemoveAt(i);
                        joins.RemoveAt(0);
                    }
                    else
                    // T or x
                    {
                        subSentences.Clear();
                        joins.Clear();
                        
                        // this.parent = null;
                    }
                    return false;
                    
                }
                else if (subSentences[i].subSentences[0].GetType() == typeof(Falsum))
                {
                    string checkOp = joins[0].OpName;
                    joins.RemoveAt(0);
                    hasBrackets = false;
                    if (checkOp == "or")
                    // F or x
                    {
                        subSentences.RemoveAt(i);
                    }
                    else
                    // F and x
                    {
                        subSentences.Clear();
                        joins.Clear();
                    }
                    return false;
                }
                
            }
        }
        return true;
    }
    /*Removes unneccesary brackets */
    private void fixBrackets()
    {
        if (joins.Count >= 0)
        {
            Sentence[] staticSent = new Sentence[subSentences.Count];
            subSentences.CopyTo(staticSent);
            foreach (Sentence sub in staticSent)
            {
                if (sub.joins.Count >= 1)
                {
                    if (sub.joins[0].Op == joins[0].Op)
                    {
                        subSentences.AddRange(sub.subSentences);
                        joins.AddRange(sub.joins);
                        subSentences.Remove(sub);
                    }
                }
            }
        }
        else
        {
            if (subSentences.Count == 1)
            {
                subSentences[0].hasBrackets = subSentences[0].not;
            }
        }
    }
    //Flips the position of propositions
    private void commutativity()
    {
        // Console.WriteLine("Before: " + printString());
        if (joins[0].OpName == "or" || joins[0].OpName == "and")
        {
            Sentence a = new Sentence(subSentences[0].printString(), false, this);
            a.hasBrackets = subSentences[0].hasBrackets;
            a.Not = subSentences[0].not;
            subSentences[0] = subSentences[1];
            subSentences[1] = a;
            // return;
        }
        // Console.WriteLine("After: " + printString());
    }
    //Determines if De Morgans can be applied to a senence
    private bool applyDeMorgans()
    {
        if (not && subSentences.Count > 1)
        {
            return true;
        }
        return false;
    }

}