# BeliefEngine
This belief agent was written in C# and has a terminal line user interface. The belief revision can print the belief base, take in new information and test the logical entailment of a given sentence. 
The syntax used to interact with the belief base is as follows:
AND: a&&b
OR: a||b
NEGATION: ~a
IMPLICATION: a->b 
BICONDITIONAL: a<>b

Propositional sentences should also be typed in without any white space. For example a -> b would not be an accepted input. 