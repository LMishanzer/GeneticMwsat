namespace GeneticMwsat;

public class Configuration
{
    public int GenerationCount { get; set; } = 1_000_000;
    public int PopulationSize { get; set; } = 1000;
    public double MutationProbability { get; set; } = 0.05;
    public string InputFileName { get; set; } = string.Empty;
    public string OutputFileName { get; set; } = string.Empty;
    public int SelectionLimit { get; set; } = 50;
    public int TournamentSize { get; set; } = 6;
}