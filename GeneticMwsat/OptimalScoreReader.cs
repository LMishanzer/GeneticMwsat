namespace GeneticMwsat;

public class OptimalScoreReader
{
    public static int? OptimalFitness(string inputFilePath, string scoreFileName)
    {
        var fileName = Path.GetFileName(inputFilePath);
        var resultToFind = fileName[1..^6];

        var lines = File.ReadAllLines(scoreFileName);

        foreach (var line in lines)
        {
            var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (values[0].Equals(resultToFind))
                return int.Parse(values[1]);
        }

        return null;
    }
}