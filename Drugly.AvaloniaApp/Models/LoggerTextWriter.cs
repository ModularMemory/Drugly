using System.Text;
using Drugly.AvaloniaApp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Drugly.AvaloniaApp.Models;

// Source - https://stackoverflow.com/a
// Posted by d_f
// Retrieved 2026-02-04, License - CC BY-SA 4.0
public class LoggerTextWriter : TextWriter
{
    private readonly LogEventLevel _logLevel;
    private readonly ILogger _logger;
    private readonly StringBuilder _builder = new();
    private bool _terminatorStarted;

    public LoggerTextWriter([ServiceKey] LogEventLevel logLevel, ILogger logger)
    {
        _logLevel = logLevel;
        _logger = logger;
    }

    public override void Write(string? value)
    {
        if (!_logger.IsEnabled(_logLevel))
        {
            return;
        }

        if (value is { Length: > 0 } && value != NewLine)
        {
            FlushBuilder();

#pragma warning disable CA2254
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            _logger.Write(_logLevel, value.TrimEnd(CoreNewLine));
#pragma warning restore CA2254
        }
    }

    public override void Write(char value)
    {
        if (!_logger.IsEnabled(_logLevel))
        {
            return;
        }

        _builder.Append(value);

        if (value == NewLine[0])
        {
            // newline is just '\n'
            if (NewLine.Length == 1)
            {
                FlushBuilder();
                return;
            }

            _terminatorStarted = true;
        }
        else if (_terminatorStarted)
        {
            _terminatorStarted = value == NewLine[1];

            // value is closing newline char
            if (_terminatorStarted)
            {
                FlushBuilder();
            }
        }
    }

    private void FlushBuilder()
    {
        _builder.TrimEnd(CoreNewLine);

        if (_builder.Length > NewLine.Length)
        {
#pragma warning disable CA2254
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            _logger.Write(_logLevel, _builder.ToString());
#pragma warning restore CA2254
        }

        _builder.Clear();
        _terminatorStarted = false;
    }

    public override Encoding Encoding => Encoding.Unicode;
}