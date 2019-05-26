

public class Clause
{
    public Clause(Sentence mySentence)
    {
        // this will be a conversion from a sentence to a clause
        // the sentence will be something like sentence && sentence
        // the clause form will just convert those sentences into propositions and operators
        // but the only operators of the clause should be OR
        // can't add negatives to propositions (deliberately) as they are globals
        // so when you change the value of 'p' it changes all of them
    }
}