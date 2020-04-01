using System;
using System.Collections.Generic;
using static GlobalProps;

namespace BeliefEngine
{
    /** Main program - controls the terminal line GUI*/
    class Program
    {
        public static BeliefBase ME = new BeliefBase("Jacob");
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.NewLine + 
            "Welcome to the Belief Revision Engine! How may I help you?" + 
            Environment.NewLine);
            string[] options = {"Print belief base",
            "Add sentence to belief base (Partial Meet Contraction)", "Entailment (Truth Tables)", "Exit"};
            props = new List<Proposition>();
            while(true){
                switch(menuOption(options))
                {
                    case 1:
                        ME.listSentences();
                    break;
                    case 2:
                        while(true)
                        {
                            Console.WriteLine("Please enter a logic sentence (type -1 to stop adding): ");
                            string input = Console.ReadLine();
                            Console.Write(Environment.NewLine);
                            if (input == "-1"){break;}
                            Sentence logicSent = new Sentence(input);
                            if (logicSent.IsValid)
                            {
                                //simplify sentence to CNF and add to KB
                                logicSent.simplfy();
                                ME.TELL(logicSent);
                                ME.printBase();
                            }
                        }
                    break;
                    case 3:
                        Console.WriteLine("Please enter a logic sentence: ");
                        string line = Console.ReadLine();
                        Sentence entailSent = new Sentence(line);
                        //Check if a propositional sentence entails the KB
                        if (ME.checkEntailment(entailSent))
                        {
                            Console.WriteLine("The sentence entails the belief base" + Environment.NewLine);
                        }
                        else
                        {
                            Console.WriteLine("The sentence does not entail the belief base." + Environment.NewLine);
                        }
                    break;
                    case 4:
                        return;
                    default:
                        break;
                };
            }

        }
        public static int menuOption(string[] options)
        {
            string menu = string.Empty;
            for(int i = 0; i < options.Length; i++)
            {
                menu = menu + (i + 1).ToString() + ". " + options[i] + Environment.NewLine;
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
            Console.Write(Environment.NewLine);
            return option;
        }
    }
    
}
