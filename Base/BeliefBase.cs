using System;

using System.Collections.Generic;

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
        bool alreadyExists = false;
        foreach (var sentence in kBase) {
            if (sentence.printString() == newSentence.printString()) {
                alreadyExists = true;
            }
        }
        if (alreadyExists) {
            Console.WriteLine("Your sentence already exists in the KB " + Environment.NewLine);
        } else {
            kBase.Add(newSentence);
            Console.WriteLine("Your sentence was added to the KB" + Environment.NewLine);
        }
        
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
        truthTable.GenerateTable(kBase);
        kProps = truthTable.InvovledProps;
        List<bool[]> criticalVals = truthTable.valuesToMatch();
        for (int i = 0; i < criticalVals.Count; i++)
        {
            GlobalProps.setPropositions(criticalVals[i], kProps);
            if (newSentence.getValue() == false)
            {
                return false;
            }
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