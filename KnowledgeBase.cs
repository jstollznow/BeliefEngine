using System;

using System.Collections.Generic;

using static GlobalProps;
public class KnowledgeBase{
    List<Sentence> kBase = new List<Sentence>();
    List<Proposition> kProps = new List<Proposition>();

    TruthTable truthTable;
    string name;
    public KnowledgeBase(string name){
        this.name = name;
        truthTable = new TruthTable(this.kBase);
    }

    public void TELL(Sentence newSentence){
        kBase.Add(newSentence);
        Console.WriteLine("Your sentence was added to the KB");
        // if (checkEntailment(newSentence))
        // {
        //     kBase.Add(newSentence);
        //     Console.WriteLine("Your sentence was added to the KB");
        // }
        // else
        // {
        //     Console.WriteLine("Sentence did not agree with the knowledge of the agent.");
        // }
    }
    public Sentence ASK(){
        return null;
    }
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
    public void listSentences()
    {
        Console.WriteLine("Knowledge Base of "+ this.name +":");
        for (int i = 0; i < kBase.Count; i++)
        {
            Console.WriteLine(i.ToString()+". "+ kBase[i].printString());
        }
    }
}