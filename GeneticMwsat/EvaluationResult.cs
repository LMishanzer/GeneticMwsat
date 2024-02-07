namespace GeneticMwsat;

public struct EvaluationResult
{
    public bool FormulaSatisfied { get; set; }
    public int SatisfiedClauses { get; set; }
}