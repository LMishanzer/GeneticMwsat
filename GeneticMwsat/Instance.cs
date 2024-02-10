namespace GeneticMwsat;

public struct Instance
{
    private readonly bool[] _variables;

    public Instance(bool[] variables)
    {
        _variables = variables;
    }

    public bool GetVariableValue(int index)
    {
        if (index <= 0 || index > _variables.Length)
            throw new ArgumentOutOfRangeException("index", "Index is out of range.");
        
        return _variables[index - 1];
    }
}