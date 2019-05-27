using System;
using System.Collections.Generic;
using static GlobalProps;

namespace BeliefEngine
{
    class Program
    {
        public static KnowledgeBase ME = new KnowledgeBase("Jacob");
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.NewLine + 
            "Welcome to the Belief Revision Engine! How may I help you?" + 
            Environment.NewLine);
            string[] options = {"Print belief base",
            "Add sentence to belief base", "Entailment" ,"Exit"};
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
                                ME.TELL(logicSent);
                            }
                        }
                    break;
                    case 3:
                        Console.WriteLine("Please enter a logic sentence: ");
                        string line = Console.ReadLine();
                        Sentence entailSent = new Sentence(line);
                        if (ME.checkEntailment(entailSent))
                        {
                            Console.WriteLine("The sentence is entails the belief base");
                        }
                        else
                        {
                            Console.WriteLine("The sentence does not entail the belief base.");
                        }
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
            Console.Write(Environment.NewLine);
            return option;
        }
    }
    
}
