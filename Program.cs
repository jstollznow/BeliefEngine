using System;

namespace BeliefEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] options={"Print belief base",
            "Enter logic sentence", "Exit"};
            while(true){
                switch(menuOption(options)){
                    case 1:

                    break;
                    case 2:
                        Console.WriteLine("Please enter a logic sentence: "+Environment.NewLine);
                        string val = Console.ReadLine();
                        Sentence test = new Sentence(val);
                    break;
                    default:
                        return;
                };
            }

        }
        public static int menuOption(string[] options){
            string menu=string.Empty;
            for(int i=0;i<options.Length;i++){
                menu=menu+(i+1).ToString()+". "+options[i]+Environment.NewLine;
            }
            menu=menu+"Please select an option, (1,2,...)"+Environment.NewLine;
            Console.Write(menu);
            string val = Console.ReadLine();
            int option = Convert.ToInt32(val);
            return option;
        }
    }
}
