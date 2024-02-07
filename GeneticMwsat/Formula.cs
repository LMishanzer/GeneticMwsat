using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeneticMwsat;

public class Formula
{
    private readonly List<Clause> _clauses = new();
    public int VariableCount { get; set; }
    public int ClauseCount { private get; set; }

    public void AddClause(Clause clause)
    {
        _clauses.Add(clause);
    }

    public void Verify()
    {
        if (_clauses.Count != ClauseCount)
            throw new ValidationException("Formula isn't valid. The number of clauses is wrong.");

        foreach (var clause in _clauses)
            clause.Verify(VariableCount);
    }

    public EvaluationResult Evaluate(Instance instance)
    {
        var result = true;
        var successfulCount = 0;
        
        foreach (var clause in _clauses)
        {
            var clauseRes = clause.Evaluate(instance);
            result = result && clauseRes;

            if (clauseRes)
                successfulCount++;
        }

        return new EvaluationResult
        {
            FormulaSatisfied = result,
            SatisfiedClauses = successfulCount
        };
    }
}