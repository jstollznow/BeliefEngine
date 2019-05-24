class Sentence{
    Sentence(Sentence a, Operator join=null,Sentence b=null){
            if (join==null){
                Proposition a;

            }
    }
    Sentence(string input){
        
        // Sentence()
    }
    public string[] split(string input)
    {
        int lCount = 0;
        int rCount = 0;
        for(int i = 0; i < input.Length; i++)
        {
            if (char.IsLetter(input[i]) == false)
            {
                switch (input[i])
                {
                    case '(':
                        lCount ++;
                    break;
                    case ')':
                        rCount ++;

                    break;
                }
                   
            }
        }
    }
}