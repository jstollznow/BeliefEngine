using System;
public class Sentence{
    Sentence a;
    Sentence b;
    Operator join;
    string input;

    public Sentence(Sentence a, Operator join=null,Sentence b=null){
        this.a=a;
        this.b=b;
        this.join=join;
            if (join==null){
                // Proposition a;

            }
    }
    public Sentence(string input)
    {
        this.input = input;
        bracketSplit();
        operatorSplit();
        propSplit();
        // Sentence()
        Console.WriteLine(this.printString());
    }
    public void bracketSplit()
    {
        int lCount = 0;
        int lLast = 0;
        int rCount = 0;
        int rLast = 0;
        for(int i = 0; i < input.Length; i++)
        {
            if (char.IsLetter(input[i]) == false)
            {
                switch (input[i])
                {
                    case '(':
                        lCount ++;
                        lLast = i;

                    break;
                    case ')':
                        rCount ++;
                        rLast = i;

                    break;
                }
                if (lCount == rCount){
                    a = new Sentence(input.Substring(lLast+1,rLast-lLast));
                    input = input.Remove(rLast+1);
                    int j;
                    for (j = 0; j < input.Length; j++){
                        if (char.IsLetter(input[i])==true){
                            break;
                        }
                    }
                    join = new Operator(input.Substring(0,j-1));
                    b = new Sentence(input.Substring(j-1));
                    input = null;
                    break;
                }
                   
            }
            
        }
    }
    public void operatorSplit()
    {
        int opStart = -1;
        int opEnd = -1;
        for (int count = 0; count < input.Length; count++)
        {
            if (char.IsLetter(input[count]) == false)
            {
                if (opStart == -1)
                {
                    opStart = count;
                }
            }
            else
            {
                if (opStart != -1)
                {
                    a = new Sentence(input.Substring(0, opStart));
                    join = new Operator(input.Substring(opStart, count - opStart - 1));
                    b = new Sentence(input.Substring(count));
                    break;
                }
            }
        }
    }
    public void propSplit(){
        a = new Proposition(false,input);
    }
    public virtual string printString(){
        string sent = null;
        if (b!=null){
            sent = "(" + a.printString() + ")" + join.printString() + "(" + b.printString() + ")";
        }
        else
        {
            sent = a.printString();
        }
        return sent;
    }
}