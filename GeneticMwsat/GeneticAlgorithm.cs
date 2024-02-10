namespace GeneticMwsat;

public class GeneticAlgorithm
{
    private readonly Logger _logger;
    private readonly int _maxGenerationCount;
    private readonly int _populationSize;
    private double _mutationProbability;
    private readonly double _originalMutationProbability;
    private readonly int _selectionLimit;
    private readonly int _tournamentSize;
    private readonly int _eliteCount;
    private readonly int _changeMutationAfter;

    private List<Chromosome> _population = [];
    
    public GeneticAlgorithm(Configuration configuration, Logger logger)
    {
        _logger = logger;
        _maxGenerationCount = configuration.GenerationCount;
        _populationSize = configuration.PopulationSize;
        _mutationProbability = configuration.MutationProbability;
        _originalMutationProbability = configuration.MutationProbability;
        _selectionLimit = configuration.SelectionLimit;
        _tournamentSize = configuration.TournamentSize;
        _eliteCount = configuration.EliteCount;
        _changeMutationAfter = configuration.ChangeMutationAfter;
    }
    
    public void Run(Formula formula)
    {
        GeneratePopulation(formula.VariableCount);
        CalculateFitness(formula);

        var generation = 0;
        
        GatherStats(generation, formula.ClauseCount);

        for (int i = 1; i <= _maxGenerationCount; i++)
        {
            generation++;
            
            var parents = SelectParents();
            var children = CrossoverParents(parents);

            ReplacePopulation(children);
            
            PerformMutation();
            CalculateFitness(formula);

            GatherStats(generation, formula.ClauseCount);
        }
    }

    private void ReplacePopulation(List<Chromosome> children)
    {
        var newPopulation = new List<Chromosome>(capacity: _populationSize);
        
        if (_eliteCount > 0)
        {
            var elite = _population.OrderBy(c => c.Rating).Take(_eliteCount);
            newPopulation.AddRange(elite);
        }
        
        newPopulation.AddRange(children);

        _population = newPopulation;
    }

    private Chromosome _maxFitnessChromosome;
    private int _formulaSatisfied;
    private int _generation;
    private int _generationsWithoutChange;

    private void GatherStats(int generation, int maxClausesCount)
    {
        _generationsWithoutChange++;
        
        var maxFitnessChromosome = _population.MaxBy(c => c.Fitness);

        if (maxFitnessChromosome.Rating > _maxFitnessChromosome.Rating)
        {
            _maxFitnessChromosome = maxFitnessChromosome;
            _formulaSatisfied = maxFitnessChromosome.FormulaSatisfied ? 1 : 0;
            _generation = generation;
            _generationsWithoutChange = 0;
            _mutationProbability = _originalMutationProbability;
        }

        if (_generationsWithoutChange >= _changeMutationAfter)
        {
            _mutationProbability *= 2;
            _generationsWithoutChange = 0;
        }
        
        if (generation == _maxGenerationCount) 
            _logger.LogToConsole($"{_generation} {_maxFitnessChromosome.Fitness} {maxClausesCount} {_formulaSatisfied} {_maxFitnessChromosome.SecondaryFitness}");
        
        if (!_logger.LogToFileEnabled)
            return;

        var minFitness = _population.Min(c => c.Fitness);
        var averageFitness = _population.Average(c => c.Fitness);
        
        _logger.Log($"{generation} {minFitness} {averageFitness} {maxFitnessChromosome.Fitness} {_maxFitnessChromosome.Fitness} {maxClausesCount} {_formulaSatisfied} {_maxFitnessChromosome.SecondaryFitness}");
    }

    private void PerformMutation()
    {
        foreach (var chromosome in _population)
        {
            chromosome.PerformMutation(_mutationProbability);
        }
    }

    private void GeneratePopulation(int chromosomeSize)
    {
        _population.Clear();
        
        for (int i = 0; i < _populationSize; i++)
        {
            _population.Add(Chromosome.GetRandom(chromosomeSize));
        }
    }

    private void CalculateFitness(Formula formula)
    {
        for (var index = 0; index < _population.Count; index++)
        {
            var chromosome = _population[index];
            var instance = new Instance(chromosome.Genes);
            var result = formula.Evaluate(instance);

            chromosome.Fitness = result.FormulaSatisfied ? chromosome.GetFitness(formula.Weights) : 0;

            chromosome.SecondaryFitness = result.SatisfiedClauses;
            chromosome.FormulaSatisfied = result.FormulaSatisfied;

            _population[index] = chromosome;
        }
    }

    private List<Chromosome> SelectParents()
    {
        var result = new HashSet<Chromosome>(capacity: _selectionLimit);
        
        for (int i = 0; i < _selectionLimit; i++)
        {
            var winner = MakeTournament(result);
            result.Add(winner);
        }

        return result.ToList();
    }

    private Chromosome MakeTournament(HashSet<Chromosome> winners)
    {
        Chromosome? best = null;
        for (int j = 0; j < _tournamentSize; j++)
        {
            var index = Random.Shared.Next(_population.Count);
            var randChromosome = _population[index];

            if ((best == null || randChromosome.Rating > best.Value.Rating) && !winners.Contains(randChromosome))
            {
                best = randChromosome;
            }
        }

        return best ?? MakeTournament(winners);
    }

    private List<Chromosome> CrossoverParents(List<Chromosome> parents)
    {
        var resultPopulation = new List<Chromosome>(capacity: _populationSize);
        
        for (int i = 0; i < _populationSize; i++)
        {
            var (index1, index2) = GetTwoRandomIndexes(parents.Count);
            
            var parent1 = parents[index1];
            var parent2 = parents[index2];
            
            var child = parent1.Crossover(parent2);
            resultPopulation.Add(child);
        }

        return resultPopulation;
    }

    private static (int First, int Second) GetTwoRandomIndexes(int upperBound)
    {
        int first = 0 , second = 0;

        while (first == second)
        {
            first = Random.Shared.Next(upperBound);
            second = Random.Shared.Next(upperBound);
        }

        return (first, second);
    }
}