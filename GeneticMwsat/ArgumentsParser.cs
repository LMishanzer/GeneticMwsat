namespace GeneticMwsat;

public static class ArgumentsParser
{
    public static Configuration? Parse(string[] args)
    {
        var configuration = new Configuration();
        
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-m":
                    configuration.PopulationSize = int.Parse(args[++i]);
                    break;
                case "-M":
                    configuration.MutationProbability = double.Parse(args[++i]);
                    break;
                case "-i":
                    configuration.GenerationCount = int.Parse(args[++i]);
                    break;
                case "-s":
                    configuration.SelectionLimit = int.Parse(args[++i]);
                    break;
                case "-t":
                    configuration.TournamentSize = int.Parse(args[++i]);
                    break;
                case "-o":
                    configuration.OutputFileName = args[++i];
                    break;
                case "-c":
                    configuration.OptimalScoreFilePath = args[++i];
                    break;
                case "-e":
                    configuration.EliteCount = int.Parse(args[++i]);
                    break;
                case "-w":
                    configuration.ChangeMutationAfter = int.Parse(args[++i]);
                    break;
                case "--help":
                case "-h":
                    ShowHelp();
                    return null;
                default:
                    configuration.InputFilePath = args[i];
                    break;
            }
        }

        return configuration;
    }

    private static void ShowHelp()
    {
        Console.WriteLine("Usage: mwsat [OPTIONS] FILENAME");
        Console.WriteLine("Available options:");
        Console.WriteLine("-m size       population size");
        Console.WriteLine("-M prob       mutation probability");
        Console.WriteLine("-M count      generations count");
        Console.WriteLine("-s limit      parent selection limit");
        Console.WriteLine("-t size       tournament size");
        Console.WriteLine("-o path       detailed results output file path");
    }
}