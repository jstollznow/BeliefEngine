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
        bool alreadyExists = false;
        foreach (var sentence in kBase) {
            if (sentence.ToString() == newSentence.ToString()) {
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
            Console.WriteLine((i + 1).ToString()+". "+ kBase[i].printString());
        }
        if (kBase.Count == 0)
        {
            Console.WriteLine("[KB is empty]");
        }
        Console.Write(Environment.NewLine);
    }
}