using System.ComponentModel.DataAnnotations;

namespace GeneticMwsat;

public struct Clause
{
    private readonly int[] _parameters;
    
    public Clause(int firstVar, int secondVar, int thirdVar)
    {
        _parameters = [firstVar, secondVar, thirdVar];
    }

    public bool Evaluate(Instance instance)
    {
        var result = false;

        foreach (var parameter in _parameters)
        {
            var variableValue = instance.GetVariableValue(Math.Abs(parameter));
            var evaluated = parameter < 0 ? !variableValue : variableValue;
            result = result || evaluated;
        }

        return result;
    }

    public void Verify(int variablesCount)
    {
        foreach (var parameter in _parameters)
        {
            if (parameter == 0 || Math.Abs(parameter) > variablesCount)
                throw new ValidationException($"Invalid variable: {parameter}");
        }
    }
}