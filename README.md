# BeliefEngine
This belief agent was written in C# and has a terminal line user interface. The belief revision can print the belief base, take in new information and test the logical entailment of a given sentence. 
The syntax used to interact with the belief base is as follows:\n
AND: a&&b\n
OR: a||b\n
NEGATION: ~a\n
IMPLICATION: a->b \n
BICONDITIONAL: a<>b\n

Propositional sentences should also be typed in without any white space. For example a -> b would not be an accepted input. 