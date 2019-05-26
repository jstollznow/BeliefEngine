using System.Collections.Generic;

public class TruthTable
{
    TruthElement[,] table;
    public TruthTable(List<Sentence> sentences)
    {
        
    }
    
    public void GenerateTable(List<Sentence> sentences)
    {
        int rows = GlobalProps.props.Count - 1;
        int cols = sentences.Count - 1;
        table = new TruthElement[rows,cols];
        
    }
}