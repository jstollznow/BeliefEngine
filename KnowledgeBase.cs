using System;

using System.Collections.Generic;

public class KnowledgeBase{
    List<Sentence> kBase = new List<Sentence>();
    string name;
    public KnowledgeBase(string name){
        this.name = name;
    }

    public void TELL(Sentence newSentence){
        kBase.Add(newSentence);
    }
    public Sentence ASK(){
        return null;
    }
    private void checkValidity(Sentence newSentence){

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