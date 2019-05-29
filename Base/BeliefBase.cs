using System;
using System.Collections.Generic;
using System.Linq;

using static GlobalProps;
public class BeliefBase{
    List<Sentence> kBase = new List<Sentence>();
    //List of propositions which are used in the beliefs
    List<Proposition> kProps = new List<Proposition>();
    //Truth table to check the logical entailment of sentences
    TruthTable truthTable;
    string name;
    public BeliefBase(string name){
        this.name = name;
        truthTable = new TruthTable(this.kBase);
    }

    /** Tell function: takes a user input and adds it to the knowledge base */
    public void TELL(Sentence newSentence){
        //Check to see if the sentence has not already been added to the knowledge base
        // need to fix somehow 
        // Later
        if (newSentence.printString() == "T" || newSentence.printString() == "F")
        {
            Console.WriteLine("Cannot add axioms to belief base" + Environment.NewLine);
            return;
        }
        if (checkEntailment(newSentence))
        {
            bool alreadyExists = false;
            foreach (var sentence in kBase)
            {
                if (sentence.printString() == newSentence.printString())
                {
                    alreadyExists = true;
                }
            }
            if (alreadyExists)
            {
                Console.WriteLine("Your sentence already exists in the KB " + Environment.NewLine);
            }
            else
            {
                kBase.Add(newSentence);
                Console.WriteLine("Your sentence was added to the KB" + Environment.NewLine);
            }
        }
        else
        {
            Console.WriteLine("Your sentence is not entailed by the belief base" + Environment.NewLine);
            // adjustBeliefBase(newSentence);
            List<Sentence> Options = new List<Sentence>();
            Options = adjustBeliefBase(newSentence);
            Options.Add(newSentence);
            int count = 1;
            Console.WriteLine("Here are your options: ");
            foreach (Sentence item in Options)
            {
                Console.WriteLine(count + ". " + item.printString());
                count++;
            }

            string val;
            int option = 0;
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine("Please select a belief set: ");
                val = Console.ReadLine();
                valid = int.TryParse(val, out option);
                if (valid)
                {
                    valid = ((Options.Count) >= option) && (option > 0);
                }
            }
            kBase.Clear();
            distributeClauses(Options[option - 1]);
            listSentences();
        }
    }
    private void distributeClauses(Sentence set)
    {
        kBase.Clear();
        if (set.Joins.Count == 0)
        {
            kBase.Add(set);
        }
        else if (set.Joins[0].Op == "&&")
        {
            foreach (Sentence sub in set.SubSentences)
            {
                kBase.Add(sub);
            }
        }
        else
        {
            kBase.Add(set);
        }
    }
    public string printBase()
    {
        string sentences = "";
        for (int i = 0; i < kBase.Count; i++)
        {
            sentences = sentences + (i + 1).ToString() + ". " + kBase[i].printString() + Environment.NewLine;
        }
        return sentences;
    }
    private List<Sentence> adjustBeliefBase(Sentence newSentence)
    {
            Sentence allSentences = makeBBSet();
            List<Sentence> partialMeetOptions = new List<Sentence>();
            List<int> order = new List<int>();
            for (int i = 0; i < allSentences.SubSentences.Count; i++)
            {
                order.Add(i);
            }
            List<List<int>> combos = Combos.GetAllCombos(order);
            combos.Reverse();
            foreach (List<int> combo in combos)
            {
                Sentence set = new Sentence(null);
                foreach (int index in combo)
                {
                    set.SubSentences.Add(allSentences.SubSentences[index]);
                    set.Joins.Add(new Operator("&&"));
                }
                set.Joins.RemoveAt(set.Joins.Count - 1);
                if (partialMeet(set, newSentence))
                {
                    set.SubSentences.Add(newSentence);
                    set.Joins.Add(new Operator("&&"));
                    partialMeetOptions.Add(set);
                }
            }
        return partialMeetOptions;
    }
    private Sentence makeBBSet()
    {
        Sentence evalSent = new Sentence(null);
        foreach (Sentence sent in kBase)
        {
            if (sent.SubSentences[0].GetType() == typeof(Proposition))
            {
                evalSent.SubSentences.Add(sent);
                evalSent.Joins.Add(new Operator("&&"));
            }
            else if (sent.Joins[0].Op == "&&")
            {
                foreach (Sentence clause in sent.SubSentences)
                {
                    evalSent.SubSentences.Add(clause);
                    evalSent.Joins.Add(new Operator("&&"));
                }
            }
            else
            {
                evalSent.SubSentences.Add(sent);
                evalSent.Joins.Add(new Operator("&&"));
            }
        }
        evalSent.Joins.RemoveAt(evalSent.Joins.Count-1);
        return evalSent;
    }


    private bool partialMeet(Sentence set, Sentence newSentence)
    {
        Sentence clauseWithNewSent = new Sentence(null);
        clauseWithNewSent.SubSentences.Add(set);
        clauseWithNewSent.SubSentences.Add(newSentence);
        clauseWithNewSent.Joins.Add(new Operator("&&"));
        clauseWithNewSent.simplfy();
        if (clauseWithNewSent.printString() == "F")
        {
            return false;
        }
        return true;
    }
    public Sentence ASK(){
        return null;
    }
    //Allows a user to query the knowledge base and determines if a logical sentence entails the KB
    public bool checkEntailment(Sentence newSentence){
        if (kBase.Count == 0)
        {
            return true;
        }
        // if there are any new variables in the new sentence
        truthTable.GenerateTable(kBase, newSentence);
        kProps = truthTable.InvovledProps;
        List<bool[]> criticalVals = truthTable.valuesToMatch(true);
        for (int i = 0; i < criticalVals.Count; i++)
        {
            GlobalProps.setPropositions(criticalVals[i], kProps);
            if (newSentence.getValue() == false)
            {
                return false;
            }
        }
        if (criticalVals.Count == 0)
        {
            return false;
        }
        return true;
    }
    //Prints the knowledge base to terminal
    public void listSentences()
    {
        Console.WriteLine("Knowledge Base of "+ this.name +":");
        for (int i = 0; i < kBase.Count; i++)
        {
            Console.WriteLine((i + 1).ToString()+". "+ kBase[i].printString());
        }
        if (kBase.Count == 0)
        {
            Console.WriteLine("[KB is empty]");
        }
        Console.Write(Environment.NewLine);
    }
}