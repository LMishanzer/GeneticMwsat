using System;
using System.IO;

namespace GeneticMwsat;

public class Logger : IDisposable
{
    private readonly FileStream _outputFile;
    private readonly StreamWriter _streamWriter;

    public Logger(string outputFileName)
    {
        _outputFile = File.OpenWrite(outputFileName);
        _streamWriter = new StreamWriter(_outputFile);
    }
    
    public void Log(string text)
    {
        _streamWriter.Write($"{text}{Environment.NewLine}");
    }

    public void Dispose()
    {
        _streamWriter.Flush();
        _streamWriter.Dispose();
        _outputFile.Dispose();
    }
}