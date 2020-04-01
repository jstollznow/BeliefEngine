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
    public List<Sentence> SubSentences { get => subSentences; set => subSentences = value; }
    public List<Operator> Joins { get => joins; set => joins = value; }

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
        SubSentences.Add(newProp);
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
                                Joins.Add(new Operator(input.Substring(i, 2)));
                                if (Joins[Joins.Count - 1].OpName == "invalid")
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
                            SubSentences.Add(new Falsum(this));
                        }
                        else if (input[i] == 'T')
                        {
                            SubSentences.Add(new Tautology(this));
                        }
                        else
                        {
                            // if char is a letter, add a subsetence which is a proposition
                            SubSentences.Add(new Sentence(input[i], nextNotVal, this));
                            nextNotVal = false;
                        }
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
                    SubSentences.Add(bracketSentence);
                    return (rLast + 1);
                }

            }

        }
        return -1;
    }

    private void pushPropsInSentence()
    // Move sentences into brackets
    {
        foreach (Sentence sent in SubSentences)
        {
            foreach (Proposition prop in sent.propsInSentence)
            {
                if (!(prop.Name == 'F' || prop.Name == 'T'))
                if (propsInSentence.Contains(prop) == false)
                {
                    propsInSentence.Add(prop);
                }
            }
        }
    }
    private void pushSubSentence()
    {
        if (SubSentences.Count == 1 && SubSentences[0].GetType() != typeof(Proposition)
        && SubSentences[0].GetType() != typeof(Tautology) 
        && SubSentences[0].GetType() != typeof(Falsum))
        {
            Sentence sent = SubSentences[0];
            this.hasBrackets = sent.hasBrackets;
            Joins = sent.Joins;
            SubSentences = sent.SubSentences;
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
        for (int i = 0; i < Joins.Count; i++)
        {
            string firstSub = SubSentences[i].printString();
            sent = sent + firstSub + Joins[i].printString();
        }
        string lastSub = SubSentences[SubSentences.Count - 1].printString();
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
        bool carry = SubSentences[0].getValue();
        for (int i = 0; i < Joins.Count; i++)
        {
            carry = Joins[i].operate(carry, SubSentences[i + 1].getValue());
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
        // Console.WriteLine("After Biconditional:" + printString());
        impLoop();
        // Console.WriteLine("After Implies:" + printString());
        deMorganLoop();
        // Console.WriteLine("After deMorgan:" + printString());
        distLoop();
        // Console.WriteLine("After dist:" + printString());
        bracketLoop();
        // Console.WriteLine("After brackets:" + printString());
        logicLoop();
        // Console.WriteLine("After logic:" + printString());
        knownLoop();
        // Console.WriteLine("After knowns:" + printString());
        truthLoop();
        // Console.WriteLine("After truth table:" + printString());
    }
    
    /*Main biconditional elimination function */
    private void bicondLoop()
    {
        foreach (Sentence sub in SubSentences)
        {
            sub.bicondLoop();
        }
        if (Joins.Count != 0)
        {
            Operator[] staticJoins = new Operator[Joins.Count];
            Joins.CopyTo(staticJoins);
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
        foreach (Sentence sub in SubSentences)
        {
            sub.impLoop();
        }
        if (Joins.Count != 0)
        {
            Operator[] staticJoins = new Operator[Joins.Count];
            Joins.CopyTo(staticJoins);
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
        foreach (Sentence sub in SubSentences)
        {
            sub.deMorganLoop();
        }
    }
    /* Distributes and over or */
    private void distLoop()
    {
        foreach (Sentence sub in SubSentences)
        {
            sub.distLoop();
        }
        if (Joins.Count != 0)
        {
            Operator[] staticJoins = new Operator[Joins.Count];
            Joins.CopyTo(staticJoins);
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
        foreach (Sentence sub in SubSentences)
        {
            sub.distLoop();
        }
    }

    /*APply main logic loop to all sentences and subsentences */
    private void logicLoop()
    {
        foreach (Sentence sub in SubSentences)
        {
            sub.logicLoop();
        }

        if (this.GetType() != typeof(Proposition) &&
        this.GetType() != typeof(Tautology) &&
        this.GetType() != typeof(Falsum))
        {
            applyBasicLogic();
        }
    }
    //Determines if a setence is a tautology or falsum 
    private void knownLoop()
    {
        foreach (Sentence sub in SubSentences)
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
        foreach (Sentence sub in SubSentences)
        {
            sub.bracketLoop();
        }
        if (Joins.Count >= 1)
        {
            fixBrackets();
        }
    }
    
    private void truthLoop()
    {
        foreach (Sentence sub in SubSentences)
        {
            sub.truthLoop();
        }
        if (Joins.Count >= 1)
        {
            applyTruthTable();
        }
    }
    private void cycle()
    {
        for (int sIndex = 0; sIndex < SubSentences.Count; sIndex++)
        {
            SubSentences[sIndex].simplfy();
        }
    }
    //Eliminates implication operators
    private bool convertImplication()
    {
        for (int impIndex = 0; impIndex < Joins.Count; impIndex++)
        {
            if (Joins[impIndex].OpName == "implication")
            {
                Joins[impIndex] = new Operator("||");
                SubSentences[impIndex].not = flip(SubSentences[impIndex].not);
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
        for (index = 0; index < Joins.Count; index++)
        {
            SubSentences[index].not = flip(SubSentences[index].not);
            if (Joins[index].OpName == "or")
            {
                Joins[index] = new Operator("&&");
            }
            else if (Joins[index].OpName == "and")
            {
                Joins[index] = new Operator("||");
            }
        }
        if (SubSentences[index].GetType() != typeof(Proposition))
        {
            SubSentences[index].not = flip(SubSentences[index].not);
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
        for (int biIndex = 0; biIndex < Joins.Count; biIndex++)
        {
            if (Joins[biIndex].OpName == "biconditional")
            {
                string aSentence = SubSentences[biIndex].printString();
                string bSentence = SubSentences[biIndex + 1].printString();

                SubSentences[biIndex] = new Sentence(aSentence + "->" + bSentence, false, this);
                SubSentences[biIndex].hasBrackets = true;

                SubSentences[biIndex + 1] = new Sentence(bSentence + "->" + aSentence, false, this);
                SubSentences[biIndex + 1].hasBrackets = true;

                Joins[biIndex] = new Operator("&&");

                return false;
            }
        }
        return true;
    }
    //Distributes AND over OR 
    private bool distributivity(string op, string overOp)
    {
        // Console.WriteLine("Before: " + printString());
        if (Joins.Count == 1)
        {
            if (Joins[0].Op == op)
            {
                if (SubSentences[1].Joins.Count >= 1)
                {
                    if (SubSentences[1].Joins[0].Op == overOp)
                    {
                        Sentence subSent1 = SubSentences[1];
                        List<Sentence> newSentences = new List<Sentence>();
                        List<Operator> newJoins = new List<Operator>();
                        foreach (Sentence subSubSent in subSent1.SubSentences)
                        {
                            Sentence a = new Sentence(SubSentences[0].printString() +
                            op + subSubSent.printString(), false, this);
                            a.hasBrackets = true;
                            newSentences.Add(a);
                        }
                        for (int i = 0; i < (newSentences.Count - 1); i++)
                        {
                            newJoins.Add(new Operator(overOp));
                        }

                        SubSentences = newSentences;
                        Joins = newJoins;

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
        for (int i = 0; i < SubSentences.Count; i++)
        {
            for (int j = 0; j < SubSentences.Count; j++)
            {
                if (i != j)
                {
                    if (SubSentences[i].printString() == SubSentences[j].printString())
                    {
                        Joins.RemoveAt(0);
                        SubSentences.RemoveAt(j);
                        return false;
                    }
                    else if (SubSentences[i].printString() == "~(" + SubSentences[j].printString() + ")"
                    || SubSentences[i].printString() == "~" + SubSentences[j].printString())
                    {
                        string checkOp = Joins[0].OpName;
                        if (j > i)
                        {
                            SubSentences.RemoveAt(j);
                            SubSentences.RemoveAt(i);
                        }
                        else
                        {
                            SubSentences.RemoveAt(i);
                            SubSentences.RemoveAt(j);
                        }
                        Joins.RemoveAt(0);
                        // hasBrackets = false;
                        if (checkOp == "or")
                        {
                            SubSentences.Clear();
                            Joins.Clear();
                            SubSentences.Add(new Tautology(this));
                        }
                        else
                        {
                            SubSentences.Add(new Falsum(this));
                        }

                        return false;
                        // pushSubSentence();
                    }
                    else
                    {
                        if (SubSentences[i].applyDeMorgans())
                        {
                            SubSentences[i].DeMorgans();
                            if (SubSentences[i].printString() == SubSentences[j].printString())
                            {
                                Joins.RemoveAt(0);
                                SubSentences.RemoveAt(j);
                                return false;
                            }
                            else if (SubSentences[i].printString() == "~(" + SubSentences[j].printString() + ")"
                            || SubSentences[i].printString() == "~" + SubSentences[j].printString())
                            {
                                string checkOp = Joins[0].OpName;
                                if (j > i)
                                {
                                    SubSentences.RemoveAt(j);
                                    SubSentences.RemoveAt(i);
                                }
                                else
                                {
                                    SubSentences.RemoveAt(i);
                                    SubSentences.RemoveAt(j);
                                }
                                Joins.RemoveAt(0);
                                // hasBrackets = false;
                                if (checkOp == "or")
                                {
                                    SubSentences.Clear();
                                    Joins.Clear();
                                    SubSentences.Add(new Tautology(this));
                                }
                                else
                                {
                                    SubSentences.Add(new Falsum(this));
                                }
                                return false;
                            }
                            else
                            {
                                SubSentences[i].DeMorgans();
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
        for (int i = 0; i < SubSentences.Count; i++)
        {
            if (SubSentences[i].SubSentences.Count == 1)
            {
                if (SubSentences[i].SubSentences[0].GetType() == typeof(Tautology))
                {
                    string checkOp = Joins[0].OpName;
                    // joins.RemoveAt(0);
                    hasBrackets = false;
                    if (checkOp == "and")
                    // T and x
                    {
                        SubSentences.RemoveAt(i);
                        Joins.RemoveAt(0);
                    }
                    else
                    // T or x
                    {
                        SubSentences.Clear();
                        Joins.Clear();
                        
                        // this.parent = null;
                    }
                    return false;
                    
                }
                else if (SubSentences[i].SubSentences[0].GetType() == typeof(Falsum))
                {
                    string checkOp = Joins[0].OpName;
                    Joins.RemoveAt(0);
                    hasBrackets = false;
                    if (checkOp == "or")
                    // F or x
                    {
                        SubSentences.RemoveAt(i);
                    }
                    else
                    // F and x
                    {
                        SubSentences.Clear();
                        Joins.Clear();
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
        if (Joins.Count >= 0)
        {
            Sentence[] staticSent = new Sentence[SubSentences.Count];
            SubSentences.CopyTo(staticSent);
            foreach (Sentence sub in staticSent)
            {
                if (sub.Joins.Count >= 1)
                {
                    if (sub.Joins[0].Op == Joins[0].Op)
                    {
                        SubSentences.AddRange(sub.SubSentences);
                        Joins.AddRange(sub.Joins);
                        SubSentences.Remove(sub);
                    }
                }
            }
        }
        else
        {
            if (SubSentences.Count == 1)
            {
                SubSentences[0].hasBrackets = SubSentences[0].not;
            }
        }
    }
    //Flips the position of propositions
    private void commutativity()
    {
        // Console.WriteLine("Before: " + printString());
        if (Joins[0].OpName == "or" || Joins[0].OpName == "and")
        {
            Sentence a = new Sentence(SubSentences[0].printString(), false, this);
            a.hasBrackets = SubSentences[0].hasBrackets;
            a.Not = SubSentences[0].not;
            SubSentences[0] = SubSentences[1];
            SubSentences[1] = a;
            // return;
        }
        // Console.WriteLine("After: " + printString());
    }
    //Determines if De Morgans can be applied to a senence
    private bool applyDeMorgans()
    {
        if (not && SubSentences.Count > 1)
        {
            return true;
        }
        return false;
    }

    private bool applyTruthTable()
    {
        pushPropsInSentence();
        List<Sentence> thisSent = new  List<Sentence>();
        thisSent.Add(this);
        TruthTable test = new TruthTable(thisSent);
        // test.GenerateTable()
        switch(test.sentenceCheck())
        {
            case 0:
                subSentences.Clear();
                joins.Clear();
                subSentences.Add(new Falsum(this));
                return false;
            case 1:
                subSentences.Clear();
                joins.Clear();
                subSentences.Add(new Falsum(this));
                return false;
            default:
                return true;
        }
        
    }
}