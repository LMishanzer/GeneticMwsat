using System;

namespace GeneticMwsat;

public struct Chromosome
{
    public bool[] Genes { get; }
    public int Fitness { get; set; }
    public bool FormulaSatisfied { get; set; }

    public Chromosome(bool[] genes)
    {
        Genes = genes;
    }

    public static Chromosome GetRandom(int chromosomeSize)
    {
        var chromosome = new Chromosome(new bool[chromosomeSize]);

        for (int i = 0; i < chromosomeSize; i++)
        {
            chromosome.Genes[i] = Random.Shared.Next(100) > 50;
        }

        return chromosome;
    }

    public Chromosome Crossover(Chromosome another)
    {
        var splitIndex = Random.Shared.Next(Genes.Length);

        var newGenes = new bool[Genes.Length];
        var currentGenes = Genes;

        for (int i = 0; i < Genes.Length; i++)
        {
            newGenes[i] = currentGenes[i];

            if (i == splitIndex)
            {
                currentGenes = another.Genes;
            }
        }

        return new Chromosome(newGenes);
    }

    public void PerformMutation(double probability)
    {
        const int accuracy = 1_000_000;
        var correctedProbability = probability * accuracy;
        for (int i = 0; i < Genes.Length; i++)
        {
            if (Random.Shared.Next(accuracy) <= correctedProbability)
                Genes[i] = !Genes[i];
        }
    }

    public int GetPositivesCount()
    {
        int positives = 0;
        for (int i = 0; i < Genes.Length; i++)
        {
            if (Genes[i])
                positives++;
        }

        return positives;
    }
}