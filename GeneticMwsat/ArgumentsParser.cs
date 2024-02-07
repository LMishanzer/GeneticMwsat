namespace GeneticMwsat;

public static class ArgumentsParser
{
    public static Configuration Parse(string[] args)
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
                default:
                    configuration.InputFileName = args[i];
                    break;
            }
        }

        return configuration;
    }
}