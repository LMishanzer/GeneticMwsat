using GeneticMwsat;

var configuration = ArgumentsParser.Parse(args);
var formula = DimaxReader.ReadFile(configuration.InputFileName);

formula.Verify();

Logger? logger = null;
if (!string.IsNullOrWhiteSpace(configuration.OutputFileName))
{
    logger = new Logger(configuration.OutputFileName);
}

var geneticAlgorithm = new GeneticAlgorithm(configuration, logger);
geneticAlgorithm.Run(formula);

logger?.Dispose();

// var variables = new bool[formula.VariableCount];
//
// var totalMax = 0;
//
// for (long i = (long) Math.Pow(2, formula.VariableCount) - 1; i >= 0; i--)
// {
//     for (var j = 0; j < variables.Length; j++)
//     {
//         variables[j] = ((i >> j) & 1) == 1;
//     }
//
//     var result = formula.Evaluate(new Instance(variables));
//     
//     if (result.FormulaSatisfied && totalMax < variables.Count(v => v))
//     {
//         totalMax = variables.Count(v => v);
//         Console.WriteLine(totalMax);
//         Console.WriteLine(string.Join(" ", variables));
//     }
// }

// var variables = new[] { true, false, false, true, false, true, false, false, false, true, false, false, true, true, true, false, true, false, false, true };
// var result = formula.Evaluate(new Instance(variables));
//
// Console.WriteLine($"{result.FormulaSatisfied} {result.SatisfiedClauses}");
