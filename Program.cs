using System;
using System.Collections.Generic;
using static GlobalProps;

namespace BeliefEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] options={"Print belief base",
            "Enter logic sentence", "Exit"};
            props = new List<Proposition>();
            while(true){
                switch(menuOption(options))
                {
                    case 1:
                        generateBoolValues(4);
                    break;
                    case 2:
                        Console.WriteLine("Please enter a logic sentence: "+Environment.NewLine);
                        string val = Console.ReadLine();
                        Sentence test = new Sentence(val);
                        KnowledgeBase ME = new KnowledgeBase("Jacob");
                        ME.TELL(test);
                        ME.listSentences();
                        bool[] propValues = new bool[] {true, true};
                        // propositions in globalProps are a list of letters
                        // the letters are set in the order they are created
                        // i.e. if you use p then q
                        // you would send an array of 2 to setPropositions
                        // array[0] would set p, and array[1] would set q
                        setPropositions(propValues);
                        Console.WriteLine("When " + propositions() + " are equal to " + 
                        propValues.GetValue(0) + " and " + propValues.GetValue(1) + ". Test is " + test.getValue());
                        // each sentence will account for the globally set values and return an answer
                        // you could use this method to rotate the values and hence generate a truth table for a 
                        // given sentence, in the knowledge base, then this truth table could be used
                        // when trying to add a new sentence
                    break;
                    default:
                        return;
                };
            }

        }
        public static int menuOption(string[] options)
        {
            string menu = string.Empty;
            for(int i = 0; i < options.Length; i++)
            {
                menu = menu + (i + 1).ToString()+". "+options[i] + Environment.NewLine;
            }
            menu = menu + "Please select an option, (1,2,...): " + Environment.NewLine;
            Console.Write(menu);
            string val = Console.ReadLine();
            int option;
            while (int.TryParse(val,out option)==false)
            {
                 Console.WriteLine("Please enter a valid number: ");
                 val = Console.ReadLine();   
            }
            return option;
        }
        public static void generateBoolValues(int numProps)
        {
            // like a bit counter
            List<bool[]> values = new List<bool[]>();
            bool[] firstEntry = new bool[numProps];
            for (int i = 0; i < numProps; i++)
            {
                firstEntry[i] = false;
            }
            values.Add(firstEntry);
            for (int i = 1; i < Math.Pow(2, numProps); i++)
            {
                bool[] newEntry = new bool[numProps];
                Array.Copy(values[i-1],newEntry,numProps);
                // newEntry = values[i - 1];
                if (newEntry[numProps-1])
                {
                    newEntry[numProps-1] = false;
                    for (int j = 1; j >= 0; j--)
                    {
                        if (newEntry[j] == false)
                        {
                            newEntry[j] = true;
                            for (int carry = j + 1; carry < numProps; carry++)
                            {
                                newEntry[carry] = false;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    newEntry[numProps-1] = true;
                }
                values.Add(newEntry);
            }
            for (int i = 0; i < values.Count; i++)
            {   
                for (int j = 0; j < numProps; j++)
                {
                    Console.Write(values[i][j] + " ");
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
    
}
