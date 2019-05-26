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
        if (isValid(newSentence))
        {
            kBase.Add(newSentence);
            
        }
    }
    public Sentence ASK(){
        return null;
    }
    private bool isValid(Sentence newSentence){
        return false;
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