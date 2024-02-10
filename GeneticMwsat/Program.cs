using GeneticMwsat;

var configuration = ArgumentsParser.Parse(args);

if (configuration == null)
    return;

var formula = DimaxReader.ReadFile(configuration.InputFilePath);

formula.Verify();

using var logger = string.IsNullOrWhiteSpace(configuration.OutputFileName) ? new Logger() : new Logger(configuration.OutputFileName);

if (!string.IsNullOrWhiteSpace(configuration.OptimalScoreFilePath))
    logger.OptimalFitness = OptimalScoreReader.OptimalFitness(configuration.InputFilePath, configuration.OptimalScoreFilePath);

var geneticAlgorithm = new GeneticAlgorithm(configuration, logger);
geneticAlgorithm.Run(formula);
