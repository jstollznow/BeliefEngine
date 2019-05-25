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
    }
    public Sentence(){

    }
    public void bracketSplit()
    {
        int lCount = 0;
        int lLast = -1;
        int rCount = 0;
        int rLast = -1;
        if (input != null){
            for(int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]) == false)
                {
                    switch (input[i])
                    {
                        case '(':
                            lCount ++;
                            if (lCount==1){
                                lLast = i;
                            }

                        break;
                        case ')':
                            rCount ++;
                            if (rCount == 1)
                            {
                                rLast = i;
                            }

                        break;
                    }
                    if (rLast != -1 && lLast != -1){
                        // operator to the left or to the right
                        
                        int j;
                        // if the rest of the eqn is to the left
                        if (input.Length>(rLast-lLast+1) && lCount > 1){
                            if (input.Length <= (rLast+1))
                            {
                                b = new Sentence(input.Substring(lLast + 1, rLast - lLast - 1));
                                input = input.Remove(lLast, rLast + 1 - lLast);
                                for (j = lLast-1; j >= 0; j --)
                                {
                                    if (char.IsLetter(input[j]) == true)
                                    {
                                        break;
                                    }
                                }
                                join = new Operator(input.Substring(j+1, lLast-1));
                                a = new Sentence(input.Substring(0, j+1));
                            }
                            // to the right
                            else
                            {
                                a = new Sentence(input.Substring(lLast+1,rLast-lLast-1));
                                input = input.Remove(lLast, rLast + 1 - lLast);
                                for (j = lLast; j < input.Length; j ++)
                                {
                                    if (char.IsLetter(input[j]) == true)
                                    {
                                        break;
                                    }
                                }
                                join = new Operator(input.Substring(lLast, j-1));
                                b = new Sentence(input.Substring(j-1));
                            }
                            input = null;
                            return;
                        }
                        else{
                            // trim unnecessary brackets for 
                            input=input.Substring(1,rLast-1);
                        }
                    }
                    
                }
                
            }

        }
    }
    public void operatorSplit()
    {
        int opStart = -1;
        int opEnd = -1;
        if (input != null){
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
                        join = new Operator(input.Substring(opStart, count - opStart));
                        b = new Sentence(input.Substring(count));
                        input = null;
                        return;
                    }
                }
            }
        }
    }
    public void propSplit()
    {
        if (input != null){
            a = new Proposition(input);
        }
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