﻿using System;
using System.Collections.Generic;
using System.Text;

using Astron.Logging;
using Astron.Logging.Strategy;

namespace Astron.Unpacker.Logging
{
    public class ConsoleOutputStrategy : LoggingStrategy
    {
        private readonly Dictionary<LogLevel, ConsoleColor> _colorWrapper = new Dictionary<LogLevel, ConsoleColor>()
        {
            {LogLevel.Trace, ConsoleColor.DarkGreen },
            {LogLevel.Debug, ConsoleColor.Green },
            {LogLevel.Info, ConsoleColor.Blue },
            {LogLevel.Warn, ConsoleColor.DarkYellow },
            {LogLevel.Error, ConsoleColor.Red },
            {LogLevel.Fatal, ConsoleColor.DarkRed }
        };

        public ConsoleOutputStrategy(bool printDate, LogLevel minLevelToPrint = LogLevel.Info)
            : base(new LogConfig(printDate, minLevelToPrint, LogLevel.None))
        {
        }

        protected override void Log(LogLevel level, string formattedHeader, string message)
        {
            if (Console.ForegroundColor != _colorWrapper[level])
                Console.ForegroundColor = _colorWrapper[level];

            Console.WriteLine($"{formattedHeader} {message}");
        }

        protected override void Log<T>(LogLevel level, string formattedHeader, string message, T instance)
        {
        }

        protected override void OnMinLevel(LogLevel level, string formattedHeader, string message)
        {
            // don't write min level
        }

        protected override void OnMaxLevel(LogLevel level, string formattedHeader, string message)
        {
            Console.WriteLine(); // write empty line on None log level
        }

        public override void Save()
        {
        }
    }
}
