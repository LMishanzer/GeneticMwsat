using System;
using System.IO;

namespace GeneticMwsat;

public static class DimaxReader
{
    public static Formula ReadFile(string filename)
    {
        var formula = new Formula();
        var lines = File.ReadAllLines(filename);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('c'))
                continue;
            
            var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (line.StartsWith('p'))
            {
                var variableCount = int.Parse(values[2]);
                var clausesCount = int.Parse(values[3]);

                formula.VariableCount = variableCount;
                formula.ClauseCount = clausesCount;
                
                continue;
            }

            var firstParameter = int.Parse(values[0]);
            var secondParameter = int.Parse(values[1]);
            var thirdParameter = int.Parse(values[2]);

            var clause = new Clause(firstParameter, secondParameter, thirdParameter);
            formula.AddClause(clause);
        }

        return formula;
    }
}