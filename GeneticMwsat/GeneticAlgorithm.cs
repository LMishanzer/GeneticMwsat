using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMwsat;

public class GeneticAlgorithm
{
    private readonly Logger? _logger;
    private readonly int _maxGenerationCount;
    private readonly int _populationSize;
    private readonly double _mutationProbability;
    private readonly int _selectionLimit;
    private readonly int _tournamentSize;

    private List<Chromosome> _population = new();
    
    public GeneticAlgorithm(Configuration configuration, Logger? logger = null)
    {
        _logger = logger;
        _maxGenerationCount = configuration.GenerationCount;
        _populationSize = configuration.PopulationSize;
        _mutationProbability = configuration.MutationProbability;
        _selectionLimit = configuration.SelectionLimit;
        _tournamentSize = configuration.TournamentSize;
    }
    
    public void Run(Formula formula)
    {
        GeneratePopulation(formula.VariableCount);
        CalculateFitness(formula);

        var generation = 0;
        
        GatherStats(generation);

        for (int i = 0; i < _maxGenerationCount; i++)
        {
            generation++;
            
            var parents = SelectParents();
            _population = CrossoverParents(parents);
            PerformMutation();
            CalculateFitness(formula);

            GatherStats(generation);
 
            if (_lastMax == formula.VariableCount)
                return;
        }

        Console.WriteLine(_lastMax);
    }

    private int _lastMax;
    private int _maxFitness;

    private void GatherStats(int generation)
    {
        var populationFitness = _population.Sum(c => c.Fitness);
        var maxFitnessChromosome = _population.MaxBy(c => c.Fitness);
        var maxPositivesCount = maxFitnessChromosome.GetPositivesCount();
        var formulaSatisfied = maxFitnessChromosome.FormulaSatisfied ? 1 : 0;
        
        _logger?.Log($"{generation}: {populationFitness} {maxFitnessChromosome.Fitness} {maxPositivesCount} {formulaSatisfied}");
        
        if (maxFitnessChromosome.FormulaSatisfied && maxPositivesCount > _lastMax)
        {
            _lastMax = maxPositivesCount;
            Console.WriteLine($"{generation}: {populationFitness} {maxFitnessChromosome.Fitness} {maxPositivesCount} {formulaSatisfied}");
            Console.WriteLine(string.Join(" ", maxFitnessChromosome.Genes).ToLower());
        }

        if (_maxFitness < maxFitnessChromosome.Fitness)
        {
            Console.WriteLine($"{generation}: {populationFitness} {maxFitnessChromosome.Fitness} {maxPositivesCount} {formulaSatisfied}");
            _maxFitness = maxFitnessChromosome.Fitness;
        }
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

            chromosome.Fitness = result.FormulaSatisfied ? chromosome.GetPositivesCount() * 10 : 0;
            chromosome.Fitness += result.SatisfiedClauses;

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

            if ((best == null || best.Value.Fitness < randChromosome.Fitness) && !winners.Contains(randChromosome))
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
            var rand1 = Random.Shared.Next(parents.Count);
            var rand2 = Random.Shared.Next(parents.Count);

            var child = parents[rand1].Crossover(parents[rand2]);
            resultPopulation.Add(child);
        }

        return resultPopulation;
    }
}