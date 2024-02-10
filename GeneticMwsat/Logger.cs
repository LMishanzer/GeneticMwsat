namespace GeneticMwsat;

public class Logger : IDisposable
{
    private readonly FileStream? _outputFile;
    private readonly StreamWriter? _streamWriter;

    public bool LogToFileEnabled { get; init; }
    public int? OptimalFitness { get; set; }

    public Logger()
    {
        LogToFileEnabled = false;
    }

    public Logger(string outputFileName)
    {
        _outputFile = File.OpenWrite(outputFileName);
        _streamWriter = new StreamWriter(_outputFile);
        LogToFileEnabled = true;
    }
    
    public void Log(string text)
    {
        text = OptimalFitness == null ? text : $"{text} {OptimalFitness}";
        _streamWriter?.Write($"{text}{Environment.NewLine}");
    }

    public void LogToConsole(string text)
    {
        text = OptimalFitness == null ? text : $"{text} {OptimalFitness}"; 
        Console.WriteLine(text);
    }

    public void Dispose()
    {
        _streamWriter?.Flush();
        _streamWriter?.Dispose();
        _outputFile?.Dispose();
    }
}