namespace GeneticMwsat;

public class Configuration
{
    public int GenerationCount { get; set; } = 10_000;
    public int PopulationSize { get; set; } = 1000;
    public double MutationProbability { get; set; } = 0.05;
    public string InputFilePath { get; set; } = string.Empty;
    public string OutputFileName { get; set; } = string.Empty;
    public string? OptimalScoreFilePath { get; set; }
    public int SelectionLimit { get; set; } = 50;
    public int TournamentSize { get; set; } = 6;
    public int EliteCount { get; set; }
    public int ChangeMutationAfter { get; set; } = 100;
}